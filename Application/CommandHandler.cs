using Application.NotificationHandlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
	/// <summary>
	/// Base command handler with internal domain event notification mechanism, implements <see cref="IRequestHandler{TRequest, TResponse}"/>
	/// </summary>
	/// <typeparam name="TRequest">MediatR request</typeparam>
	/// <typeparam name="TResponse">Command response</typeparam>
	internal abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly INotificationsHub _notificationsHub;

		protected CommandHandler(INotificationsHub notificationsHub)
		{
			_notificationsHub = notificationsHub;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
		{
			var notifiableResponse = await HandleInternal(request, cancellationToken);

			foreach (var notification in notifiableResponse.Notifications)
				_notificationsHub.Enqueue(notification);

			return notifiableResponse.Response;
		}

		protected abstract Task<NotifiableResponse> HandleInternal(TRequest request, CancellationToken cancellationToken);

		protected class NotifiableResponse
		{
			public TResponse Response { get; }
			public IReadOnlyCollection<INotification> Notifications { get; }

			public NotifiableResponse(TResponse response, params INotification[] notifications)
			{
				Response = response;
				Notifications = notifications ?? Array.Empty<INotification>();
			}
		}
	}
}
