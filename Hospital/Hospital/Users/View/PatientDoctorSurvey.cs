using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Service;
using Hospital.Users.Model;
using Hospital.Appointments.View;
using Hospital.Appointments.Model;
using Hospital.Users.Repository;

namespace Hospital.Users.View
{
    public class PatientDoctorSurvey
    {
        private IDoctorSurveyService _doctorSurveyService;
        private UserService _userService;
        private PatientAppointmentsService _appointmentsService;

        public PatientDoctorSurvey(UserService userService, PatientAppointmentsService appointmentsService)
        {
            this._doctorSurveyService = new DoctorSurveyService(new DoctorSurveyRepository());
            this._userService = userService;
            this._appointmentsService = appointmentsService;
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

        public void GetAppointmentsForEvaluation()
        {
            List<Appointment> performedAppointment = this._appointmentsService.GetPerformedAppointmentForPatient(false);
            List<Appointment> appointmentsForEvaluation = new List<Appointment>();
            bool rated = false;

            foreach (Appointment appointment in performedAppointment)
            {
                foreach (DoctorSurvey doctorSurvey in this._doctorSurveyService.EvaluatedDoctors)
                {
                    if (doctorSurvey.IdAppointment.Equals(appointment.AppointmentId))
                        rated = true;
                }
                if (!rated) appointmentsForEvaluation.Add(appointment);
            }
            this.PickAppointmentForEvaluation(appointmentsForEvaluation);
        }

        private void PickAppointmentForEvaluation(List<Appointment> appointmentsForEvaluation)
        {
            if (appointmentsForEvaluation.Count == 0)
            { Console.WriteLine("Nemate nijedan pregled za ocenjivanje."); return; }
            
            this._appointmentsService.PrintRecommendedAppointments(appointmentsForEvaluation);

            int numAppointment;
            string choice;
            do
            {
                Console.WriteLine("\nUnesite broj pregleda koji zelite da ocenite: ");
                Console.Write(">> ");
                choice = Console.ReadLine();
            } while (!int.TryParse(choice, out numAppointment) || numAppointment < 1 || numAppointment > appointmentsForEvaluation.Count);

            DoctorSurvey evaluatedDoctor = this._doctorSurveyService.DoctorSurveyRepository.EvaluateDoctor(appointmentsForEvaluation[numAppointment-1]);

            this._doctorSurveyService.AddEvaluatedDoctor(evaluatedDoctor);
        }
    }
}
