using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;
using Hospital.Users.Repository;

namespace Hospital.Users.Service
{
	public interface INotificationService
	{
		List<Notification> Notifications { get; }
		string GetNewNotificationId();
		void ReadNotification(Notification notificationRead);
		void AddNotification(Notification notification);
		void SendUrgentNotification(string receiverEmail, DateTime time);
		void SendRescheduleNotification(string receiverEmail, DateTime date, DateTime time);
		void SendVacationNotification(string receiverEmail, DateTime startDate, DateTime endDate, string reason);
	}
}
