using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Appointments.Model
{
    public class HealthRecord
    {
        
        private string _idHealthRecord;
        private string _emailPatient;
        private int _patientHeight;
        private double _patientWeight;
        private string _previousIllnesses;
        private string _allergen;
        private string _bloodType;
        

        public string IdHealthRecord { get { return _idHealthRecord; } set { _idHealthRecord = value; } }

        public string EmailPatient { get { return _emailPatient; } set { _emailPatient = value; } }
        public int PatientHeight { get { return _patientHeight; } set { _patientHeight = value; } }
        public double PatientWeight { get { return _patientWeight; } set { _patientWeight = value; } }
        public string PreviousIllnesses { get { return _previousIllnesses; } set { _previousIllnesses = value; } }
        public string Allergen { get { return _allergen; } set { _allergen = value; } }

        public string BloodType { get { return _bloodType; } set { _bloodType = value; } }
        

        public HealthRecord(string idHealthRecord, string emailPatient, int patientHeight, double patientWeight, string previousIllnesses,string allergen,
            string bloodType)
        {
            this._idHealthRecord = idHealthRecord;
            this._emailPatient = emailPatient;
            this._patientHeight = patientHeight;
            this._patientWeight = patientWeight;
            this._previousIllnesses = previousIllnesses;
            this._allergen = allergen;
            this._bloodType = bloodType;
         
        }

        public override string ToString()
        {
            return "Id zdravstvenog: " + this._idHealthRecord + "\n" +
                "Email pacijenta:" + this._emailPatient + "\n" +
                "Visina: " + this._patientHeight + "\n" +
                "Težina: " + this._patientWeight + "\n" +
                "Alergena: " + this._allergen + "\n" +
                "Krvna grupa: " + this._bloodType + "\n";
              
        }

        public string ToStringForFile()
		{
            return this._idHealthRecord + "*" + this._emailPatient + "*" + this._patientHeight + "*" +
                this._patientWeight + "*" + this._previousIllnesses + "*" + this._allergen + "*" + this._bloodType;
		}

    }
}
