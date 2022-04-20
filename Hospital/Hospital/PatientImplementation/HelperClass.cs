using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using System.Globalization;

namespace Hospital.PatientImplementation
{
    class HelperClass
    {
        AppointmentService appointmentService = new AppointmentService();  // loading all appointments
        List<User> allUsers;
        User currentRegisteredUser;

        public HelperClass(User user, List<User> allUsers)
        {
            this.currentRegisteredUser = user;
            this.allUsers = allUsers;
        }

        public List<Appointment> refreshPatientAppointments() 
        {
            List<Appointment> allAppointments = appointmentService.AppointmentRepository.Load();

            // finding all appointments that have not passed for the registered patient
            List<Appointment> patientAppointment = new List<Appointment>();
            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.PatientEmail.Equals(currentRegisteredUser.Email) &&
                appointment.DateExamination > DateTime.Now)
                    patientAppointment.Add(appointment);
            }
            return patientAppointment;
        }

        public bool isValidInput(string doctorEmail, string newDateAppointment, string newStartAppointment)
        {
            DateTime checkDate;
            bool validDate = DateTime.TryParseExact(newDateAppointment, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out checkDate);
            if (!validDate)
            {
                Console.WriteLine("Nevalidan unos datuma");
                return false;
            }
            else if(checkDate < DateTime.Now)
            {
                Console.WriteLine("Uneli ste datum koji je prosao");
                return false;
            }

            TimeSpan checkTime;
            bool validTime = TimeSpan.TryParse(newStartAppointment, out checkTime);
            if (!validTime)
            {
                Console.WriteLine("Nevalidan unos vremena");
                return false;
            }

            foreach (User user in allUsers) {
                if (user.Email.Equals(doctorEmail) && user.UserRole == User.Role.Doctor)
                    return true;
            }
            Console.WriteLine("Uneli ste nepostojeceg doktora");
            return false;
        }

        public bool isAppointmentFree(string doctorEmail, string newDateExamination, string newStartTime)
        {
            DateTime dateExamination = DateTime.Parse(newDateExamination);
            DateTime startTime = DateTime.Parse(newStartTime);

            foreach (Appointment appointment in appointmentService.Appointments) {
                if (appointment.DoctorEmail.Equals(doctorEmail) && appointment.DateExamination == dateExamination
                    && appointment.StartTime == startTime)
                {
                    Console.WriteLine("Termin je vec zauzet!");
                    return false;
                }
            }
            return true;
        }
    }
}
