using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class UrgentSchedulingView
	{
		private UrgentScheduling _urgentScheduling;
		private PatientAccountService _patientAccountService;

		public UrgentSchedulingView()
		{
			this._urgentScheduling = new UrgentScheduling();
			this._patientAccountService = new PatientAccountService();
		}


		public void SelectValuesForUrgentSchedule()
		{
			User patient = PatientAccountView.SelectPatient(_patientAccountService.FilterActivePatients());
			DoctorUser.Speciality speciality = SelectSpeciality();
			Console.WriteLine("\nTip:\n1.Pregled\n2.Operacija");
			string indexInput;
			int index;
			do
			{
				Console.Write(">>");
				indexInput = Console.ReadLine();
			} while (!int.TryParse(indexInput, out index) || index < 1 || index > 2);
			_urgentScheduling.ScheduleUrgently(patient, speciality, index);
		}

		public string SpecialityToString(DoctorUser.Speciality speciality)
		{
			if (speciality == DoctorUser.Speciality.Cardiologist)
			{
				return "Kardiologija";
			}
			else if (speciality == DoctorUser.Speciality.Neurologist)
			{
				return "Neurologija";
			}
			else if (speciality == DoctorUser.Speciality.Pediatrician)
			{
				return "Pedijatrija";
			}
			else if (speciality == DoctorUser.Speciality.Psychologist)
			{
				return "Psihologija";
			}
			else if (speciality == DoctorUser.Speciality.General)
			{
				return "Opsta praksa";
			}
			return "Hirurgija";
		}

		public void ShowSpecialities()
		{
			var specialities = Enum.GetValues(typeof(DoctorUser.Speciality)).Cast<DoctorUser.Speciality>().ToList();
			Console.WriteLine("");
			int i = 1;
			foreach (DoctorUser.Speciality speciality in specialities)
			{
				Console.WriteLine("{0}. {1}", i, SpecialityToString(speciality));
				++i;
			}
		}

		public DoctorUser.Speciality SelectSpeciality()
		{
			var specialities = Enum.GetValues(typeof(DoctorUser.Speciality)).Cast<DoctorUser.Speciality>().ToList();
			ShowSpecialities();
			string indexInput;
			int index;
			do
			{
				Console.WriteLine("Unesite redni broj specijalizacije.");
				Console.Write(">>");
				indexInput = Console.ReadLine();

			} while (!int.TryParse(indexInput, out index) || index < 1 || index > specialities.Count());
			return specialities[index - 1];

		}

	}
}
