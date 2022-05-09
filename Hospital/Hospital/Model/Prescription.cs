using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    class Prescription
    {
        
        private string _idAppointment;
        private string _idDrug;
        private DateTime _startConsuming;
        private int _dose;


        public string IdAppointment { get { return _idAppointment; } set { _idAppointment = value; } }
        public string IdDrug { get { return _idDrug; } set { _idDrug = value; } }
        public DateTime StartConsuming { get { return _startConsuming; } set { _startConsuming = value; } }
        public int Dose { get { return _dose; }set { _dose = value; } }

        public Prescription(string idAppointment, string idDrug, DateTime startConsuming, int dose)
        {
            this._idAppointment = idAppointment;
            this._idDrug = idDrug;
            this._startConsuming = startConsuming;
            this._dose = dose;
        }

        public override string ToString()
        {
            return this._idAppointment + "," + this._idDrug + "," + this._startConsuming.ToString("HH:mm") + "," + this._dose.ToString();
        }




    }
}
