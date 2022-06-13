using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Users.Model
{
    public class RequestForDaysOff
    {
        public enum State
        {
            Accepted = 1,
            Rejected = 2,
            Waiting = 3
        }
        private string _id;
        private string _emailDoctor;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _reasonRequired;
        private State _state;
        private bool _urgent;

        public RequestForDaysOff(string id,string emailDoctor, DateTime startDate, DateTime endDate, string reasonRequired, State state, bool urgen)
        {
            this._id = id;
            this._emailDoctor = emailDoctor;
            this._startDate = startDate;
            this._endDate = endDate;
            this._reasonRequired = reasonRequired;
            this._state = state;
            this._urgent = urgen;
        }

        public string Id { get { return _id; } set { _id = value; } }

        public string EmailDoctor { get { return _emailDoctor; } set { _emailDoctor = value; } }

        public DateTime StartDate { get { return _startDate; } set { _startDate = value; } }

        public DateTime EndDate { get { return _endDate; } set { _endDate = value; } }

        public string ReasonRequired { get { return _reasonRequired; }set { _reasonRequired = value;}  }

        public State StateRequired { get { return _state; }set{_state = value;}  }

        public bool Urgen { get { return _urgent; } set { _urgent = value; } }

        public override string ToString()
        {
            return this._id + ";" + this._emailDoctor + ";" + this._startDate.ToString("MM/dd/yyyy") + ";" + this._endDate.ToString("MM/dd/yyyy") + ";" + this._reasonRequired + ";" + (int)this._state + ";" + this._urgent;
        }

    }
}
