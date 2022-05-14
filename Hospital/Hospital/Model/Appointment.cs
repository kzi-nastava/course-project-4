using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Appointment
    {
        public enum State
        {
            Created = 1,
            Updated = 2,
            Deleted = 3,
            UpdateRequest = 4,
            DeleteRequest = 5
        }
        public enum Type
        {
            Examination = 1,
            Operation = 2
        }

        private string _id;
        private string _patientEmail;
        private string _doctorEmail;
        private DateTime _dateAppointment;
        private DateTime _startTime;
        private DateTime _endTime;
        private State _appointmentState;
        private int _roomNumber;
        private Type _typeOfTerm;
        bool _appointmentPerformed;

        public string AppointmentId { get { return _id; } }
        public string PatientEmail { get { return _patientEmail;  } set { this._patientEmail = value; } }
        public string DoctorEmail { get { return _doctorEmail; } set { this._doctorEmail = value; } }
        public DateTime DateAppointment { get { return _dateAppointment; } set { _dateAppointment = value; } }
        public DateTime StartTime { get { return _startTime; } set { _startTime = value; } }
        public DateTime EndTime { get { return _endTime; } set { _endTime = value; } }
        public State AppointmentState { get { return _appointmentState; } set { _appointmentState = value; } }
        public int RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }
        public Type TypeOfTerm { get { return _typeOfTerm;} }
        public bool AppointmentPerformed { get { return _appointmentPerformed; } set { _appointmentPerformed = value; } }
        

        public Appointment(string id, string patientEmail, string doctorEmail,
        DateTime dateAppointment, DateTime start, DateTime end, State state, int roomNumber, Type term, bool appointment)
        {
            this._id = id;
            this._patientEmail = patientEmail;
            this._doctorEmail = doctorEmail;
            this._dateAppointment = dateAppointment;
            this._startTime = start;
            this._endTime = end;
            this._appointmentState = state;
            this._roomNumber = roomNumber;
            this._typeOfTerm = term;
            this._appointmentPerformed = appointment;
        }

        public override string ToString()
        {
            return this._id + "," + this._patientEmail + "," + this._doctorEmail + "," + 
                this.DateAppointment.ToString("MM/dd/yyyy") + "," + this._startTime.ToString("HH:mm") + "," + 
                this._endTime.ToString("HH:mm") + "," + (int)this.AppointmentState
                + "," + this._roomNumber + "," + (int)this.TypeOfTerm + "," + "false";
        }

        public string DisplayOfPatientAppointment()
        {
            string typeOfTerm = "pregled";
            string appointmentState = "aktivno";
            if (this.TypeOfTerm == Appointment.Type.Operation)
                typeOfTerm = "operacija";
            else
            {
                if (this.AppointmentState == State.UpdateRequest)
                    appointmentState = "Poslato sekretaru na izmenu";
                else if(this.AppointmentState == State.DeleteRequest)
                    appointmentState = "Poslato sekretaru na brisanje";
            }
            return String.Format("|{0,10}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}",
                this._doctorEmail, this._dateAppointment.ToString("MM/dd/yyyy"), this._startTime.ToString("HH:mm"), this._endTime.ToString("HH:mm"), this._roomNumber, typeOfTerm, appointmentState); ;
        }
        public string ToStringDisplayForDoctor(int serialNumber)
        {
            return String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", serialNumber, this._patientEmail, this._dateAppointment.Month + "/" + this._dateAppointment.Day + "/" + this._dateAppointment.Year, this._startTime.Hour + ":" + this._startTime.Minute, this._endTime.Hour + ":" + this._endTime.Minute, this._roomNumber, this.TypeOfTerm); 
              
        }
    }
}
