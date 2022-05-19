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

		public NotificationService()
		{
			_notificationRepository = new NotificationRepository();
			_notifications = _notificationRepository.Load();
		}

		public void UpdateFile()
		{
			string filePath = @"..\..\Data\notifications.csv";
			List<string> lines = new List<string>();

			string line;
			foreach(Notification notification in _notifications)
			{
				line = notification.Id + "," + notification.UserEmail + "," + notification.Content + "," + notification.Read;
				lines.Add(line);
			}
			File.WriteAllLines(filePath, lines.ToArray());
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
			this.UpdateFile();
		}

		public string GetNewNotificationId()
		{
			return (this._notifications.Count + 1).ToString();
		}
	}
}
