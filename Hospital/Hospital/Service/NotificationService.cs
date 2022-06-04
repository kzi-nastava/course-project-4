using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
	public class NotificationService
	{
		private NotificationRepository _notificationRepository;
		private List<Notification> _notifications;

		public List<Notification> Notifications { get { return _notifications; } }
		public NotificationRepository Repository { get { return _notificationRepository; } }

		public NotificationService()
		{
			_notificationRepository = new NotificationRepository();
			_notifications = _notificationRepository.Load();
		}

		public void ReadNotification(Notification notificationRead)
		{
			
			foreach(Notification notification in _notifications)
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
		}
	}
}
