using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Appointments.Model;
using Hospital.Users.Model;

namespace Hospital.Appointments.Service
{
    public interface IReferralService
    {
        void Add(Referral referral);
        void UseReferral(Referral usedReferral);
        List<Referral> FilterUnused();
        int GetNewReferralId();

    }
}
