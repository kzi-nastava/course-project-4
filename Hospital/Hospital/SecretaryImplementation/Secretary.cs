using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class Secretary
	{
		private List<User> patients;
		private UserService userService;
		private HealthRecordService healthRecordService;
		private AppointmentService appointmentService;
		private RequestService requestService;

		public Secretary(UserService service)
		{
			this.userService = service;
			this.patients = FilterPatients(service.Users);
			this.healthRecordService = new HealthRecordService();
			this.appointmentService = new AppointmentService();
			this.requestService = new RequestService(appointmentService);
		}

		public void SecretaryMenu()
		{
			while (true)
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
				Console.WriteLine("x. Odjavi se");
				Console.WriteLine("--------------------------------------------");
				Console.Write(">>");
				var choice = Console.ReadLine();
				if (choice == "1")
				{
					CreatePatientAccount();
				}
				else if (choice == "2")
				{
					List<User> activePatients = FilterActivePatients();
					if(activePatients.Count != 0)
					{
						ShowPatients(activePatients);
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
					List<User> blockedPatients = FilterBlockedPatients();
					if (blockedPatients.Count != 0)
					{
						ShowPatients(blockedPatients);
					}
					else
					{
						Console.WriteLine("Trenutno nema blokiranih pacijenata.");
					}
				}
				else if(choice == "7")
				{
					AnswerRequest();
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

		public List<User> FilterPatients(List<User> allUsers)
		{
			List<User> patients = new List<User>();
			foreach (User user in allUsers)
			{
				if (user.UserRole == User.Role.Patient)
				{
					patients.Add(user);
				}
			}
			return patients;
		}

		public List<User> FilterActivePatients()
		{
			List<User> activePatients = new List<User>();

			foreach (User user in this.patients)
			{
				if (user.UserState == User.State.Active)
				{
					activePatients.Add(user);
				}
			}
			return activePatients;
		}

		public List<User> FilterBlockedPatients()
		{
			List<User> blockedPatients = new List<User>();
			foreach (User user in this.patients)
			{
				if (user.UserState == User.State.BlockedBySecretary || user.UserState == User.State.BlockedBySystem)
				{
					blockedPatients.Add(user);
				}
			}
			return blockedPatients;
		}

		public void ShowPatients(List<User> patients)
		{
			for (int i = 0; i < patients.Count; i++)
			{
				User patient = patients[i];
				Console.WriteLine("{0}. {1}", i + 1, patient.Name + " " + patient.Surname);
			}
		}

		public void BlockPatient()
		{
			List<User> activePatients = this.FilterActivePatients();
			if(activePatients.Count == 0)
			{
				Console.WriteLine("Trenutno nema aktivnih pacijenata. ");
				return;
			}
			ShowPatients(activePatients);
			Console.WriteLine("x. Odustani");

			Console.WriteLine("--------------------------------------");
			string patientIndexInput;
			int patientIndex;
			do
			{
				Console.WriteLine("Unesite redni broj pacijenta ciji nalog zelite da blokirate");
				Console.Write(">>");
				patientIndexInput = Console.ReadLine();
				if(patientIndexInput == "x")
				{
					return;
				}
			}
			while (!int.TryParse(patientIndexInput, out patientIndex) || patientIndex < 1 || patientIndex > activePatients.Count);

			User patient = activePatients[patientIndex-1];

			userService.BlockOrUnblockUser(patient, true);
			this.patients = FilterPatients(userService.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno blokiran.\n");
		}


		public void UnblockPatient()
		{
			List<User> blockedPatients = this.FilterBlockedPatients();
			if (blockedPatients.Count == 0)
			{
				Console.WriteLine("Trenutno nema blokiranih pacijenata. ");
				return;
			}
			ShowPatients(blockedPatients);
			Console.WriteLine("--------------------------------------");
			Console.WriteLine("x. Odustani");
			string patientIndexInput;
			int patientIndex;
			do
			{
				Console.WriteLine("Unesite redni broj pacijenta ciji nalog zelite da odblokirate");
				Console.Write(">>");
				patientIndexInput = Console.ReadLine();
				if(patientIndexInput == "x")
				{
					return;
				}
			} 
			while (!int.TryParse(patientIndexInput, out patientIndex) || patientIndex < 1 || patientIndex > blockedPatients.Count);

			User patient = blockedPatients[patientIndex - 1];
			userService.BlockOrUnblockUser(patient, false);
			this.patients = FilterPatients(userService.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno odblokiran.\n");

		}

		public void CreatePatientAccount()
		{
			//TODO: create health record

			Console.WriteLine("-------------------------------------");
			Console.WriteLine("Unesite podatke o pacijentu");
			Console.Write("Email: ");
			var email = Console.ReadLine();
			bool emailDuplicate = this.userService.IsEmailValid(email);
			while (emailDuplicate)
			{
				Console.WriteLine("Vec postoji nalog sa ovom email adresom.");
				Console.Write("Email: ");
				email = Console.ReadLine();
				emailDuplicate = this.userService.IsEmailValid(email);
			}
			Console.Write("Password: ");
			var password = Console.ReadLine();
			Console.Write("Ime: ");
			var name = Console.ReadLine();
			Console.Write("Prezime: ");
			var surname = Console.ReadLine();

			User newPatient = new User(User.Role.Patient, email, password, name, surname, User.State.Active);
			this.userService.Users.Add(newPatient);
			this.patients.Add(newPatient);

			this.userService.UpdateFile();

			this.healthRecordService.CreateHealthRecord(newPatient);
			Console.WriteLine("\nNalog za pacijenta " + name + " " + surname + " je uspesno kreiran.");
		}

		private void ChangePatientAccount()
		{
			if (patients.Count == 0)
			{
				Console.WriteLine("\nTrenutno nema postojecih naloga za izmenu.");
				return;
			}
			ShowPatients(patients);
			Console.WriteLine("x. Odustani");
			Console.WriteLine("--------------------------------------");
			string patientIndexInput;
			int patientIndex;
			do
			{
				Console.WriteLine("Unesite redni broj pacijenta ciji nalog zelite da izmenite.");
				Console.Write(">>");
				patientIndexInput = Console.ReadLine();
				if (patientIndexInput == "x")
				{
					return;
				}
			} while (!int.TryParse(patientIndexInput, out patientIndex) || patientIndex < 1 || patientIndex > this.patients.Count);

			User patient = patients[patientIndex - 1];
			Console.WriteLine("\nStari podaci\n---------------------------------------------");
			Console.WriteLine("Ime: " + patient.Name);
			Console.WriteLine("Prezime: " + patient.Surname);
			Console.WriteLine("Email: " + patient.Email);
			Console.WriteLine("Lozinka: " + patient.Password);
			Console.WriteLine("---------------------------------------------\n\nNovi podaci");
			Console.Write("Ime: ");
			var newName = Console.ReadLine();
			Console.Write("Prezime: ");
			var newSurname = Console.ReadLine();
			Console.Write("Lozinka: ");
			var newPassword = Console.ReadLine();

			patient.Name = newName;
			patient.Surname = newSurname;
			patient.Password = newPassword;

			userService.UpdateUserInfo(patient);

			Console.WriteLine("\nNalog pacijenta je uspesno izmenjen.");

		}

		private void AnswerRequest()
		{
			List<Appointment> requests = requestService.Requests;
			if(requests.Count == 0)
			{
				Console.WriteLine("Trenutno nema zahteva za obradu. ");
				return;
			}
			for(int i = 0; i < requests.Count; i++)
			{
				Appointment request = requests[i];
				switch (request.AppointmentStateProp)
				{
					case (Appointment.AppointmentState.UpdateRequest):
						Appointment oldValuesAppointment = requestService.findInitialAppointment(request.AppointmentId);
						Console.Write("{0}. {1}, {2}->{3}, {4}->{5}, {6}->{7}, ", i + 1, userService.GetUserFullName(oldValuesAppointment.PatientEmail),
							oldValuesAppointment.DateAppointment.ToString("MM/dd/yyyy"), request.DateAppointment.ToString("MM/dd/yyyy"),
							oldValuesAppointment.StartTime.ToString("HH:mm"), request.StartTime.ToString("HH:mm"),
							oldValuesAppointment.EndTime.ToString("HH:mm"), request.EndTime.ToString("HH:mm"));
						Console.Write("Izmena termina");
						break;
					case (Appointment.AppointmentState.DeleteRequest):
						Console.Write("{0}. {1}, {2}, {3}, {4}, ", i+1,userService.GetUserFullName(request.PatientEmail), request.DateAppointment.ToString("MM/dd/yyyy"),
							request.StartTime.ToString("HH:mm"), request.EndTime.ToString("HH:mm"));
						Console.Write("Brisanje termina");
						break;
				}
			}
			Console.WriteLine("\nx. Odustani");
			Console.WriteLine("--------------------------------------------");
			string requestIndexInput;
			int requestIndex;
			do
			{
				Console.WriteLine("Unesite broj zahteva koji zelite da obradite");
				Console.Write(">>");
				requestIndexInput = Console.ReadLine();
				if (requestIndexInput == "x")
				{
					return;
				}
			} while (!int.TryParse(requestIndexInput, out requestIndex) || requestIndex < 1 || requestIndex > requests.Count);

			Appointment activeRequest = requests[requestIndex - 1];
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

			requestService.ProcessRequest(activeRequest, actionIndex);
			Console.WriteLine("\nZahtev je uspesno obradjen");
		}

		private void LogOut()
		{
			Login loging = new Login();
			loging.LogIn();
		}

	}

}
