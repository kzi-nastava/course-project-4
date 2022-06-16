using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Appointments.Service;
using Hospital.Appointments.Model;
using Hospital.Users.View;
using Hospital.Users.Model;
using Hospital.Rooms.Model;
using Hospital.Users.Service;

namespace Hospital.Appointments.View
{
    public class ReferralSchedulingView
	{
		private ReferralService _referralService;
		private AppointmentService _appointmentService;
		private UserService _userService;

		public ReferralSchedulingView()
		{
			this._appointmentService = new AppointmentService();
			this._referralService = new ReferralService();
			this._userService = new UserService();
		}

		public void ShowReferrals(List<Referral> referrals)
		{
			int i = 1;
			foreach (Referral referral in referrals)
			{

				if (referral.Doctor != "null")
				{
					Console.Write("{0}. Pacijent: {1} | Doktor: {2} | ", i,
						_userService.GetUserFullName(referral.Patient), _userService.GetUserFullName(referral.Doctor));

					i++;
				}
				else
				{
					Console.Write("{0}. Pacijent: {1} | Specijalnost: {2} | ", i,
						_userService.GetUserFullName(referral.Patient), SpecialityToString(referral.DoctorSpeciality));
					i++;
				}
				Console.WriteLine("Tip: " + AppointmentType(referral));

			}
		}

		public Referral SelectReferral()
		{
			List<Referral> unusedReferrals = _referralService.FilterUnused();
			if (unusedReferrals.Count == 0)
			{
				Console.WriteLine("Trenutno nema neiskoriscenih uputa u sistemu.");
				return null;
			}
			this.ShowReferrals(unusedReferrals);
			Console.WriteLine("x. Odustani");
			Console.WriteLine("-------------------------------------------------------------");
			string referralIndexInput;
			int referralIndex;
			do
			{
				Console.WriteLine("Unesite redni broj uputa koji zelite da obradite.");
				Console.Write(">>");
				referralIndexInput = Console.ReadLine();
				if (referralIndexInput == "x")
				{
					return null;
				}
			} while (!int.TryParse(referralIndexInput, out referralIndex) || referralIndex < 1 || referralIndex > unusedReferrals.Count);
			return unusedReferrals[referralIndex - 1];
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
			return newAppointment;
		}

		public void ScheduleAppointmentWithReferral()
		{
			Referral referral = SelectReferral();
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
			_appointmentService.Add(newAppointment);
			_referralService.UseReferral(referral);

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

		public Appointment CreateAppointment(Referral referral)
		{
			string date = EnterDate();
			string startingTime = EnterStartingTime();

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

		public string AppointmentType(Referral referral)
		{
			if (referral.TypeProp == Appointment.Type.Examination)
			{
				return "Pregled";
			}
			return "Operacija";
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
	}
}
