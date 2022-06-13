using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Appointments.Model
{
    public class Prescription
    {
        public enum TimeOfConsuming
        {
            DuringMeals = 1,
            AfterMeals = 2,
            BeforeMeals = 3,
            Irrelevant = 4
        }
        
        private string _idAppointment;
        private string _idDrug;
        private DateTime _startConsuming;
        private int _dose;
        private TimeOfConsuming _timeOfConsuming;

        public string IdAppointment { get { return _idAppointment; } set { _idAppointment = value; } }
        public string IdDrug { get { return _idDrug; } set { _idDrug = value; } }
        public DateTime StartConsuming { get { return _startConsuming; } set { _startConsuming = value; } }
        public int Dose { get { return _dose; }set { _dose = value; } }

        public TimeOfConsuming TimeConsuming { get { return _timeOfConsuming; } set { _timeOfConsuming = value; } }

        public Prescription(string idAppointment, string idDrug, DateTime startConsuming, int dose, TimeOfConsuming timeOfConsuming)
        {
            this._idAppointment = idAppointment;
            this._idDrug = idDrug;
            this._startConsuming = startConsuming;
            this._dose = dose;
            this._timeOfConsuming = timeOfConsuming;
        }

        public override string ToString()
        {
            return this._idAppointment + "," + this._idDrug + "," + this._startConsuming.ToString("HH:mm") + "," + this._dose.ToString() + "," + (int)this._timeOfConsuming;
        }

    }
}
