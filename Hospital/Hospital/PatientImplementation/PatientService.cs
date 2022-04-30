// HEADER
/*In PatientService there are functions that enable the implementation of the patient's functionalities*/

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
        AppointmentService _appointmentService = new AppointmentService();  // loading all _appointments
        List<Appointment> _allAppointments;
        List<User> _allUsers;
        User _currentRegisteredUser;

        // getters
        public List<Appointment> Appointments { get { return _allAppointments; } }

        public PatientService(User user, List<User> allUsers)
        {
            this._currentRegisteredUser = user;
            this._allUsers = allUsers;
            _allAppointments = _appointmentService.AppointmentRepository.Load();
        }

        public void RefreshPatientAppointments(Patient currentRegisteredPatient) 
        {
             this._allAppointments = _appointmentService.AppointmentRepository.Load();

            // finding all _appointments for the registered patient that have not been deleted and has not yet passed
            List<Appointment> patientCurrentAppointment = new List<Appointment>();
            foreach (Appointment appointment in _allAppointments)
            {
                if (appointment.PatientEmail.Equals(_currentRegisteredUser.Email) &&
                    appointment.AppointmentState != Appointment.State.Deleted &&
                    appointment.DateAppointment >= DateTime.Now.Date)
                {
                    // check if today's appointment has passed
                    if (appointment.DateAppointment == DateTime.Now.Date && appointment.StartTime.Hour <= DateTime.Now.Hour)
                        continue;
                    patientCurrentAppointment.Add(appointment);
                }
            }
            currentRegisteredPatient.PatientAppointments = patientCurrentAppointment;
        }

        public void TableHeader()
        {
            Console.WriteLine();
            Console.WriteLine(String.Format("{0,3}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}|{7,10}",
                "Br.", "Doktor", "Datum", "Pocetak", "Kraj", "Soba", "Tip", "Stanje"));
        }

        public List<Appointment> FindAppointmentsForDeleteAndUpdate(Patient currentlyRegisteredPatient)
        {
            int appointmentOrdinalNumber = 0;

            this.TableHeader();

            List<Appointment> appointmentsForChange = new List<Appointment>();
            foreach (Appointment appointment in currentlyRegisteredPatient.PatientAppointments) 
            {
                if (appointment.AppointmentState != Appointment.State.UpdateRequest &&
                    appointment.AppointmentState != Appointment.State.DeleteRequest && 
                    appointment.TypeOfTerm != Appointment.Type.Operation)
                {
                    appointmentsForChange.Add(appointment);
                    appointmentOrdinalNumber++;
                    Console.WriteLine(appointmentOrdinalNumber + ". " + appointment.DisplayOfPatientAppointment());
                }
            }
            Console.WriteLine();

            return appointmentsForChange;
        }

        public bool HasPatientAppointmen(Patient patient)
        {
            if (patient.PatientAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate nijedan zakazan pregled!");
                return false;
            }
            else
                return true;
        }

        public bool IsValidInput(string doctorEmail, string newDateAppointment, string newStartAppointment)
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

            foreach (User user in _allUsers) {
                if (user.Email.Equals(doctorEmail) && user.UserRole == User.Role.Doctor)
                    return true;
            }
            Console.WriteLine("Uneli ste nepostojeceg doktora");
            return false;
        }


        public bool IsAppointmentFree(string id, string patientEmail, string doctorEmail, string newDateExamination, string newStartTime)
        {
            DateTime dateExamination = DateTime.Parse(newDateExamination);
            DateTime startTime = DateTime.Parse(newStartTime);

            foreach (Appointment appointment in _appointmentService.Appointments) {
                if (appointment.DoctorEmail.Equals(doctorEmail) && appointment.DateAppointment == dateExamination
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime)
                {
                    Console.WriteLine("Izabran doktor je zauzet u tom terminu!");
                    return false;
                }
                else if (appointment.PatientEmail.Equals(patientEmail) && appointment.DateAppointment == dateExamination
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime && !appointment.AppointmentId.Equals(id))
                {
                    Console.WriteLine("Vec imate zakazan pregled u tom terminu!");
                    return false;
                }
            }
            return true;
        }

        public int GetNewAppointmentId() 
        {
            return this._allAppointments.Count + 1;
        }

        public List<UserAction> LoadMyCurrentActions(string registeredUserEmail)
        {
            UserActionService actionService = new UserActionService();  // loading all _users actions
            List<UserAction> myCurrentActions = new List<UserAction>();

            foreach (UserAction action in actionService.Actions)
            {
                if (registeredUserEmail.Equals(action.PatientEmail) && (DateTime.Now - action.ActionDate).TotalDays <= 30)
                    myCurrentActions.Add(action);
            }
            return myCurrentActions;
        }

        public void BlockAccessApplication(string registeredUserEmail)
        {
            // read from file
            string filePath = @"..\..\Data\users.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string userEmailFromFile = fields[1];
                if (registeredUserEmail.Equals(userEmailFromFile))
                    lines[i] = fields[0] + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4]
                        + "," + (int) User.State.BlockedBySystem;
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }

        public void AppendToActionFile(string registeredUserEmail, string typeAction)
        {
            string filePath = @"..\..\Data\actions.csv";

            UserAction newAction;
            if (typeAction.Equals("create"))
                newAction = new UserAction(registeredUserEmail, DateTime.Now, UserAction.State.Created);
            else if(typeAction.Equals("update"))
                newAction = new UserAction(registeredUserEmail, DateTime.Now, UserAction.State.Modified);
            else
                newAction = new UserAction(registeredUserEmail, DateTime.Now, UserAction.State.Deleted);

            File.AppendAllText(filePath, newAction.ToString());
        }

        public Room FindFreeRoom(Patient patient, DateTime newDate, DateTime newStartTime)
        {
            RoomService roomService = new RoomService();
            List<Room> freeRooms = roomService.Rooms;  // at the beginning all the rooms are free

            foreach (Appointment appointment in this._allAppointments)
            {
                if (appointment.DateAppointment == newDate && newStartTime >= appointment.StartTime 
                    && newStartTime < appointment.EndTime && appointment.AppointmentState != Appointment.State.Deleted)
                {
                    Room occupiedRoom = roomService.GetRoomById(appointment.RoomNumber.ToString());
                    freeRooms.Remove(occupiedRoom);
                }
            }

            if (freeRooms.Count == 0)
            {
                Console.WriteLine("\nNe postoji nijedna slobodna soba za unesen termin.");
                return null;
            }
            else
                return freeRooms[0];
        }
    }
}
