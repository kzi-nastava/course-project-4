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
		private ReferralRepository referralRepository;
		private List<Referral> referrals;

		public List<Referral> Referrals { get { return this.referrals; } }

		public ReferralService()
		{
			this.referralRepository = new ReferralRepository();
			referrals = this.referralRepository.Load();
		}

		public List<Referral> FilterUnused()
		{
			List<Referral> unusedReferrals = new List<Referral>();

			foreach(Referral referral in referrals)
			{
				if (!referral.Used)
				{
					unusedReferrals.Add(referral);
				}
			}
			return unusedReferrals;
		}

		public void UpdateFile()
		{
			string filePath = @"..\..\Data\referrals.csv";
			List<string> lines = new List<String>();

			string line;
			foreach (Referral referral in referrals)
			{
				line = referral.Id + "," + referral.Patient + "," + referral.Doctor + "," + ((int)referral.TypeProp) + "," + referral.Used;
				lines.Add(line);
			}
			File.WriteAllLines(filePath, lines.ToArray());
		}

		public void UseReferral(Referral usedReferral)
		{
			foreach(Referral referral in referrals)
			{
				if (referral.Id.Equals(usedReferral.Id))
				{
					referral.Used = true;
					UpdateFile();
					break;
				}
			}
		}
	}
}
