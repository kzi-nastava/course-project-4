using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class DoctorSurvey
    {
        private string _idAppointment;
        private string _patientEmail;
        private string _doctorEmail;
        private int _quality;
        private int _recommendation;
        private string _comment;

        public string IdAppointment { get { return _idAppointment; } }
        public string PatientEmail { get { return _patientEmail; } }
        public string DoctorEmail { get { return _doctorEmail; } }
        public int Quality { get { return _quality; } }
        public int Recommendation { get { return _recommendation; } }
        public string Comment { get { return _comment; } }

        public DoctorSurvey(string idAppointment, string patientEmail, string doctorEmail, int quality, int recommendation, string comment)
        {
            this._idAppointment = idAppointment;
            this._patientEmail = patientEmail;
            this._doctorEmail = doctorEmail;
            this._quality = quality;
            this._recommendation = recommendation;
            this._comment = comment;
        }
    }
}
