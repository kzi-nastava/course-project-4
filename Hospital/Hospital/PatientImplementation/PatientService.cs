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
    class PatientService
    {
        AppointmentService appointmentService = new AppointmentService();  // loading all appointments
        List<Appointment> allAppointments;
        List<User> allUsers;
        User currentRegisteredUser;

        // getters
        public List<Appointment> Appointments { get { return allAppointments; } }

        public PatientService(User user, List<User> allUsers)
        {
            this.currentRegisteredUser = user;
            this.allUsers = allUsers;
            allAppointments = appointmentService.AppointmentRepository.Load();
        }

        public void refreshPatientAppointments(Patient currentlyRegisteredPatient) 
        {
             this.allAppointments = appointmentService.AppointmentRepository.Load();

            // finding all appointments for the registered patient that have not been deleted and has not yet passed
            List<Appointment> patientCurrentAppointment = new List<Appointment>();
            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.PatientEmail.Equals(currentRegisteredUser.Email) &&
                    appointment.GetAppointmentState != Appointment.AppointmentState.Deleted &&
                    appointment.DateAppointment >= DateTime.Now.Date)
                {
                    // check if today's appointment has passed
                    if (appointment.DateAppointment == DateTime.Now.Date && appointment.StartTime.Hour <= DateTime.Now.Hour)
                        continue;
                    patientCurrentAppointment.Add(appointment);
                }
            }
            currentlyRegisteredPatient.PatientAppointments = patientCurrentAppointment;
        }

        public List<Appointment> findAppointmentsForDeleteAndUpdate(Patient currentlyRegisteredPatient, string typeAction)
        {
            int appointmentOrdinalNumber = 0;

            if(typeAction.Equals("delete"))
                Console.WriteLine("\n\tPREGLEDI ZA BRISANJE");
            else
                Console.WriteLine("\n\tPREGLEDI ZA IZMENU");
            Console.Write("-----------------------------------------\n");

            List<Appointment> appointmentsForChange = new List<Appointment>();
            foreach (Appointment appointment in currentlyRegisteredPatient.PatientAppointments) 
            {
                if (appointment.GetAppointmentState != Appointment.AppointmentState.UpdateRequest &&
                    appointment.GetAppointmentState != Appointment.AppointmentState.DeleteRequest && 
                    appointment.GetTypeOfTerm != Appointment.TypeOfTerm.Operation)
                {
                    appointmentsForChange.Add(appointment);
                    appointmentOrdinalNumber++;
                    Console.WriteLine(appointmentOrdinalNumber + ". " + appointment.ToString());
                }
            }
            Console.WriteLine();

            return appointmentsForChange;
        }

        public bool hasPatientAppointment(Patient patient)
        {
            if (patient.PatientAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate nijedan zakazan pregled!");
                return false;
            }
            else
                return true;
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


        public bool isAppointmentFree(string patientEmail, string doctorEmail, string newDateExamination, string newStartTime)
        {
            DateTime dateExamination = DateTime.Parse(newDateExamination);
            DateTime startTime = DateTime.Parse(newStartTime);

            foreach (Appointment appointment in appointmentService.Appointments) {
                if (appointment.DoctorEmail.Equals(doctorEmail) && appointment.DateAppointment == dateExamination
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime)
                {
                    Console.WriteLine("Izabran doktor je zauzet u tom terminu!");
                    return false;
                }
                else if (appointment.PatientEmail.Equals(patientEmail) && appointment.DateAppointment == dateExamination
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime)
                {
                    Console.WriteLine("Vec imate zakazan pregled u tom terminu!");
                    return false;
                }
            }
            return true;
        }

        public int getNewAppointmentId() 
        {
            return this.allAppointments.Count + 1;
        }

        public List<UserAction> loadMyCurrentActions(string registeredUserEmail)
        {
            ActionService actionService = new ActionService();  // loading all users actions
            List<UserAction> myCurrentActions = new List<UserAction>();

            foreach (UserAction action in actionService.Actions)
            {
                if (registeredUserEmail.Equals(action.PatientEmail) && (DateTime.Now - action.ActionDate).TotalDays <= 30)
                    myCurrentActions.Add(action);
            }
            return myCurrentActions;
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

        public void appendToActionFile(string registeredUserEmail, string typeAction)
        {
            string filePath = @"..\..\Data\actions.csv";

            UserAction newAction;
            if (typeAction.Equals("kreiranje"))
                newAction = new UserAction(registeredUserEmail, DateTime.Now, UserAction.ActionState.Created);
            else if(typeAction.Equals("izmena"))
                newAction = new UserAction(registeredUserEmail, DateTime.Now, UserAction.ActionState.Modified);
            else
                newAction = new UserAction(registeredUserEmail, DateTime.Now, UserAction.ActionState.Deleted);

            File.AppendAllText(filePath, newAction.ToString());
        }
    }
}
