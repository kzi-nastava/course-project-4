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
            Modified = 2,
            Deleted = 3,
            ChangeRequest = 4,
            DeleteRequest = 5
        }

        private string id;
        private string patientEmail;
        private string doctorEmail;
        private DateTime schedulingDate;
        private DateTime dateExamination;
        private DateTime startTime;
        private DateTime endTime;
        private AppointmentState state;

        public string AppointmentId { get { return id; } }
        public string PatientEmail { get { return patientEmail; } }
        public string DoctorEmail { get { return doctorEmail; } }
        public DateTime SchedulingDate { get { return schedulingDate; } }
        public DateTime DateExamination { get { return dateExamination; } }
        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get { return endTime; } }
        public AppointmentState GetAppointmentState { get { return state; } }

        public Appointment(string id, string patientEmail, string doctorEmail, DateTime schedulingDate,
        DateTime dateExamination, DateTime start, DateTime end, AppointmentState state)
        {
            this.id = id;
            this.patientEmail = patientEmail;
            this.doctorEmail = doctorEmail;
            this.schedulingDate = schedulingDate;
            this.dateExamination = dateExamination;
            this.startTime = start;
            this.endTime = end;
            this.state = state;
        }

        public override string ToString()
        {
            return "Doktor: " + this.doctorEmail + 
                " Datum: " + this.dateExamination.Day + "/" + this.dateExamination.Month + "/" + this.dateExamination.Year +
                " Pocetak: " + this.startTime.Hour + ":" + this.startTime.Minute +
                " Kraj: " + this.endTime.Hour + ":" + this.endTime.Minute;
        }
    }
}
