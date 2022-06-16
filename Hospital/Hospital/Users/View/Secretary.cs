using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Appointments.View;
using Hospital.Rooms.View;
using Hospital.Users.Service;
using Hospital.Appointments.Model;

namespace Hospital.Users.View
{
	public class Secretary : IMenuView
	{

		private PatientAccountView _patientAccountView;
		private PatientRequestView _patientRequestView;
		private UrgentSchedulingView _urgentSchedulingView;
		private ReferralSchedulingView _referralScheduling;
		private DynamicEquipmentMovingView _dynamicEquipmentMovingView;
		private DynamicEquipmentRequestView _dynamicEquipmentRequestView;
		private AnswerRequestsForDaysOffView _answerRequestsForDaysOffView;

		public Secretary()
		{
			this._patientAccountView = new PatientAccountView();
			this._patientRequestView = new PatientRequestView();
			this._referralScheduling = new ReferralSchedulingView();
			this._urgentSchedulingView = new UrgentSchedulingView();
			this._dynamicEquipmentMovingView = new DynamicEquipmentMovingView();
			this._dynamicEquipmentRequestView = new DynamicEquipmentRequestView();
			this._answerRequestsForDaysOffView = new AnswerRequestsForDaysOffView();
		}

		public void PrintSecretaryMenu()
		{
			Console.WriteLine("\nMENI");
			Console.WriteLine("-------------------------------------------");
			Console.WriteLine("1. Kreiranje novog naloga za pacijenta");
			Console.WriteLine("2. Pregled aktivnih naloga pacijenata");
			Console.WriteLine("3. Izmena naloga pacijenta");
			Console.WriteLine("4. Blokiranje naloga pacijenta");
			Console.WriteLine("5. Odblokiranje naloga pacijenata");
			Console.WriteLine("6. Pregled blokiranih naloga pacijenata");
			Console.WriteLine("7. Pregled pristiglih zahteva za izmenu/brisanje pregleda");
			Console.WriteLine("8. Zakazivanje pregleda/operacija na osnovu uputa");
			Console.WriteLine("9. Hitno zakazivanje");
			Console.WriteLine("10. Kreiranje zahteva za nabavku dinamicke opreme");
			Console.WriteLine("11. Rasporedjivanje dinamicke opreme");
			Console.WriteLine("12. Pregled zahteva za slobodne dane");
			Console.WriteLine("x. Odjavi se");
			Console.WriteLine("--------------------------------------------");
			Console.Write(">>");
		}

		public void SecretaryMenu()
		{
			while (true)
			{
				this.PrintSecretaryMenu();
				var choice = Console.ReadLine();
				if (choice == "1")
				{
					this._patientAccountView.CreateNewPatientAccount();
				}
				else if (choice == "2")
				{
					_patientAccountView.ShowActivePatients();
				}
				else if (choice == "3")
				{
					this._patientAccountView.ChangePatientAccount();
				}
				else if (choice == "4")
				{
					this._patientAccountView.BlockingPatient();
				}
				else if (choice == "5")
				{
					this._patientAccountView.UnblockingPatient();
				}
				else if (choice == "6")
				{
					_patientAccountView.ShowBlockedPatients();
				}
				else if (choice == "7")
				{
					AnswerRequest();
				}
				else if (choice == "8")
				{
					this._referralScheduling.ScheduleAppointmentWithReferral();
				}
				else if (choice == "9")
				{
					this._urgentSchedulingView.SelectValuesForUrgentSchedule();
				}
				else if (choice == "10")
				{
					this._dynamicEquipmentRequestView.SendRequestForProcurment();
				}
				else if (choice == "11")
				{
					this._dynamicEquipmentMovingView.MoveEquipment();
				}
				else if (choice == "12")
				{
					this._answerRequestsForDaysOffView.AnswerRequest();
				}
				else if (choice == "x")
				{
					this.LogOut();
				}
				else
				{
					Console.WriteLine("\nNepostojeca opcija!\n");
				}
			}

		}

		public int GetAction()
		{
			string actionIndexInput;
			int actionIndex;
			do
			{
				Console.WriteLine("\nIzaberite akciju koju zelite da izvrsite: ");
				Console.WriteLine("1. Prihvati zahtev");
				Console.WriteLine("2. Odbij zahtev");
				Console.Write(">>");
				actionIndexInput = Console.ReadLine();
			}
			while (!int.TryParse(actionIndexInput, out actionIndex) || actionIndex < 1 || actionIndex > 2);
			return actionIndex;
		}

		public void AnswerRequest()
		{
			Appointment activeRequest = _patientRequestView.SelectRequest();
			if (activeRequest is null)
			{
				return;
			}

			int actionIndex = GetAction();

			_patientRequestView._patientRequestService.ProcessRequest(activeRequest, actionIndex);
			Console.WriteLine("\nZahtev je uspesno obradjen");
		}
	}
}