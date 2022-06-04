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
		private PatientAccountView _patientAccountView;

		public PatientAccountService()
		{
			this._userService = new UserService();
			this._patients = FilterPatients(_userService.Users);
			this._healthRecordService = new HealthRecordService();
			this._patientAccountView = new PatientAccountView();
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

		public void BlockPatient()
		{
			User patient = _patientAccountView.SelectPatient(FilterActivePatients());
			if (patient is null)
				return;

			_userService.BlockOrUnblockUser(patient, true);
			this._patients = FilterPatients(_userService.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno blokiran.\n");
		}

		public void UnblockPatient()
		{
			User patient = _patientAccountView.SelectPatient(FilterBlockedPatients());
			if (patient is null)
				return;
			_userService.BlockOrUnblockUser(patient, false);
			this._patients = FilterPatients(_userService.Users);

			Console.WriteLine("\nPacijent " + patient.Name + " " + patient.Surname + " je uspesno odblokiran.\n");

		}

		public void CreatePatientAccount()
		{
			User newPatient = _patientAccountView.EnterNewUserData();
			this._userService.AddUser(newPatient);
			this._patients.Add(newPatient);

			
			this._healthRecordService.CreateHealthRecord(newPatient);
		}

		public void ChangePatientAccount()
		{
			User patient = _patientAccountView.SelectPatient(_patients);
			if (patient is null)
				return;
			patient = _patientAccountView.ChangePatientData(patient);
			_userService.UpdateUserInfo(patient);

			Console.WriteLine("\nNalog pacijenta je uspesno izmenjen.");

		}
	}
}
