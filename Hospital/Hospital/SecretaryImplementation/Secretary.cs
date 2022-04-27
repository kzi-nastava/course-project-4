using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class Secretary
	{
		private List<User> patients;
		private UserService service;
		public Secretary(UserService service)
		{
			this.service = service;
			this.patients = FilterPatients(service.Users);
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
				Console.WriteLine("5. Pregled blokiranih naloga pacijenata");
				Console.WriteLine("6. Pregled pristiglih zahteva za izmenu/brisanje pregleda");
				Console.WriteLine("x. Odjavi se");
				Console.WriteLine("--------------------------------------------");
				Console.Write(">>");
				var choice = Console.ReadLine();
				if (choice == "1")
				{

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

				}
				else if (choice == "4")
				{
					BlockPatient();
				}
				else if (choice == "5")
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
				else if (choice == "6")
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

			Console.WriteLine("--------------------------------------");
			string patientIndexInput;
			int patientIndex;
			do
			{
				Console.WriteLine("Unesite redni broj pacijenta ciji nalog zelite da blokirate");
				Console.Write(">>");
				patientIndexInput = Console.ReadLine();
			}
			while (!int.TryParse(patientIndexInput, out patientIndex) || patientIndex < 1 || patientIndex > activePatients.Count);

			User patient = activePatients[patientIndex-1];

			service.BlockUser(patient);
			this.patients = FilterPatients(service.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno blokiran.\n");
		}

		private void LogOut()
		{
			Login loging = new Login();
			loging.LogIn();
		}

	}

}
