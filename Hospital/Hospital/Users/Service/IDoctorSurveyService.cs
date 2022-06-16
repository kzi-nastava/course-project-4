using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Model;
using Hospital.Appointments.Model;
using Hospital.Users.Repository;

namespace Hospital.Users.Service
{
    public interface IDoctorSurveyService
    {
        IDoctorSurveyRepository DoctorSurveyRepository { get; }
        List<DoctorSurvey> EvaluatedDoctors { get; }
        DoctorSurvey EvaluateDoctor(Appointment appointment);
        DoctorSurveyResult GetSurveyResultForDoctor(DoctorUser doctor);
        void AddEvaluatedDoctor(DoctorSurvey evaluatedDoctor);
        List<DoctorSurveyResult> GetResults();
        List<DoctorSurveyResult> GetBestDoctors();
        List<DoctorSurveyResult> GetWorstDoctors();
    }
}
