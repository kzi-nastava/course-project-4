using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class UrgentSchedulingView
	{
		private PatientAccountService _patientAccountService;
		private PatientAccountView _patientAccountView;
		private AppointmentService _appointmentService;
		private NotificationService _notificationService;
		private UserService _userService;

		public UrgentSchedulingView()
		{
			this._patientAccountService = new PatientAccountService();
			this._patientAccountView = new PatientAccountView();
			this._appointmentService = new AppointmentService();
			this._notificationService = new NotificationService();
			this._userService = new UserService();
		}


		public void SelectValuesForUrgentSchedule()
		{
			User patient = _patientAccountView.SelectPatient(_patientAccountService.FilterActivePatients());
			DoctorUser.Speciality speciality = SelectSpeciality();
			Console.WriteLine("\nTip:\n1.Pregled\n2.Operacija");
			string indexInput;
			int index;
			do
			{
				Console.Write(">>");
				indexInput = Console.ReadLine();
			} while (!int.TryParse(indexInput, out index) || index < 1 || index > 2);
			ScheduleUrgently(patient, speciality, index);
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

		public void ScheduleUrgently(User patient, DoctorUser.Speciality speciality, int appointmentType)
		{
			List<User> capableDoctors = _userService.FilterDoctors(speciality);
			DateTime currentTime = DateTime.Now.AddMinutes(15);
			DateTime gapTime = DateTime.Now.AddHours(2);
			Appointment newAppointment;

			while (currentTime <= gapTime)
			{
				foreach (User doctor in capableDoctors)
				{
					if (_appointmentService.IsDoctorFree(doctor, currentTime))
					{
						newAppointment = _appointmentService.CreateNewAppointment(patient, doctor, currentTime, appointmentType);
						_appointmentService.AppendNewAppointmentInFile(newAppointment);
						Console.WriteLine("\nUspesno obavljeno hitno zakazivanje\nSlanje obavestenja izabranom lekaru...");
						_notificationService.SendUrgentNotification(doctor.Email, currentTime);
						return;
					}
				}
				currentTime = currentTime.AddMinutes(15);
			}

			ScheduleWithNoFreeTerm(patient, capableDoctors[0], appointmentType);
		}

		public void ScheduleWithNoFreeTerm(User patient, User doctor, int appointmentType)
		{
			Appointment leastUrgent = _appointmentService.FindLeastUrgentAppointment();
			Appointment rescheduledAppointment;
			do
			{
				rescheduledAppointment = RescheduleAppointment(leastUrgent);
			} while (_appointmentService.IsAppointmentFreeForDoctor(rescheduledAppointment));
			_appointmentService.UpdateAppointment(rescheduledAppointment);

			Appointment newAppointment = new Appointment(_appointmentService.GetNewAppointmentId().ToString(), patient.Email, doctor.Email,
				leastUrgent.DateAppointment, leastUrgent.StartTime, leastUrgent.StartTime.AddMinutes(45), Appointment.State.Created,
				leastUrgent.RoomNumber, (Appointment.Type)appointmentType, false, true);
			_appointmentService.AppendNewAppointmentInFile(newAppointment);
			_notificationService.SendUrgentNotification(doctor.Email, leastUrgent.StartTime);
			_notificationService.SendRescheduleNotification(rescheduledAppointment.DoctorEmail, rescheduledAppointment.DateAppointment, rescheduledAppointment.StartTime);
			_notificationService.SendRescheduleNotification(rescheduledAppointment.PatientEmail, rescheduledAppointment.DateAppointment, rescheduledAppointment.StartTime);
		}
		public string EnterDate()
		{
			string date;
			do
			{
				Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
				date = Console.ReadLine();
			} while (!Utils.IsDateFormValid(date));
			return date;
		}

		public string EnterStartingTime()
		{
			string startingTime;
			do
			{
				Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
				startingTime = Console.ReadLine();
			} while (!Utils.IsTimeFormValid(startingTime));
			return startingTime;
		}

		public Appointment RescheduleAppointment(Appointment appointment)
		{
			string date = EnterDate();
			string startingTime = EnterStartingTime();

			DateTime dateOfAppointment = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
			DateTime startTime = DateTime.ParseExact(startingTime, "HH:mm", CultureInfo.InvariantCulture);
			DateTime endTime;
			if (appointment.TypeOfTerm == Appointment.Type.Examination)
				endTime = startTime.AddMinutes(15);
			else
				endTime = startTime.AddMinutes(60);
			string id = _appointmentService.GetNewAppointmentId().ToString();
			Room freeRoom = _appointmentService.FindFreeRoom(dateOfAppointment, startTime);
			if (freeRoom is null)
			{
				return null;
			}
			int roomId = Int32.Parse(freeRoom.Id);

			return new Appointment(id, appointment.PatientEmail, appointment.DoctorEmail, dateOfAppointment,
				startTime, endTime, Appointment.State.Created, roomId, appointment.TypeOfTerm, false, false);
		}

	}
}
