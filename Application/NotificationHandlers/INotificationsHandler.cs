using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NotificationHandlers
{
	internal interface INotificationsHandler
	{
		void Subscribe(IObservable<INotification> notifications);
	}
}
