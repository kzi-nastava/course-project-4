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

        private string patientEmail;
        private string doctorEmail;
        private DateTime dateExamination;
        private DateTime startExamination;
        private DateTime endExamination;
        private AppointmentState state;

        public string PatientEmail { get { return patientEmail; } }
        public string DoctorEmail { get { return doctorEmail; } }
        public DateTime DateExamination { get { return dateExamination; } }
        public DateTime StartExamination { get { return startExamination; } }
        public DateTime EndExamination { get { return endExamination; } }
        public AppointmentState UserState { get { return state; } }

        public Appointment(string patientEmail, string doctorEmail, DateTime date, DateTime start, DateTime end, AppointmentState state)
        {
            this.patientEmail = patientEmail;
            this.doctorEmail = doctorEmail;
            this.dateExamination = date;
            this.startExamination = start;
            this.endExamination = end;
            this.state = state;
        }
    }
}
