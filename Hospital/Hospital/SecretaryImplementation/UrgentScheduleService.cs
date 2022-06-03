using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class UrgentScheduleService
	{
		private AppointmentService _appointmentService;
		private PatientAccountService _patientAccountService;
		private UserService _userService;
		private NotificationService _notificationService;

		public UrgentScheduleService()
		{
			this._appointmentService = new AppointmentService();
			this._patientAccountService = new PatientAccountService();
			this._userService = new UserService();
			this._notificationService = new NotificationService();
		}

		public void SelectValuesForUrgentSchedule()
		{
			User patient = _patientAccountService.SelectPatient(_patientAccountService.FilterActivePatients());
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

		public DoctorUser.Speciality SelectSpeciality()
		{
			Console.WriteLine("");
			var specialities = Enum.GetValues(typeof(DoctorUser.Speciality)).Cast<DoctorUser.Speciality>().ToList();
			int i = 1;
			foreach (DoctorUser.Speciality speciality in specialities)
			{
				Console.WriteLine("{0}. {1}", i, SpecialityToString(speciality));
				++i;
			}
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
						Room freeRoom = _appointmentService.FindFreeRoom(currentTime, currentTime);
						DateTime endTime;
						if (appointmentType == 1)
							endTime = currentTime.AddMinutes(15);
						else
							endTime = currentTime.AddMinutes(60);
						newAppointment = new Appointment(_appointmentService.GetNewAppointmentId().ToString(), patient.Email, doctor.Email,
							currentTime, currentTime, endTime, Appointment.State.Created,
							Int32.Parse(freeRoom.Id), (Appointment.Type)appointmentType, false, true);
						_appointmentService.AppendNewAppointmentInFile(newAppointment);
						Console.WriteLine("\nUspesno obavljeno hitno zakazivanje\nSlanje obavestenja izabranom lekaru...");
						Notification notificaton = new Notification(_notificationService.GetNewNotificationId(), doctor.Email,
							"Imate hitno zakazan termin u " + currentTime.ToString("HH:mm"), false);
						_notificationService.Notifications.Add(notificaton);
						_notificationService.UpdateFile();
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
				rescheduledAppointment = _appointmentService.RescheduleAppointment(leastUrgent);
			} while (_appointmentService.IsAppointmentFreeForDoctor(rescheduledAppointment));
			_appointmentService.UpdateAppointment(rescheduledAppointment);

			Appointment newAppointment = new Appointment(_appointmentService.GetNewAppointmentId().ToString(), patient.Email, doctor.Email,
				leastUrgent.DateAppointment, leastUrgent.StartTime, leastUrgent.StartTime.AddMinutes(45), Appointment.State.Created,
				leastUrgent.RoomNumber, (Appointment.Type)appointmentType, false, true);
			_appointmentService.AppendNewAppointmentInFile(newAppointment);
			_notificationService.Notifications.Add(new Notification(_notificationService.GetNewNotificationId(), doctor.Email,
						   "Imate hitno zakazan termin u " + leastUrgent.StartTime.ToString("HH:mm"), false));
			_notificationService.Notifications.Add(new Notification(_notificationService.GetNewNotificationId(), rescheduledAppointment.DoctorEmail,
						   "Premesten vam je termin za " + rescheduledAppointment.DateAppointment.ToString("dd/MM/yyyy ") + rescheduledAppointment.StartTime.ToString("HH:mm"), false));
			_notificationService.Notifications.Add(new Notification(_notificationService.GetNewNotificationId(), rescheduledAppointment.PatientEmail,
						   "Premesten vam je termin za " + rescheduledAppointment.DateAppointment.ToString("dd/MM/yyyy ") + rescheduledAppointment.StartTime.ToString("HH:mm"), false));
			_notificationService.UpdateFile();
		}

	}
}
