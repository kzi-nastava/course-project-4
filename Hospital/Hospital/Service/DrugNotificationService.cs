using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    class DrugNotificationService
    {
		private DrugNotificationRepository _drugNotificationRepository;
		private List<DrugNotification> _drugNotifications;

		public List<DrugNotification> Notifications { get { return _drugNotifications; } set { _drugNotifications = value; } }

		public DrugNotificationService()
		{
			_drugNotificationRepository = new DrugNotificationRepository();
			_drugNotifications = _drugNotificationRepository.Load();
		}
	}
}
