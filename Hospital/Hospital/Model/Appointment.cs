using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    class Appointment
    {
        public enum AppointmentState
        {
            Created = 1,
            Updated = 2,
            Deleted = 3,
            UpdateRequest = 4,
            DeleteRequest = 5,
        }
        public enum TypeOfTerm
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
        private AppointmentState _state;
        private int _roomNumber;
        private TypeOfTerm _term;
        bool _appointmentPerformed;



        public string AppointmentId { get { return _id; } }
        public string PatientEmail { get { return _patientEmail; } }
        public string DoctorEmail { get { return _doctorEmail; } }
        public DateTime DateAppointment { get { return _dateAppointment; } }
        public DateTime StartTime { get { return _startTime; } }
        public DateTime EndTime { get { return _endTime; } }
        public AppointmentState GetAppointmentState { get { return _state; } }

        public void SetAppointmentState(AppointmentState appointmentState)
        {
            this._state = appointmentState;
        }

        public bool AppointmentPerformed { get { return _appointmentPerformed; } }

        public void SetAppointmentPerformed(bool appointmentPerformed)
        {
            this._appointmentPerformed = appointmentPerformed;
        }
        

        public int RoomNumber { get { return _roomNumber; } }
        public TypeOfTerm GetTypeOfTerm { get { return _term;} }

        public Appointment(string id, string patientEmail, string doctorEmail,
        DateTime dateAppointment, DateTime start, DateTime end, AppointmentState state, int roomNumber, TypeOfTerm term, bool appointmentPerformed)
        {
            this._id = id;
            this._patientEmail = patientEmail;
            this._doctorEmail = doctorEmail;
            this._dateAppointment = dateAppointment;
            this._startTime = start;
            this._endTime = end;
            this._state = state;
            this._roomNumber = roomNumber;
            this._term = term;
            this._appointmentPerformed = appointmentPerformed;
           
        }

        public override string ToString()
        {
            return "Doktor: " + this._doctorEmail + 
                " Datum: " + this._dateAppointment.Month + "/" + this._dateAppointment.Day + "/" + this._dateAppointment.Year +
                " Pocetak: " + this._startTime.Hour + ":" + this._startTime.Minute +
                " Kraj: " + this._endTime.Hour + ":" + this._endTime.Minute;
        }
        public string ToStringDisplayForDoctor(int serialNumber)
        {
            return String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}|{7,10}", serialNumber, this._patientEmail, this._dateAppointment.Month + "/" + this._dateAppointment.Day + "/" + this._dateAppointment.Year, this._startTime.Hour + ":" + this._startTime.Minute, this._endTime.Hour + ":" + this._endTime.Minute, this._roomNumber, this.GetTypeOfTerm,this.GetAppointmentState); 
              
        }


    }
}
