using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Model;
using Hospital.Appointments.Service;
using Hospital.Users.View;

namespace Hospital.Users.Service
{
    public class PatientAccountService : IPatientAccountService
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

		public List<User> Patients { get { return _patients; } }

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

		public void BlockPatient(User patient)
		{
			_userService.BlockOrUnblockUser(patient, true);
			this._patients = FilterPatients(_userService.Users);
		}

		public void UnblockPatient(User patient)
		{
			_userService.BlockOrUnblockUser(patient, false);
			this._patients = FilterPatients(_userService.Users);

		}

		public void CreatePatientAccount(User newPatient)
		{
			this._userService.Add(newPatient);
			this._patients.Add(newPatient);

			
			this._healthRecordService.CreateHealthRecord(newPatient);
		}
	}
}
