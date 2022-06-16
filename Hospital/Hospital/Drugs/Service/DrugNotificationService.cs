using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

using Hospital.Drugs.Repository;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Service
{
    public class DrugNotificationService: IDrugNotificationService
    {
		private DrugNotificationRepository _drugNotificationRepository;
		private List<DrugNotification> _drugNotifications;

		public DrugNotificationRepository DrugNotificationRepository { get { return _drugNotificationRepository; } }
		public List<DrugNotification> Notifications { get { return _drugNotifications; } set { _drugNotifications = value; } }

		public DrugNotificationService()
		{
			_drugNotificationRepository = new DrugNotificationRepository();
			_drugNotifications = _drugNotificationRepository.Load();
		}

		public void ChangeNotificationTime(string patientEmail)
		{
			Console.Write("\nUnesite koliko vremena ranije zelite da dobije obavestenje: ");
			string inputTime = Console.ReadLine();
			DateTime newTime = DateTime.ParseExact(inputTime, "HH:mm", CultureInfo.InvariantCulture);

			foreach (DrugNotification notification in this._drugNotifications)
			{
				if (notification.PatientEmail.Equals(patientEmail))
				{
					notification.TimeNotification = newTime;
				}
			}

			this._drugNotificationRepository.Save(this._drugNotifications);
		}
	}
}
