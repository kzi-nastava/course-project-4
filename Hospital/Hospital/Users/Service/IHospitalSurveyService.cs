using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
    public interface IHospitalSurveyService
    {
        void EvaluateHospitalSurvey(string patientEmail);
        HospitalSurvey InputValuesForServey(string patientEmail);
        HospitalSurveyResult GetResult();
        List<HospitalSurvey> SurveyResults { get; }
    }
}
