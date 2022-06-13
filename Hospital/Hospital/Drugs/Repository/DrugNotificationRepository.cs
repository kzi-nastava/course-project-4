using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using System.IO;

using Hospital.Drugs.Model;

namespace Hospital.Drugs.Repository
{
    public class DrugNotificationRepository
	{
		public List<DrugNotification> Load()
		{
			List<DrugNotification> drugNotifications = new List<DrugNotification>();

			using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\drugNotification.csv"))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					string[] fields = parser.ReadFields();
					string patientEmail = fields[0];
					DateTime timeNotification = DateTime.ParseExact(fields[1], "HH:mm", CultureInfo.InvariantCulture);
					DrugNotification drugNotification = new DrugNotification(patientEmail, timeNotification);
					drugNotifications.Add(drugNotification);
				}
			}
			return drugNotifications;
		}

		public void ChangeTimeNotification(string userEmail)
		{
			Console.Write("\nUnesite koliko vremena ranije zelite da dobije obavestenje: ");
			string newTime = Console.ReadLine();

			List<string> lines = new List<string>();
			string line;
			foreach (DrugNotification notification in this.Load())
			{
				if (notification.PatientEmail.Equals(userEmail))
					line = notification.PatientEmail + "," + newTime;
				else
					line = notification.PatientEmail + "," + notification.TimeNotification.ToString("HH:mm");
				lines.Add(line);
			}
			File.WriteAllLines(@"..\..\Data\drugNotification.csv", lines.ToArray());
		}
    }
}
