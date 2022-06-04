using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;

namespace Hospital.SecretaryImplementation
{
	class PatientAccountView
	{

		public static void ShowPatients(List<User> patients)
		{
			for (int i = 0; i < patients.Count; i++)
			{
				User patient = patients[i];
				Console.WriteLine("{0}. {1}", i + 1, patient.Name + " " + patient.Surname);
			}
		}

		public static int EnterPatientIndex()
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

		public static User SelectPatient(List<User> patients)
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

		public static User EnterNewUserData()
		{
			Console.WriteLine("-------------------------------------");
			Console.WriteLine("Unesite podatke o pacijentu");
			Console.Write("Email: ");
			var email = Console.ReadLine();
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

		public static void ShowPatientData(User patient)
		{
			Console.WriteLine("\nStari podaci\n---------------------------------------------");
			Console.WriteLine("Ime: " + patient.Name);
			Console.WriteLine("Prezime: " + patient.Surname);
			Console.WriteLine("Email: " + patient.Email);
			Console.WriteLine("Lozinka: " + patient.Password);
			Console.WriteLine("---------------------------------------------\n");
		}

		public static User ChangePatientData(User patient)
		{
			ShowPatientData(patient);
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
	}
}
