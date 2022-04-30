using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    class HealthRecord
    {
        
        private string idHealthRecord;
        private string emailPatient;
        private int patientHeight;
        private double patientWeight;
        private string previousIllnesses;
        private string allergen;
        private string bloodType;
        

        public string IdHealthRecord { get { return idHealthRecord; } }

        public string EmailPatient { get { return emailPatient; } }
        public int PatientHeight { get { return patientHeight; } }
        public double PatientWeight { get { return patientWeight; } }
        public string PreviousIllnesses { get { return previousIllnesses; } }
        public string Allergen { get { return allergen; } }

        public string BloodType { get { return bloodType; } }
        

        public HealthRecord(string idHealthRecord, string emailPatient, int patientHeight, double patientWeight, string previousIllnesses,string allergen,
            string bloodType)
        {
            this.idHealthRecord = idHealthRecord;
            this.emailPatient = emailPatient;
            this.patientHeight = patientHeight;
            this.patientWeight = patientWeight;
            this.previousIllnesses = previousIllnesses;
            this.allergen = allergen;
            this.bloodType = bloodType;
         
        }

        public override string ToString()
        {
            return "Id zdravstvenog: " + this.idHealthRecord + "\n" +
                "Email pacijenta:" + this.emailPatient + "\n" +
                "Visina: " + this.patientHeight + "\n" +
                "Težina: " + this.patientWeight + "\n" +
                "Alergena: " + this.allergen + "\n" +
                "Krvna grupa: " + this.bloodType + "\n";
              
        }

        public string ToStringForFile()
		{
            return this.idHealthRecord + "*" + this.emailPatient + "*" + this.patientHeight + "*" +
                this.patientWeight + "*" + this.previousIllnesses + "*" + this.allergen + "*" + this.bloodType;
		}

    }
}
