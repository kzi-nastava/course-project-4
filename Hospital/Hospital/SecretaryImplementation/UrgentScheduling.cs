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
	class UrgentScheduling
	{
		private AppointmentService _appointmentService;
		private UserService _userService;
		private NotificationService _notificationService;

		public UrgentScheduling()
		{
			this._appointmentService = new AppointmentService();
			this._userService = new UserService();
			this._notificationService = new NotificationService();
		}

		public Appointment CreateNewAppointment(User patient, User doctor, DateTime startingTime, int appointmentType)
		{
			Room freeRoom = _appointmentService.FindFreeRoom(startingTime, startingTime);
			DateTime endTime;
			if (appointmentType == 1)
				endTime = startingTime.AddMinutes(15);
			else
				endTime = startingTime.AddMinutes(60);
			return new Appointment(_appointmentService.GetNewAppointmentId().ToString(), patient.Email, doctor.Email,
				startingTime, startingTime, endTime, Appointment.State.Created,
				Int32.Parse(freeRoom.Id), (Appointment.Type)appointmentType, false, true);
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
						newAppointment = CreateNewAppointment(patient, doctor, currentTime, appointmentType);
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
