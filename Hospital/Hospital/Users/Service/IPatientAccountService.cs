using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
	public interface IPatientAccountService : IService<User>
	{
		List<User> Patients { get; }
		List<User> FilterPatients(List<User> allUsers);
		List<User> FilterActivePatients();
		List<User> FilterBlockedPatients();
		void BlockPatient(User patient);
		void UnblockPatient(User patient);
		void CreatePatientAccount(User newPatient);
	}
}
