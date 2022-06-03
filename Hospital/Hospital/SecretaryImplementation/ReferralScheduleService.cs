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
	class ReferralScheduleService
	{
		private ReferralService _referralService;
		private AppointmentService _appointmentService;

		public ReferralScheduleService()
		{
			this._appointmentService = new AppointmentService();
			this._referralService = new ReferralService();
		}

		public void ScheduleAppointmentWithReferral()
		{
			Referral referral = _referralService.SelectReferral();
			if (referral is null)
			{
				return;
			}
			Appointment newAppointment;
			if (referral.Doctor != "null")
			{
				do
				{
					newAppointment = CreateAppointment(referral);
				} while (!_appointmentService.IsAppointmentFreeForDoctor(newAppointment));

			}
			else
			{
				newAppointment = MakeAppointmentWithFreeDoctor(referral);
			}
			Console.WriteLine("Uspesno zakazan pregled!");
			_appointmentService.Appointments.Add(newAppointment);
			_appointmentService.UpdateFile();
			_referralService.UseReferral(referral);

		}

		public Appointment MakeAppointmentWithFreeDoctor(Referral referral)
		{
			User freeDoctor;
			Appointment newAppointment;
			do
			{
				newAppointment = CreateAppointment(referral);
				freeDoctor = _appointmentService.FindFreeDoctor(referral.DoctorSpeciality, newAppointment);
			} while (freeDoctor is null);
			newAppointment.DoctorEmail = freeDoctor.Email;
			//Console.WriteLine("Izabrani doktor: " + _userService.GetUserFullName(freeDoctor.Email));
			return newAppointment;
		}

		public Appointment CreateAppointment(Referral referral)
		{
			string date, startingTime;
			do
			{
				Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
				date = Console.ReadLine();
			} while (!_appointmentService.IsDateFormValid(date));
			do
			{
				Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
				startingTime = Console.ReadLine();
			} while (!_appointmentService.IsTimeFormValid(startingTime));

			DateTime dateOfAppointment = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
			DateTime startTime = DateTime.ParseExact(startingTime, "HH:mm", CultureInfo.InvariantCulture);
			DateTime endTime;
			if (referral.TypeProp == Appointment.Type.Examination)
			{
				endTime = startTime.AddMinutes(15);
			}
			else
			{
				endTime = startTime.AddMinutes(60);
			}
			string id = _appointmentService.GetNewAppointmentId().ToString();
			Room freeRoom = _appointmentService.FindFreeRoom(dateOfAppointment, startTime);
			if (freeRoom is null)
			{
				return null;
			}
			int roomId = Int32.Parse(freeRoom.Id);

			return new Appointment(id, referral.Patient, referral.Doctor, dateOfAppointment,
				startTime, endTime, Appointment.State.Created, roomId, referral.TypeProp, false, false);

		}
	}
}
