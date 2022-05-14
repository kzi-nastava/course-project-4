using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
	public class NotificationRepository
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
	}
}
