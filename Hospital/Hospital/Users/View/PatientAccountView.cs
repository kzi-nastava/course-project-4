using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital;
using Autofac;
using Hospital.Users.Service;
using Hospital.Users.Model;

namespace Hospital.Users.View
{
	public class PatientAccountView
	{
		private IUserService _userService;
		private PatientAccountService _patientAccountService; //ovde

		public PatientAccountView()
		{
			this._userService = Globals.container.Resolve<IUserService>();
			this._patientAccountService = new PatientAccountService(); //ovde
		}

		public void ShowActivePatients()
		{
			List<User> activePatients = _patientAccountService.FilterActivePatients();
			if (activePatients.Count != 0)
			{
				ShowPatients(activePatients);
			}
			else
			{
				Console.WriteLine("Trenutno nema aktivnih pacijenata.");
			}
		}

		public void ShowBlockedPatients()
		{
			List<User> activePatients = _patientAccountService.FilterBlockedPatients();
			if (activePatients.Count != 0)
			{
				ShowPatients(activePatients);
			}
			else
			{
				Console.WriteLine("Trenutno nema blokiranih pacijenata.");
			}
		}

		public void ShowPatients(List<User> patients)
		{
			for (int i = 0; i < patients.Count; i++)
			{
				User patient = patients[i];
				Console.WriteLine("{0}. {1}", i + 1, patient.Name + " " + patient.Surname);
			}
		}

		public int EnterPatientIndex()
		{
			string patientIndexInput;
			int patientIndex;
			do
			{
				Console.WriteLine("Unesite redni broj pacijenta:");
				Console.Write(">>");
				patientIndexInput = Console.ReadLine();
				if (patientIndexInput == "x")
				{
					return 0;
				}

			} while (!int.TryParse(patientIndexInput, out patientIndex) || patientIndex < 1);
			return patientIndex;

		}

		public User SelectPatient(List<User> patients)
		{
			if (patients.Count == 0)
			{
				Console.WriteLine("Trenutno nema pacijenata za prikazivanje");
				return null;
			}
			ShowPatients(patients);
			Console.WriteLine("x. Odustani");
			Console.WriteLine("------------------------------------");
			int patientIndex;
			do
			{
				patientIndex = EnterPatientIndex();
			} while (patientIndex > patients.Count);
			if (patientIndex == 0)
				return null;

			return patients[patientIndex - 1];
		}

		public User EnterNewUserData()
		{
			Console.WriteLine("-------------------------------------");
			Console.WriteLine("Unesite podatke o pacijentu");
			string email;
			do
			{
				Console.Write("Email: ");
				email = Console.ReadLine();
			} while (_userService.IsEmailValid(email));
			Console.Write("Password: ");
			var password = Console.ReadLine();
			Console.Write("Ime: ");
			var name = Console.ReadLine();
			Console.Write("Prezime: ");
			var surname = Console.ReadLine();
			User newPatient = new User(User.Role.Patient, email, password, name, surname, User.State.Active);
			Console.WriteLine("\nNalog za pacijenta " + name + " " + surname + " je uspesno kreiran.");
			return newPatient;
		}

		public void ShowOldPatientData(User patient)
		{
			Console.WriteLine("\nStari podaci\n---------------------------------------------");
			Console.WriteLine("Ime: " + patient.Name);
			Console.WriteLine("Prezime: " + patient.Surname);
			Console.WriteLine("Email: " + patient.Email);
			Console.WriteLine("Lozinka: " + patient.Password);
			Console.WriteLine("---------------------------------------------\n");
		}

		public User ChangePatientData(User patient)
		{
			ShowOldPatientData(patient);
			Console.WriteLine("\nNovi podaci\n------------------------------------------");
			Console.Write("Ime: ");
			var newName = Console.ReadLine();
			Console.Write("Prezime: ");
			var newSurname = Console.ReadLine();
			Console.Write("Lozinka: ");
			var newPassword = Console.ReadLine();

			patient.Name = newName;
			patient.Surname = newSurname;
			patient.Password = newPassword;
			return patient;
		}

		public void BlockingPatient()
		{
			User patient = SelectPatient(_patientAccountService.FilterActivePatients());
			if (patient is null)
				return;
			_patientAccountService.BlockPatient(patient);
			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno blokiran.\n");
		}

		public void UnblockingPatient()
		{
			User patient = SelectPatient(_patientAccountService.FilterBlockedPatients());
			if (patient is null)
				return;
			_patientAccountService.UnblockPatient(patient);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno odblokiran.\n");

		}

		public void CreateNewPatientAccount()
		{
			User newPatient = EnterNewUserData();
			_patientAccountService.CreatePatientAccount(newPatient);
		}

		public void ChangePatientAccount()
		{
			User patient = SelectPatient(_patientAccountService.Patients);
			if (patient is null)
				return;
			patient = ChangePatientData(patient);
			_userService.UpdateUserInfo(patient);

			Console.WriteLine("\nNalog pacijenta je uspesno izmenjen.");

		}
	}
}