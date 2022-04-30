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
        public enum ActionState
        {
            Created = 1,
            Modified = 2,
            Deleted = 3
        }

        string patientEmail;
        DateTime actionDate;
        ActionState actionState;

        public string PatientEmail { get { return patientEmail; } }
        public DateTime ActionDate { get { return actionDate; } }
        public ActionState GetActionState { get { return actionState; } }

        public UserAction(string patientEmail, DateTime actionDate, ActionState state)
        {
            this.patientEmail = patientEmail;
            this.actionDate = actionDate;
            this.actionState = state;
        }

        public override string ToString()
        {
            return "\n" + this.patientEmail + "," + (int)this.actionState + 
                "," + this.actionDate.ToString("MM/dd/yyyy");
        }
    }
}
