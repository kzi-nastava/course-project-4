using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Users.Model
{
    public class HospitalSurvey
    {
        private string _patientEmail;
        private int _quality;
        private int _cleanliness;
        private int _satisfied;
        private int _recommendation;
        private string _comment;

        public string PatientEmail { get { return _patientEmail; } }
        public int Quality { get { return _quality; } }
        public int Cleanliness { get { return _cleanliness; } }
        public int Satisfied { get { return _satisfied; } }
        public int Recommendation { get { return _recommendation; } }
        public string Comment { get { return _comment; } }

        public HospitalSurvey(string patientEmail, int quality, int cleanliness, int satisfied, int recommendation, string comment)
        {
            this._patientEmail = patientEmail;
            this._quality = quality;
            this._cleanliness = cleanliness;
            this._satisfied = satisfied;
            this._recommendation = recommendation;
            this._comment = comment;
        }
    }
}
