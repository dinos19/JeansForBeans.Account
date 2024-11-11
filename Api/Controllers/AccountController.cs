using Application;
using Application.Commands.Account;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.ApiExceptions;
using Shared.Requests;
using Shared.Results.Account;

namespace Api.Controllers
{
	/// <summary>
	/// User registration and information controller
	/// </summary>
	[Authorize]
	[ApiVersion("1.0")]
	[Route("account/api/v{version:apiVersion}/[controller]")]
	public class AccountController : BaseController<AccountController>
	{
		public AccountController(ILogger<AccountController> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
		{
		}

		[Route("[action]")]
		[ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest payloadRequest)
	=> await ProcessCommandAsync<RegisterUserRequest, RegisterUserResponse, RegisterUserCommand, BaseCommandResult>(payloadRequest);
	}
}