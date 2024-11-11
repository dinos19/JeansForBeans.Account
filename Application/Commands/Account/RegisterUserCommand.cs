using Application.NotificationHandlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Account
{
	public class RegisterUserCommand : IRequest<BaseCommandResult>
	{
		/// <summary>
		/// Username
		/// </summary>
		public string Username { get; set; } = default!;

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; } = default!;
	}

	///<inheritdoc/>
	internal class RegisterUserHandler : CommandHandler<RegisterUserCommand, BaseCommandResult>
	{

		/// <summary>
		/// ctor of RegisterUserHandler
		/// </summary>
		/// <param name="notificationsHub"></param>
		public RegisterUserHandler(INotificationsHub notificationsHub) : base(notificationsHub)
		{
			
		}
		///<inheritdoc/>
		protected override async Task<NotifiableResponse> HandleInternal(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			//var result = await _agentsToolAuthenticationService.Authenticate(request.Username.Trim(), request.Password.Trim());

			//if (!result.IsSuccessful)
			//	throw new ApplicationException(result.ErrorsSummary, result.Errors);

			//return result.Payload switch
			//{
			//	SuccessAuthenticationResult _ => new NotifiableResponse(BaseCommandResult.SuccessWithPayload(result.Payload), new Authenticated(request.Username)),
			//	FailedsAuthenticationResult _ => new NotifiableResponse(BaseCommandResult.SuccessWithPayload(result.Payload)),
			//	_ => throw new ApplicationException($"Unexpected type {result.Payload.GetType().FullName}"),
			//};

			return null;
		}
	}
}
