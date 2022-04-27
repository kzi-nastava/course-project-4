using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;

namespace Hospital.SecretaryImplementation
{
	class Secretary
	{
		private List<User> patients;
		public Secretary(List<User> users)
		{
			this.patients = FilterPatients(users);
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
					//ShowActivePatients(FilterActivePatients());
				}
				else if (choice == "3")
				{

				}
				else if (choice == "4")
				{

				}
				else if (choice == "5")
				{
					//ShowBlockedPatients(FilterBlockedPatients());
				}
				else if (choice == "6")
				{

				}
				else if (choice == "x")
				{
					break;
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

		//public void ShowActivePatients(List<User> patients)
		//{
		//	for (int i = 0; i < patients.Count; i++)
		//	{
		//		User patient = patients[i];
		//		Console.WriteLine("{0}. {1}", i + 1, patient.Name + " " + patient.Surname);
		//	}
		//}

		//public void ShowBlockedPatients(List<User> patients)
		//{
		//	for (int i = 0; i < patients.Count; i++)
		//	{
		//		User patient = patients[i];
		//		Console.WriteLine("{0}. {1}", i + 1, patient.Name + " " + patient.Surname);

		//	}
		//}

		private void LogOut()
		{
			Login loging = new Login();
			loging.LogIn();
		}

	}

}
