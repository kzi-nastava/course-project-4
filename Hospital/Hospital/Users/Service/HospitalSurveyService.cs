using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Repository;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
    class HospitalSurveyService
    {
        private HospitalSurveyRepository _hospitalServiceRepository;
        private List<HospitalSurvey> _surveyResults;
        private UserService _userService;
        private string _patientEmail;

        public List<HospitalSurvey> SurveyResults { get { return _surveyResults; } }

        public HospitalSurveyService(string patientEmail)
        {
            this._patientEmail = patientEmail;
            this._hospitalServiceRepository = new HospitalSurveyRepository();
            this._surveyResults = _hospitalServiceRepository.Load();
            this._userService = new UserService();
        }

        public void EvaluateHospitalSurvey()
        {
            foreach (HospitalSurvey hospitalSurvey in this._surveyResults)
            {
                if (hospitalSurvey.PatientEmail.Equals(this._patientEmail)) 
                { Console.WriteLine("Vec ste ocenili rad bolnice."); return; }    
            }
            HospitalSurvey surveyResult = this._hospitalServiceRepository.InputValuesForServey(this._patientEmail);
            this.AddNewSurvey(surveyResult);
        }

        private void AddNewSurvey(HospitalSurvey newRatedSurvey)
        {
            this._surveyResults.Add(newRatedSurvey);
            this._hospitalServiceRepository.Save(this._surveyResults);
            this._surveyResults = this._hospitalServiceRepository.Load();
        }
    }
}
