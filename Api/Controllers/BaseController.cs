using Api.Infrastructure;
using Application;
using AutoMapper;
using Domain.Exceptions.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ApiExceptions;
using Shared.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace Api.Controllers
{
	[Route("account/api/v{version:apiVersion}/[controller]")]
	[Consumes(ApiConstants.ApplicationJsonMediaType)]
	[Produces(ApiConstants.ApplicationJsonMediaType, ApiConstants.ProblemDetailsMediaType)]
	[ProducesResponseType(typeof(AccountApiProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(AccountApiProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(AccountApiProblemDetails), StatusCodes.Status415UnsupportedMediaType)]
	[ProducesResponseType(typeof(AccountApiProblemDetails), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(typeof(AccountApiProblemDetails), StatusCodes.Status501NotImplemented)]
	[ApiController]
	public abstract class BaseController<TController> : ControllerBase where TController : ControllerBase
	{
		/// <summary>
		/// Mapper field used for object mapping
		/// </summary>
		protected readonly IMapper Mapper;

		private readonly ILogger<TController> _logger;
		private readonly IMediator _mediator;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="mediator"></param>
		/// <param name="mapper"></param>
		protected BaseController(ILogger<TController> logger, IMediator mediator, IMapper mapper)
		{
			_logger = logger.ThrowArgumentNullExceptionOnNull();
			_mediator = mediator.ThrowArgumentNullExceptionOnNull();
			Mapper = mapper.ThrowArgumentNullExceptionOnNull();

			Trace.CorrelationManager.ActivityId = Guid.NewGuid();
		}

		/// <summary>
		/// Base Query processor. All Queries must pass through this method
		/// </summary>
		/// <typeparam name="TApiRequest">Api Request DTO, must inherit from <see cref="IApiRequest"/></typeparam>
		/// <typeparam name="TApiResult">Api FaceMatches DTO, must inherit from <see cref="IApiRequest"/></typeparam>
		/// <typeparam name="TApplicationRequest">Application Request DTO, must inherit from <see cref="IRequest{TApplicationResult}"/></typeparam>
		/// <typeparam name="TApplicationResult">Application FaceMatches DTO, must be of type class</typeparam>
		/// <param name="request"></param>
		/// <returns></returns>
		protected async Task<IActionResult> ProcessQueryAsync<TApiRequest, TApiResult, TApplicationRequest, TApplicationResult>(TApiRequest request)
			where TApiRequest : IApiRequest
			where TApiResult : IApiResult
			where TApplicationRequest : IRequest<TApplicationResult>
			where TApplicationResult : class
		{
			return await ProcessActionWithExceptionHandling(request, async (request) =>
			{
				_logger.LogDebug("Invoked API QUERY: {context}", request.GetType().Name);

				//if (!ModelState.IsValid) return ProblemDetailsHelper.CreateValidationProblemDetailsResult(ModelState, HttpContext);

				var applicationRequest = Mapper.Map<TApplicationRequest>(request);
				TApplicationResult queryResult = default!;

				try
				{
					queryResult = await _mediator.Send(applicationRequest);
				}
				//catch (Exception ex) when (ex is PartialSuccessException)
				//{
				//	return StatusCode((int)HttpStatusCode.PartialContent, new PartialServiceResult(((PartialSuccessException)ex).Payload, ex.Message));
				//}
				//catch (Exception ex) when (ex is NoContentException)
				//{
				//	return StatusCode((int)HttpStatusCode.NoContent);
				//}
				catch (Exception ex)
				{
					return StatusCode((int)HttpStatusCode.BadGateway);
				}

				if (queryResult == null) return NotFound();

				var apiResult = Mapper.Map<TApiResult>(queryResult);

				return Ok(apiResult);
			});
		}

		/// <summary>
		/// Base Command processor. All Commands must pass through this method
		/// </summary>
		/// <typeparam name="TApiRequest">Api Request DTO, must inherit from <see cref="IApiRequest"/></typeparam>
		/// <typeparam name="TApiResult">Api FaceMatches DTO, must inherit from <see cref="IApiRequest"/></typeparam>
		/// <typeparam name="TApplicationRequest">Application Request DTO, must inherit from <see cref="IRequest{TApplicationResult}"/></typeparam>
		/// <typeparam name="TApplicationResult">Application FaceMatches DTO, must be of type <see cref="BaseCommandResult"/></typeparam>
		/// <param name="request"></param>
		/// <returns></returns>
		protected async Task<IActionResult> ProcessCommandAsync<TApiRequest, TApiResult, TApplicationRequest, TApplicationResult>(TApiRequest request)
			where TApiRequest : IApiRequest
			where TApiResult : IApiResult
			where TApplicationRequest : IRequest<BaseCommandResult>
			where TApplicationResult : BaseCommandResult
		{
			return await ProcessActionWithExceptionHandling(request, async (request) =>
			{
				_logger.LogDebug("Invoked API COMMAND: {context}", request.GetType().Name);

				//if (!ModelState.IsValid) return ProblemDetailsHelper.CreateValidationProblemDetailsResult(ModelState, HttpContext);

				var applicationRequest = Mapper.Map<TApplicationRequest>(request);

				var commandResult = await _mediator.Send(applicationRequest);

				if (!commandResult.IsSuccessful) throw new AccountApiException($"{request.GetType().Name} COMMAND has failed", commandResult.Errors);

				if (commandResult.Payload is null) return Ok();

				// this was added to cater for overloading calls from the
				// below method ProcessCommandAsync. the one with two params.
				// since the method doesnt specify the API Result obj
				// when calling the overloading method, it will try to map
				// it's payload into an object but it will throw exception.
				// therefore if no payload expected, and type is HttpStatusCode,
				// then return Ok()
				if (commandResult.Payload.GetType().Namespace?.Contains("System.Net") ?? default!)
					return Ok();

				var apiResult = Mapper.Map<TApiResult>(commandResult.Payload);

				return Ok(apiResult);
			});
		}

		/// <summary>
		/// Base Command processor. All Commands must pass through this method
		/// </summary>
		/// <typeparam name="TApiRequest">Api Request DTO, must inherit from <see cref="IApiRequest"/></typeparam>
		/// <typeparam name="TApplicationRequest">Application Request DTO, must inherit from <see cref="IRequest{TApplicationResult}"/></typeparam>
		/// <param name="request"></param>
		protected async Task<IActionResult> ProcessCommandAsync<TApiRequest, TApplicationRequest>(TApiRequest request)
			where TApiRequest : IApiRequest
			where TApplicationRequest : IRequest<BaseCommandResult>
			=> await ProcessCommandAsync<TApiRequest, IApiResult, TApplicationRequest, BaseCommandResult>(request);

		/// <summary>
		/// Return Access Token
		/// </summary>
		protected string GetAccessToken()
		{
			AuthenticationHeaderValue.TryParse(HttpContext.Request.Headers["Authorization"], out var headerValue);
			return headerValue?.Parameter!;
		}

		private async Task<IActionResult> ProcessActionWithExceptionHandling<TRequest>(TRequest request, Func<TRequest, Task<IActionResult>> action)
		{
			try
			{
				return await action(request);
			}
			catch (ValidationException ex)
			{
				//LogError(ex, request);
				throw;
			}
			//catch (NotFoundException ex)
			//{
			//	LogError(ex, request);
			//	throw;
			//}
			//catch (ApplicationException ex)
			//{
			//	LogError(ex, request);
			//	throw;
			//}
			//catch (BackOfficeApiException ex)
			//{
			//	LogError(ex, request);
			//	throw;
			//}
			//catch (BackOfficeUnauthorizedAccessException)
			//{
			//	throw;
			//}
			//catch (ForbiddenException ex)
			//{
			//	LogError(ex, request);
			//	throw;
			//}
			//catch (Exception ex)
			//{
			//	LogError(ex, request);
			//	throw;
			//}
		}

		//private void LogError<TRequest>(Exception ex, TRequest request)
		//{
		//	var exceptions = ex.RetrieveInnerExceptions();
		//	var requestPayload = request?.ToJsonFull();

		//	if (ex is BackOfficeApiException apiException)
		//		_logger.LogError(ex, "{context}, Request: {requestPayload}, Errors: {errors}", typeof(TRequest).Name, requestPayload, string.Join(" | ", apiException.Errors?.ToJson()));
		//	else if (request is AuthenticatePayloadRequest authRequest)
		//	{
		//		_logger.LogError(ex, "{context} : {@data}", typeof(TRequest).Name, new { Request = new { Username = authRequest.Username, Password = "*****" }, InnerExceptions = exceptions });
		//	}
		//	else
		//		_logger.LogError(ex, "{context} : {@data}", typeof(TRequest).Name, new { Request = request, InnerExceptions = exceptions });
		//}
	}
}