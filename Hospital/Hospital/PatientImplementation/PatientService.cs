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
using Hospital.DoctorImplementation;

namespace Hospital.PatientImplementation
{
    class PatientService
    {
        RequestService _requestService;
        AppointmentService _appointmentService = new AppointmentService();  // loading all appointments
        List<Appointment> _allAppointments;
        List<User> _allUsers;
        User _currentRegisteredUser;

        // getters
        public List<Appointment> Appointments { get { return _allAppointments; } }

        public RequestService RequestService { get { return _requestService; } }

        public AppointmentService AppointmentService { get { return _appointmentService; } }

        public PatientService(User user, List<User> allUsers)
        {
            _requestService = new RequestService(_appointmentService);
            this._currentRegisteredUser = user;
            this._allUsers = allUsers;
            _allAppointments = _appointmentService.AppointmentRepository.Load();
        }

        public List<Appointment> RefreshPatientAppointments() 
        {
             this._allAppointments = _appointmentService.AppointmentRepository.Load();

            // finding all appointments for the registered patient that have not been deleted and has not yet passed
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
            return patientCurrentAppointment;
        }

        public List<Appointment> FindAppointmentsForDeleteAndUpdate(Patient currentlyRegisteredPatient)
        {
            this._appointmentService.TableHeaderForPatient();
            Console.WriteLine();

            List<Appointment> appointmentsForChange = new List<Appointment>();
            foreach (Appointment appointment in currentlyRegisteredPatient.PatientAppointments) 
            {
                if (appointment.AppointmentState != Appointment.State.UpdateRequest &&
                    appointment.AppointmentState != Appointment.State.DeleteRequest && 
                    appointment.TypeOfTerm != Appointment.Type.Operation)
                {
                    appointmentsForChange.Add(appointment);
                    Console.WriteLine(appointmentsForChange.Count + ". " + appointment.DisplayOfPatientAppointment());
                }
            }
            Console.WriteLine();

            return appointmentsForChange;
        }

        public bool HasPatientAppointmen(List<Appointment> patientAppointments)
        {
            if (!(patientAppointments.Count == 0))
                return true;
            Console.WriteLine("\nNemate ni jedan pregled za prikaz!");
            return false;
        }

        public string[] InputValuesForAppointment()
        {
            string[] inputValues = new string[3];

            string doctorEmail;
            string newDate;
            string newStartTime;

            do
            {
                Console.Write("\nUnesite email doktora: ");
                doctorEmail = Console.ReadLine();
                Console.Write("Unesite datum (MM/dd/yyyy): ");
                newDate = Console.ReadLine();
                Console.Write("Unesite vreme pocetka pregleda (HH:mm): ");
                newStartTime = Console.ReadLine();
            } while (!this.IsValidInput(doctorEmail, newDate, newStartTime));

            inputValues[0] = doctorEmail;
            inputValues[1] = newDate;
            inputValues[2] = newStartTime;

            return inputValues;
        }

        public bool IsValidInput(string doctorEmail, string newDateAppointment, string newStartTime)
        {           
            return (_appointmentService.IsDateFormValid(newDateAppointment) &&
                _appointmentService.IsTimeFormValid(newStartTime) && _appointmentService.IsDoctorExist(doctorEmail)!=null);
        }

        public List<UserAction> LoadMyCurrentActions(string registeredUserEmail)
        {
            UserActionService actionService = new UserActionService();  // loading all users actions
            List<UserAction> myCurrentActions = new List<UserAction>();

            foreach (UserAction action in actionService.Actions)
            {
                if (registeredUserEmail.Equals(action.PatientEmail) && (DateTime.Now - action.ActionDate).TotalDays <= 30)
                    myCurrentActions.Add(action);
            }
            return myCurrentActions;
        }

        public void BlockAccessApplication()
        {
            // read from file
            string filePath = @"..\..\Data\users.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string userEmailFromFile = fields[1];
                if (_currentRegisteredUser.Email.Equals(userEmailFromFile))
                    lines[i] = fields[0] + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4]
                        + "," + (int) User.State.BlockedBySystem + "," + "null";
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }

        public void AppendToActionFile(string typeAction)
        {
            string filePath = @"..\..\Data\actions.csv";

            UserAction newAction;
            if (typeAction.Equals("create"))
                newAction = new UserAction(_currentRegisteredUser.Email, DateTime.Now, UserAction.State.Created);
            else if(typeAction.Equals("update"))
                newAction = new UserAction(_currentRegisteredUser.Email, DateTime.Now, UserAction.State.Modified);
            else
                newAction = new UserAction(_currentRegisteredUser.Email, DateTime.Now, UserAction.State.Deleted);

            File.AppendAllText(filePath, newAction.ToString());
        }

        public Appointment PickAppointmentForDeleteOrUpdate(Patient patient)
        {
            List<Appointment> appointmentsForChange = this.FindAppointmentsForDeleteAndUpdate(patient);

            if (appointmentsForChange.Count == 0)
                return null;

            string inputNumberAppointment;
            int numberAppointment;
            do
            {
                Console.WriteLine("Unesite broj pregleda za izabranu operaciju");
                Console.Write(">> ");
                inputNumberAppointment = Console.ReadLine();
            } while (!int.TryParse(inputNumberAppointment, out numberAppointment) || numberAppointment < 1
            || numberAppointment > appointmentsForChange.Count);

            return appointmentsForChange[numberAppointment - 1];
        }

        public void ReadFileForDeleteAppointment(Appointment appointmentForDelete)
        {
            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForDelete.AppointmentId))
                {

                    if ((appointmentForDelete.DateAppointment - DateTime.Now).TotalDays <= 2)
                    {
                        appointmentForDelete.AppointmentState = Appointment.State.DeleteRequest;
                        this._requestService.Requests.Add(appointmentForDelete);
                        this._requestService.UpdateFile();
                        Console.WriteLine("Zahtev za brisanje je poslat sekretaru!");
                    }
                    else
                    {
                        // logical deletion
                        appointmentForDelete.AppointmentState = Appointment.State.Deleted;
                        Console.WriteLine("Uspesno ste obrisali pregled!");
                    }
                    lines[i] = appointmentForDelete.ToString();
                }
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }

        public void ReadFileForUpdateAppointment(Appointment appointmentForUpdate, string[] inputValues)
        {

            string doctorEmail = inputValues[0];

            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForUpdate.AppointmentId))
                {
                    Appointment newAppointment;
                    DateTime appointmentDate = DateTime.ParseExact(inputValues[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);

                    if ((appointmentForUpdate.DateAppointment - DateTime.Now).TotalDays <= 2)
                    {
                        appointmentForUpdate.AppointmentState = Appointment.State.UpdateRequest;
                        newAppointment = new Appointment(id, this._currentRegisteredUser.Email, doctorEmail, appointmentDate,
                        appointmentStartTime, appointmentEndTime, Appointment.State.UpdateRequest, Int32.Parse(fields[7]),
                        Appointment.Type.Examination, false, false);


                        this._requestService.Requests.Add(newAppointment);
                        this._requestService.UpdateFile();
                        Console.WriteLine("Zahtev za izmenu je poslat sekretaru!");
                    }
                    else
                    {
                        appointmentForUpdate.AppointmentState = Appointment.State.Updated;
                        appointmentForUpdate.DoctorEmail = doctorEmail;
                        appointmentForUpdate.DateAppointment = appointmentDate;
                        appointmentForUpdate.StartTime = startTime;
                        appointmentForUpdate.EndTime = startTime.AddMinutes(15);
                        Console.WriteLine("Uspesno ste izvrsili izmenu pregleda!");
                    }
                    lines[i] = appointmentForUpdate.ToString();
                }
                // saving changes
                File.WriteAllLines(filePath, lines);
            }
        }

        public bool CompareTwoTimes(string startTime, string endTime)
        {
            DateTime initialTime = DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture);
            DateTime latestTime = DateTime.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture);

            if (initialTime.AddMinutes(15) > latestTime)
                return false;

            return initialTime < latestTime;
        }

        public string[] InputValueForRecommendationAppointments()
        {
            string[] inputValues = new string[4];

            string doctorEmail;
            string latestDate;
            string startTime;
            string endTime;

            do
            {
                Console.Write("\nUnesite email doktora: ");
                doctorEmail = Console.ReadLine();
                Console.Write("Unesite datum do kog mora da bude pregled (MM/dd/yyyy): ");
                latestDate = Console.ReadLine();
                Console.Write("Unesite vreme najranije moguceg pregleda (HH:mm): ");
                startTime = Console.ReadLine();
                Console.Write("Unesite vreme najkasnijeg moguceg pregleda (HH:mm): ");
                endTime = Console.ReadLine();
            } while (!(this.IsValidInput(doctorEmail, latestDate, startTime) && _appointmentService.IsTimeFormValid(endTime)
            && this.CompareTwoTimes(startTime, endTime)));

            inputValues[0] = doctorEmail;
            inputValues[1] = latestDate;
            inputValues[2] = startTime;
            inputValues[3] = endTime;

            return inputValues;
        }
    }
}
