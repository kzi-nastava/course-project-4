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
            DeleteRequest = 5
        }
        public enum TypeOfTerm
        {
            Examination = 1,
            Operation = 2
        }

        private string id;
        private string patientEmail;
        private string doctorEmail;
        private DateTime dateAppointment;
        private DateTime startTime;
        private DateTime endTime;
        private AppointmentState state;
        private int roomNumber;
        private TypeOfTerm term;


        public string AppointmentId { get { return id; } }
        public string PatientEmail { get { return patientEmail; } }
        public string DoctorEmail { get { return doctorEmail; } }
        public DateTime DateAppointment { get { return dateAppointment; } }
        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get { return endTime; } }
        public AppointmentState GetAppointmentState { get { return state; } }

        public int RoomNumber { get { return roomNumber; } }
        public TypeOfTerm GetTypeOfTerm { get { return term;} }

        public Appointment(string id, string patientEmail, string doctorEmail,
        DateTime dateAppointment, DateTime start, DateTime end, AppointmentState state, int roomNumber, TypeOfTerm term)
        {
            this.id = id;
            this.patientEmail = patientEmail;
            this.doctorEmail = doctorEmail;
            this.dateAppointment = dateAppointment;
            this.startTime = start;
            this.endTime = end;
            this.state = state;
            this.roomNumber = roomNumber;
            this.term = term;
        }

        public override string ToString()
        {
            return "Doktor: " + this.doctorEmail + 
                " Datum: " + this.dateAppointment.Month + "/" + this.dateAppointment.Day + "/" + this.dateAppointment.Year +
                " Pocetak: " + this.startTime.Hour + ":" + this.startTime.Minute +
                " Kraj: " + this.endTime.Hour + ":" + this.endTime.Minute;
        }
        public string ToStringDisplayForDoctor(int serialNumber)
        {
            return String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", serialNumber, this.patientEmail, this.dateAppointment.Month + "/" + this.dateAppointment.Day + "/" + this.dateAppointment.Year, this.startTime.Hour + ":" + this.startTime.Minute, this.endTime.Hour + ":" + this.endTime.Minute, this.roomNumber, this.GetTypeOfTerm); 
              
        }


    }
}
