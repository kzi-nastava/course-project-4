using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Service;
using Hospital.Appointments.Repository;
using Hospital.Appointments.Model;
using Hospital.Users.Model;
using Hospital.Users.View;

namespace Hospital.Appointments.Service
{
    public class ReferralService
	{
		private UserService _userService;
		private ReferralRepository _referralRepository;
		private List<Referral> _referrals;

		public List<Referral> Referrals { get { return this._referrals; } }

		public ReferralService()
		{
			this._userService = new UserService();
			this._referralRepository = new ReferralRepository();
			_referrals = this._referralRepository.Load();
		}

		public int GetNewReferralId()
		{
			return this._referrals.Count + 1;
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
			if(speciality == DoctorUser.Speciality.Cardiologist)
			{
				return "Kardiologija";
			}else if(speciality == DoctorUser.Speciality.Neurologist)
			{
				return "Neurologija";
			}else if(speciality == DoctorUser.Speciality.Pediatrician)
			{
				return "Pedijatrija";
			}else if(speciality == DoctorUser.Speciality.Psychologist)
			{
				return "Psihologija";
			}else if(speciality == DoctorUser.Speciality.General){
				return "Opsta praksa";
			}
			return "Hirurgija";
		}

		public List<Referral> FilterUnused()
		{
			List<Referral> unusedReferrals = new List<Referral>();

			foreach(Referral referral in _referrals)
			{
				if (!referral.Used)
				{
					unusedReferrals.Add(referral);
				}
			}
			return unusedReferrals;
		}

		

		public void UseReferral(Referral usedReferral)
		{
			foreach(Referral referral in _referrals)
			{
				if (referral.Id.Equals(usedReferral.Id))
				{
					referral.Used = true;
					_referralRepository.Save(this._referrals);
					break;
				}
			}
		}

		public void AddReferral(Referral referral)
        {
			this._referrals.Add(referral);
			this._referralRepository.Save(this._referrals);
        }

		public Referral SelectReferral()
		{
			List<Referral> unusedReferrals = FilterUnused();
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
	}
}
