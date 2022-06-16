using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Users.Model;

namespace Hospital.Users.Repository
{
	public class NotificationRepository : INotificationRepository
	{
		public List<Notification> Load()
		{
			List<Notification> allNotifications = new List<Notification>();

			using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\notifications.csv"))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					string[] fields = parser.ReadFields();
					string id = fields[0];
					string userEmail = fields[1];
					string content = fields[2];
					bool read = Convert.ToBoolean(fields[3]);
					Notification notification = new Notification(id, userEmail, content, read);
					allNotifications.Add(notification);
				}
			}
			return allNotifications;
		}

		public void Save(List<Notification> notifications)
		{
			string filePath = @"..\..\Data\notifications.csv";
			List<string> lines = new List<String>();

			string line;
			foreach (Notification notification in notifications)
			{
				line = notification.Id + "," + notification.UserEmail + "," + notification.Content + "," + notification.Read;
				lines.Add(line);
			}
			File.WriteAllLines(filePath, lines.ToArray());
		}
	}
}
