namespace Hospital.Model
{
    class HealthRecord
    {
       
        private string _idHealthRecord;
        private string _emailPatient;
        private int _patientHeight;
        private double _patientWeight;
        private string _previousIllnesses;
        private string _allergen;
        private string _bloodType;
      


        public string IdHealthRecord { get { return _idHealthRecord; } }

        public string EmailPatient { get { return _emailPatient; } }
        public int PatientHeight { get { return _patientHeight; } }
        public double PatientWeight { get { return _patientWeight; } }
        public string PreviousIllnesses { get { return _previousIllnesses; } }
        public string Allergen { get { return _allergen; } }

        public string BloodType { get { return _bloodType; } }
       
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
                "Krvna grupa: " + this._bloodType;
        }

    }
}
