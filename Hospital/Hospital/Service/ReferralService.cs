using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
	class ReferralService
	{
		private ReferralRepository referralRepository;
		private List<Referral> referrals;

		public List<Referral> Referrals { get { return this.referrals; } }

		public ReferralService()
		{
			this.referralRepository = new ReferralRepository();
			referrals = this.referralRepository.Load();
		}
	}
}
