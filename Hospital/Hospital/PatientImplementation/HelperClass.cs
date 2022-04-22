// HEADER
/*In HelperClass there are functions that enable the implementation of the patient's functionalities*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using System.Globalization;
using System.IO;

namespace Hospital.PatientImplementation
{
    class HelperClass
    {
        AppointmentService appointmentService = new AppointmentService();  // loading all appointments
        List<Appointment> allAppointments;
        List<User> allUsers;
        User currentRegisteredUser;

        // getters
        public List<Appointment> Appointments { get { return allAppointments; } }

        public HelperClass(User user, List<User> allUsers)
        {
            this.currentRegisteredUser = user;
            this.allUsers = allUsers;
            allAppointments = appointmentService.AppointmentRepository.Load();
        }

        public List<Appointment> refreshPatientAppointments() 
        {
             this.allAppointments = appointmentService.AppointmentRepository.Load();

            // finding all appointments for the registered patient
            List<Appointment> patientAppointment = new List<Appointment>();
            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.PatientEmail.Equals(currentRegisteredUser.Email))
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
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime)
                {
                    Console.WriteLine("Termin je vec zauzet!");
                    return false;
                }
            }
            return true;
        }

        public int getNewAppointmentId() 
        {
            return this.allAppointments.Count + 1;
        }

        public void blockAccessApplication(string registeredUserEmail)
        {
            // read from file
            string filePath = @"..\..\Data\users.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string userEmailFromFile = fields[1];
                if (registeredUserEmail.Equals(userEmailFromFile))
                    lines[i] = fields[0] + "," + fields[1] + "," + fields[2] + "," + (int) User.State.BlockedBySystem;
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }
    }
}
