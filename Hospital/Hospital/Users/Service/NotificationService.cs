using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital;
using Autofac;
using Hospital.Users.Repository;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
	public class NotificationService : INotificationService
	{
		private INotificationRepository _notificationRepository;
		private List<Notification> _notifications;

		public List<Notification> Notifications { get { return _notifications; } }
		public INotificationRepository Repository { get { return _notificationRepository; } }

		public NotificationService()
		{
			_notificationRepository = Globals.container.Resolve<INotificationRepository>(); //ovde
			_notifications = _notificationRepository.Load();
		}

		public void ReadNotification(Notification notificationRead)
		{

			foreach (Notification notification in _notifications)
			{
				if (notification.Id.Equals(notificationRead.Id))
				{
					notification.Read = true;
					break;
				}
			}
			_notificationRepository.Save(_notifications);
		}

		public string GetNewNotificationId()
		{
			return (this._notifications.Count + 1).ToString();
		}

		public void AddNotification(Notification notification)
		{
			Notifications.Add(notification);
			_notificationRepository.Save(Notifications);
		}

		public void SendUrgentNotification(string receiverEmail, DateTime time)
		{
			Notification notificaton = new Notification(GetNewNotificationId(), receiverEmail,
							"Imate hitno zakazan termin u " + time.ToString("HH:mm"), false);
			AddNotification(notificaton);
		}

		public void SendRescheduleNotification(string receiverEmail, DateTime date, DateTime time)
		{
			Notification notification = new Notification(GetNewNotificationId(), receiverEmail, "Vas termin je prebacen za "
				+ date.ToString("MM/dd/yyyy") + " u " + time.ToString("HH:mm") + " sati", false);
			AddNotification(notification);
		}

		public void SendVacationNotification(string receiverEmail, DateTime startDate, DateTime endDate, string reason)
		{
			Notification notification;
			if (reason == "")
				notification = new Notification(GetNewNotificationId(), receiverEmail, "Zahtev za slobodne dane (" + startDate.ToString("MM/dd/yyyy") +
					" - " + endDate.ToString("MM/dd/yyyy") + ") je prihvacen", false);
			else
				notification = new Notification(GetNewNotificationId(), receiverEmail, "Zahtev za slobodne dane je odbijen. Razlog : " + reason, false);
			AddNotification(notification);
		}
	}
}