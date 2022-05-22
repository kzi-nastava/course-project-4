using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Service;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.PatientImplementation
{
    class PatientDoctorSurvey
    {
        Patient _currentPatient;
        AppointmentService _appointmentService = new AppointmentService();
        List<Appointment> _allAppointments;
        DoctorSurveyService _doctorSurveyService = new DoctorSurveyService();
        UserRepository _userRepository = new UserRepository();
        List<User> _allUsers;

        public PatientDoctorSurvey(Patient patient)
        {
            this._currentPatient = patient;
            _allAppointments = _appointmentService.AppointmentRepository.Load();
            this._allUsers = _userRepository.Load();
        }

        public IDictionary<string, double> CalculateAverageDoctorGrade()
        {
            IDictionary<string, double> averageGrades = new Dictionary<string, double>();
            double grades = 0.0, numberGrades = 0.0;
            //double numberGrades = 0.0;
            foreach (DoctorUser doctor in _userRepository.DoctorUsers)
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
