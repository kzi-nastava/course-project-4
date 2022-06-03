using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class PatientAccountService
	{
		private List<User> _patients;
		private UserService _userService;
		private HealthRecordService _healthRecordService;

		public PatientAccountService()
		{
			this._userService = new UserService();
			this._patients = FilterPatients(_userService.Users);
			this._healthRecordService = new HealthRecordService();
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

			foreach (User user in this._patients)
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
			foreach (User user in this._patients)
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
			string patientIndexInput;
			int patientIndex;
			do
			{
				Console.WriteLine("Unesite redni broj pacijenta:");
				Console.Write(">>");
				patientIndexInput = Console.ReadLine();
				if (patientIndexInput == "x")
				{
					return null;
				}

			} while (!int.TryParse(patientIndexInput, out patientIndex) || patientIndex < 1 || patientIndex > patients.Count);

			return patients[patientIndex - 1];
		}

		public void BlockPatient()
		{
			User patient = SelectPatient(FilterActivePatients());
			if (patient is null)
				return;

			_userService.BlockOrUnblockUser(patient, true);
			this._patients = FilterPatients(_userService.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno blokiran.\n");
		}

		public void UnblockPatient()
		{
			User patient = SelectPatient(FilterBlockedPatients());
			if (patient is null)
				return;
			_userService.BlockOrUnblockUser(patient, false);
			this._patients = FilterPatients(_userService.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno odblokiran.\n");

		}

		public User EnterNewUserData() {
			Console.WriteLine("-------------------------------------");
			Console.WriteLine("Unesite podatke o pacijentu");
			Console.Write("Email: ");
			var email = Console.ReadLine();
			bool emailDuplicate = this._userService.IsEmailValid(email);
			while (emailDuplicate)
			{
				Console.WriteLine("Vec postoji nalog sa ovom email adresom.");
				Console.Write("Email: ");
				email = Console.ReadLine();
				emailDuplicate = this._userService.IsEmailValid(email);
			}
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

		public void CreatePatientAccount()
		{
			User newPatient = EnterNewUserData();
			this._userService.AddUser(newPatient);
			this._patients.Add(newPatient);

			
			this._healthRecordService.CreateHealthRecord(newPatient);
		}

		public void ChangePatientData(User patient)
		{
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

			_userService.UpdateUserInfo(patient);

		}

		public void ChangePatientAccount()
		{
			User patient = SelectPatient(_patients);
			if (patient is null)
				return;

			this.ChangePatientData(patient);

			Console.WriteLine("\nNalog pacijenta je uspesno izmenjen.");

		}
	}
}
