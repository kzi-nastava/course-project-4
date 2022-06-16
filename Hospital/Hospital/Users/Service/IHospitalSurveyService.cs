using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
    public interface IHospitalSurveyService: IService<HospitalSurvey>
    {
        void EvaluateHospitalSurvey();
        HospitalSurvey InputValuesForServey(string patientEmail);
        void AddNewSurvey(HospitalSurvey newRatedSurvey);
        HospitalSurveyResult GetResult();

    }
}
