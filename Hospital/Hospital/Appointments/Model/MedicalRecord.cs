using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Appointments.Model
{
    public class MedicalRecord
    {
        private string _idAppointment;
        private string _anamnesis;
        private string _referralToDoctor;

        public string IdAppointment { get { return _idAppointment; } }
        public string Anamnesis { get { return _anamnesis; } }

        public string ReferralToDoctor { get { return _referralToDoctor; } }

        public MedicalRecord(string idAppointment, string anamnesis, string referralToDoctor)
        {
            this._idAppointment = idAppointment;
            this._anamnesis = anamnesis;
            this._referralToDoctor = referralToDoctor;
        }

    }
}
