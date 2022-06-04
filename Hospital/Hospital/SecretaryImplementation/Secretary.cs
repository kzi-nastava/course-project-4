using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class Secretary : IMenuView
	{
		private PatientAccountService _patientAccountService;
		private RequestView _requestView;
		private RefferalScheduling _referralScheduleService;
		private UrgentSchedulingView _urgentSchedulingView;
		private DynamicEquipmentMovingService _dynamicEquipmentMovingService;
		private DynamicEquipmentRequestService _dynamicEquipmentRequestService;

		public Secretary(UserService service)
		{
			this._patientAccountService = new PatientAccountService();
			this._requestView = new RequestView();
			this._referralScheduleService = new RefferalScheduling();
			this._urgentSchedulingView = new UrgentSchedulingView();
			this._dynamicEquipmentMovingService = new DynamicEquipmentMovingService();
			this._dynamicEquipmentRequestService = new DynamicEquipmentRequestService();
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
					CreatePatientAccount();
				}
				else if (choice == "2")
				{
					List<User> activePatients = _patientAccountService.FilterActivePatients();
					if (activePatients.Count != 0)
					{
						PatientAccountView.ShowPatients(activePatients);
					}
					else
					{
						Console.WriteLine("Trenutno nema aktivnih pacijenata.");
					}
				}
				else if (choice == "3")
				{
					ChangePatientAccount();
				}
				else if (choice == "4")
				{
					BlockPatient();
				}
				else if (choice == "5")
				{
					UnblockPatient();
				}
				else if (choice == "6")
				{
					List<User> blockedPatients = _patientAccountService.FilterBlockedPatients();
					if (blockedPatients.Count != 0)
					{
						PatientAccountView.ShowPatients(blockedPatients);
					}
					else
					{
						Console.WriteLine("Trenutno nema blokiranih pacijenata.");
					}
				}
				else if (choice == "7")
				{
					AnswerRequest();
				}
				else if (choice == "8")
				{
					ScheduleAppointmentWithReferral();
				}
				else if(choice == "9")
				{
					UrgentSchedule();
				}
				else if(choice == "10")
				{
					MakeEquipmentRequest();
				}
				else if(choice == "11")
				{

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

		public void CreatePatientAccount()
		{
			this._patientAccountService.CreatePatientAccount();
		}

		public void ChangePatientAccount()
		{
			this._patientAccountService.ChangePatientAccount();
		}

		public void BlockPatient()
		{
			this._patientAccountService.BlockPatient();
		}

		public void UnblockPatient()
		{
			this._patientAccountService.UnblockPatient();
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
			Appointment activeRequest = _requestView.SelectRequest();
			if (activeRequest is null)
			{
				return;
			}

			int actionIndex = GetAction();

			_requestView._requestService.ProcessRequest(activeRequest, actionIndex);
			Console.WriteLine("\nZahtev je uspesno obradjen");
		}

		public void ScheduleAppointmentWithReferral()
		{
			this._referralScheduleService.ScheduleAppointmentWithReferral();
		}

		public void UrgentSchedule()
		{
			this._urgentSchedulingView.SelectValuesForUrgentSchedule();
		}

		public void MakeEquipmentRequest()
		{
			this._dynamicEquipmentRequestService.SendRequestForProcurment();
		}
	}

}
