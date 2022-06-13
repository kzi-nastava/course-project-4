using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Service;
using Hospital.Users.Model;

namespace Hospital.Users.View
{
    public class PatientDoctorSurvey
    {
        private DoctorSurveyService _doctorSurveyService;
        private UserService _userService;

        public PatientDoctorSurvey(UserService userService)
        {
            this._doctorSurveyService = new DoctorSurveyService();
            this._userService = userService;
        }

        public IDictionary<string, double> CalculateAverageDoctorGrade()
        {
            IDictionary<string, double> averageGrades = new Dictionary<string, double>();
            double grades = 0.0, numberGrades = 0.0;
            foreach (DoctorUser doctor in _userService.UsersRepository.DoctorUsers)
            {
                foreach (DoctorSurvey evaluatedDoctor in _doctorSurveyService.EvaluatedDoctors)
                {
                    if (doctor.Email.Equals(evaluatedDoctor.DoctorEmail))
                    {
                        grades += evaluatedDoctor.Quality;
                        grades += evaluatedDoctor.Recommendation;
                        numberGrades += 2;
                    }
                }
                double average = Double.IsNaN(grades / numberGrades) ? 0 : grades / numberGrades;
                averageGrades.Add(new KeyValuePair<string, double>(doctor.Email, average));
                grades = 0.0;
                numberGrades = 0.0;
            }
            return averageGrades;
        }
    }
}
