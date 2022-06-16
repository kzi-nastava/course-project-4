using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Model;
using Hospital.Appointments.Model;

namespace Hospital.Users.Repository
{
    public interface IDoctorSurveyRepository : IRepository<DoctorSurvey>
    {
        DoctorSurvey EvaluateDoctor(Appointment appointment);
    }
}
