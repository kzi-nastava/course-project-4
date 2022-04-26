using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.PatientImplementation;

namespace Hospital.Model
{
    class Action
    {
        public enum ActionState
        {
            Created = 1,
            Modified = 2,
            Deleted = 3
        }

        Patient patient;
        DateTime actionDate;
        ActionState actionState;

        public Patient AppointmentId { get { return patient; } }
        public DateTime ActionDate { get { return actionDate; } }
        public ActionState GetActionState { get { return actionState; } }
    }
}
