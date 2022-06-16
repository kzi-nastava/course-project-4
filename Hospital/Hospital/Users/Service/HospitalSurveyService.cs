using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Repository;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
    public class HospitalSurveyService: IHospitalSurveyService
    {
        private IHospitalSurveyRepository _hospitalServiceRepository;
        private List<HospitalSurvey> _surveyResults;
        private UserService _userService;
        private string _patientEmail;

        public HospitalSurveyService(string patientEmail)
        {
            this._patientEmail = patientEmail;
            this._hospitalServiceRepository = hospitalSurveyRepository;
            this._surveyResults = _hospitalServiceRepository.Load();
            this._userService = new UserService();
        }

        public List<HospitalSurvey> SurveyResults { get { return _surveyResults; } }

        public void EvaluateHospitalSurvey()
        {
            foreach (HospitalSurvey hospitalSurvey in this._surveyResults)
            {
                if (hospitalSurvey.PatientEmail.Equals(this._patientEmail)) 
                { Console.WriteLine("Vec ste ocenili rad bolnice."); return; }    
            }
            HospitalSurvey surveyResult = this.InputValuesForServey(this._patientEmail);
            this.AddNewSurvey(surveyResult);
        }

        public HospitalSurvey InputValuesForServey(string patientEmail)
        {
            Console.WriteLine("\nBolnicu ocenjujete ocenama od 1 do 5");
            Console.Write("\nKvalitet usluga bolnice: ");
            int quality = Int32.Parse(Console.ReadLine());
            Console.Write("Koliko je cista bolnica? : ");
            int cleanliness = Int32.Parse(Console.ReadLine());
            Console.Write("Da li ste zadovoljni? : ");
            int satisfied = Int32.Parse(Console.ReadLine());
            Console.Write("Da li biste predlozili bolnicu prijateljima? : ");
            int recommendation = Int32.Parse(Console.ReadLine());
            Console.Write("Komentar: ");
            string comment = Console.ReadLine();

            HospitalSurvey hospitalSurvey =
                new HospitalSurvey(patientEmail, quality, cleanliness, satisfied, recommendation, comment);

            return hospitalSurvey;
        }

        public void AddNewSurvey(HospitalSurvey newRatedSurvey)
        {
            this._surveyResults.Add(newRatedSurvey);
            this._hospitalServiceRepository.Save(this._surveyResults);
            this._surveyResults = this._hospitalServiceRepository.Load();
        }

        public HospitalSurveyResult GetResult()
        {
            int surveyCount = 0;

            float qualitySum = 0;
            int[] qualityCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            
            float cleanlinessSum = 0;
            int[] cleanlinessCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            
            float satisfiedSum = 0;
            int[] satisfiedCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            
            float recommendationSum = 0;
            int[] recommendationCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            
            List<string> comments = new List<string>();

            foreach (HospitalSurvey survey in _surveyResults)
            {
                surveyCount++;

                qualitySum += survey.Quality;
                qualityCount[survey.Quality]++;

                cleanlinessSum += survey.Cleanliness;
                cleanlinessCount[survey.Cleanliness]++;

                satisfiedSum += survey.Satisfied;
                satisfiedCount[survey.Satisfied]++;

                recommendationSum += survey.Recommendation;
                recommendationCount[survey.Recommendation]++;

                comments.Add("Pacijent " + survey.PatientEmail + ":\n" + survey.Comment);
            }

            float averageQuality = 0;
            float averageCleanliness = 0;
            float averageSatisfied = 0;
            float averageRecommendation = 0;

            if (surveyCount != 0)
            {
                averageQuality = qualitySum / surveyCount;
                averageCleanliness = cleanlinessSum / surveyCount;
                averageSatisfied = satisfiedSum / surveyCount;
                averageRecommendation = recommendationSum / surveyCount;
            }

            return new HospitalSurveyResult(surveyCount, averageQuality, qualityCount, averageCleanliness, cleanlinessCount, averageSatisfied,
                satisfiedCount, averageRecommendation, recommendationCount, comments);
        }
    }
}
