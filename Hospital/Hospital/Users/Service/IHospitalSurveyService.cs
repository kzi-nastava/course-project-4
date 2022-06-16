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
        List<HospitalSurvey> SurveyResults { get; }

        void EvaluateHospitalSurvey();

        HospitalSurveyResult GetResult();
    }
}
