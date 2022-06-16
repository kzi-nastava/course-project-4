using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;
using Hospital.Appointments.Model;

namespace Hospital.Users.Service
{
    public interface IDoctorSurveyService: IService<DoctorSurvey>
    {
        List<DoctorSurvey> EvaluatedDoctors { get; }
        DoctorSurvey EvaluateDoctor(Appointment appointment);
        DoctorSurveyResult GetSurveyResultForDoctor(DoctorUser doctor);
        void AddEvaluatedDoctor(DoctorSurvey evaluatedDoctor);
        List<DoctorSurveyResult> GetResults();
        List<DoctorSurveyResult> GetBestDoctors();
        List<DoctorSurveyResult> GetWorstDoctors();
    }
}
