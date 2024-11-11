using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Application.NotificationHandlers
{
	internal interface INotificationsHub
	{
		void Enqueue(INotification notification);
	}

	internal sealed class NotificationsHub : INotificationsHub
	{
		private readonly Subject<INotification> _notificationsSubject;

		public NotificationsHub(IEnumerable<INotificationsHandler> handlers)
		{
			_notificationsSubject = new Subject<INotification>();
			var notifications = _notificationsSubject.AsObservable();

			foreach (var handler in handlers)
				handler.Subscribe(notifications);
		}

		public void Enqueue(INotification notification)
		{
			_notificationsSubject.OnNext(notification);
		}
	}
}
