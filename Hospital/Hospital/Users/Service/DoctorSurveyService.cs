using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital;
using Autofac;
using Hospital.Users.Repository;
using Hospital.Users.Model;
using Hospital.Appointments.Model;

namespace Hospital.Users.Service
{
    public class DoctorSurveyService
    {
        private IDoctorSurveyRepository _doctorServiceRepository;
        private List<DoctorSurvey> _evaluatedDoctors;
        private IUserService _userService;

        public List<DoctorSurvey> EvaluatedDoctors { get { return _evaluatedDoctors; } }
        public IDoctorSurveyRepository DoctorSurveyRepository { get { return _doctorServiceRepository; } }

        public DoctorSurveyService()
        {
            _doctorServiceRepository = Globals.container.Resolve<IDoctorSurveyRepository>();
            _evaluatedDoctors = _doctorServiceRepository.Load();
            _userService = Globals.container.Resolve<IUserService>();
        }
        public DoctorSurvey EvaluateDoctor(Appointment appointment)
        {
            Console.WriteLine("\nDoktora ocenjujete ocenama od 1 do 5");
            Console.Write("\nKvalitet usluge doktora: ");
            int quality = Int32.Parse(Console.ReadLine());
            Console.Write("Da li biste doktora preporucili prijatelju: ");
            int recommendation = Int32.Parse(Console.ReadLine());
            Console.Write("Komentar: ");
            string comment = Console.ReadLine();

            DoctorSurvey evaluatedDoctor =
                new DoctorSurvey(appointment.AppointmentId, appointment.PatientEmail, appointment.DoctorEmail, quality, recommendation, comment);

            return evaluatedDoctor;
        }

        public DoctorSurveyResult GetSurveyResultForDoctor(DoctorUser doctor)
        {
            int surveyCount = 0;
            float qualitySum = 0;
            int[] qualityCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            float recommendationSum = 0;
            int[] recommendationCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            List<string> comments = new List<string>();
            foreach (DoctorSurvey survey in _evaluatedDoctors)
            {
                if (survey.DoctorEmail.Equals(doctor.Email))
                {
                    surveyCount++;
                    qualitySum += survey.Quality;
                    qualityCount[survey.Quality]++;
                    recommendationSum += survey.Recommendation;
                    recommendationCount[survey.Recommendation]++;
                    comments.Add("Pacijent " + survey.PatientEmail + ":\n" + survey.Comment);
                }
            }
            float averageQuality = surveyCount > 0 ? qualitySum / surveyCount : 0.0f;
            float averageRecommendation = surveyCount > 0 ? recommendationSum / surveyCount : 0.0f;
            return new DoctorSurveyResult(doctor, surveyCount, averageQuality, qualityCount, averageRecommendation,
                recommendationCount, comments);
        }

        public void AddEvaluatedDoctor(DoctorSurvey evaluatedDoctor)
        {
            this._evaluatedDoctors.Add(evaluatedDoctor);
            this._doctorServiceRepository.Save(this._evaluatedDoctors);
            this._evaluatedDoctors = this._doctorServiceRepository.Load();
        }

        public List<DoctorSurveyResult> GetResults()
        {
            List<DoctorSurveyResult> results = new List<DoctorSurveyResult>();
            foreach (DoctorUser doctor in _userService.GetDoctors())
            {
                results.Add(GetSurveyResultForDoctor(doctor));
            }
            return results;
        }

        public List<DoctorSurveyResult> GetBestDoctors()
        {
            List<DoctorSurveyResult> doctors = GetResults();
            doctors.Sort(new Comparison<DoctorSurveyResult>((d1, d2) => d2.AverageQuality.CompareTo(d1.AverageQuality)));
            List<DoctorSurveyResult> bestDoctors = new List<DoctorSurveyResult>();
            foreach (DoctorSurveyResult doctor in doctors)
            {
                if (bestDoctors.Count < 3 && doctor.SurveyCount != 0)
                    bestDoctors.Add(doctor);
            }
            return bestDoctors;
        }

        public List<DoctorSurveyResult> GetWorstDoctors()
        {
            List<DoctorSurveyResult> doctors = GetResults();
            doctors.Sort(new Comparison<DoctorSurveyResult>((d1, d2) => d1.AverageQuality.CompareTo(d2.AverageQuality)));
            List<DoctorSurveyResult> worstDoctors = new List<DoctorSurveyResult>();
            foreach (DoctorSurveyResult doctor in doctors)
            {
                if (worstDoctors.Count < 3 && doctor.SurveyCount != 0)
                    worstDoctors.Add(doctor);
            }
            return worstDoctors;
        }
    }
}
