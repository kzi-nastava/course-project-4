using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
	class ReferralService
	{
		private ReferralRepository _referralRepository;
		private List<Referral> _referrals;

		public List<Referral> Referrals { get { return this._referrals; } }

		public ReferralService()
		{
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
	}
}
