using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.PatientImplementation;

namespace Hospital.Model
{
    class UserAction
    {
        public enum State
        {
            Created = 1,
            Modified = 2,
            Deleted = 3
        }

        string _patientEmail;
        DateTime _actionDate;
        State _actionState;

        public string PatientEmail { get { return _patientEmail; } }
        public DateTime ActionDate { get { return _actionDate; } }
        public State ActionState { get { return _actionState; } }

        public UserAction(string patientEmail, DateTime actionDate, State state)
        {
            this._patientEmail = patientEmail;
            this._actionDate = actionDate;
            this._actionState = state;
        }

        public override string ToString()
        {
            return "\n" + this._patientEmail + "," + (int)this._actionState + 
                "," + this._actionDate.ToString("MM/dd/yyyy");
        }
    }
}
