using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using System.IO;
using System.Globalization;
using Hospital.Service;

namespace Hospital.PatientImplementation
{
    class Patient
    {
        string _email;
        PatientService _patientService;
        List<Appointment> _currentAppointments; 

        public string Email { get { return _email; } }
        public List<Appointment> PatientAppointments
        {
            get { return _currentAppointments; }
            set { _currentAppointments = value; }
        } 

        public Patient(string email, PatientService patientService)
        {
            this._email = email;
            this._patientService = patientService;
            patientService.RefreshPatientAppointments(this);
        }

        // methods
        public void PatientMenu()
        {
            // the menu
            string choice;
            Console.WriteLine("\n\tMENI");
            Console.Write("------------------");
            do
            {
                Console.WriteLine("\n1. Lista sopstvenih pregleda");
                Console.WriteLine("2. Kreiraj pregled");
                Console.WriteLine("3. Izmeni pregled");
                Console.WriteLine("4. Obrisi pregled");
                Console.WriteLine("5. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                // patient choice
                if (choice.Equals("1"))
                    this.ReadOwnAppointments();
                else if (choice.Equals("2"))
                    this.CreateAppointment();
                else if (choice.Equals("3"))
                    this.UpdateAppointment();
                else if (choice.Equals("4"))
                    this.DeleteAppointment();
                else if (choice.Equals("5"))
                    this.LogOut();
            } while (true);
        }

        private void ReadOwnAppointments()
        {
            if (!_patientService.HasPatientAppointmen(this))
                return;
                
            int serialNumber = 0;

            _patientService.TableHeader();
            
            foreach (Appointment appointment in this._currentAppointments)
            {
                serialNumber++;
                Console.WriteLine(serialNumber + ". " + appointment.DisplayOfPatientAppointment());
            }
            Console.WriteLine();
        }

        private void DeleteAppointment() 
        {
            // first check if patient has _appointments for delete
            if (!_patientService.HasPatientAppointmen(this))
                return;

            // pick appointment for delete
            List<Appointment> appointmentsForDelete = _patientService.FindAppointmentsForDeleteAndUpdate(this);
            string inputNumberAppointment;
            int numberAppointment;
            do
            {
                Console.WriteLine("Unesite broj pregleda za brisanje");
                Console.Write(">> ");
                inputNumberAppointment = Console.ReadLine();
            } while (!int.TryParse(inputNumberAppointment, out numberAppointment) || numberAppointment < 1 
            || numberAppointment > appointmentsForDelete.Count);

            Appointment appointmentForDelete = appointmentsForDelete[numberAppointment-1];

            // read from file
            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForDelete.AppointmentId)) {

                    if ((appointmentForDelete.DateAppointment - DateTime.Now).TotalDays <= 2)
                    {
                        appointmentForDelete.AppointmentState = Appointment.State.DeleteRequest;
                        _patientService.RequestService.Requests.Add(appointmentForDelete);
                        _patientService.RequestService.UpdateFile();
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

            //refresh data
            _patientService.RefreshPatientAppointments(this);

            // append new action in action file
            _patientService.AppendToActionFile(this._email, "delete");

            // check number of changed, deleted and created _appointments
            this.AntiTrolMechanism();
        }

        private void UpdateAppointment()
        {
            // first check if patient has _appointments for update
            if (!_patientService.HasPatientAppointmen(this))
                return;

            // pick appointment for update
            List<Appointment> appointmentsForUpdate = _patientService.FindAppointmentsForDeleteAndUpdate(this);
            string inputNumberAppointment;
            int numberAppointment;
            do
            {
                Console.WriteLine("Unesite broj pregleda za izmenu");
                Console.Write(">> ");
                inputNumberAppointment = Console.ReadLine();
            } while (!int.TryParse(inputNumberAppointment, out numberAppointment) || numberAppointment < 1
            || numberAppointment > appointmentsForUpdate.Count);

            Appointment appointmentForUpdate = appointmentsForUpdate[numberAppointment - 1];

            // update
            string doctorEmail;
            string newDate;
            string newStartTime;

            do
            {
                // input new values
                Console.Write("\nUnesite email doktora: ");
                doctorEmail = Console.ReadLine();
                Console.Write("Unesite datum (MM/dd/yyyy): ");
                newDate = Console.ReadLine();
                Console.Write("Unesite vreme pocetka pregleda (HH:mm): ");
                newStartTime = Console.ReadLine();
            } while (!_patientService.IsValidInput(doctorEmail, newDate, newStartTime));

            if (_patientService.IsAppointmentFree(appointmentForUpdate.AppointmentId, this._email, doctorEmail, newDate, newStartTime))
            {
                // read from file
                string filePath = @"..\..\Data\appointments.csv";
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(new[] { ',' });
                    string id = fields[0];

                    if (id.Equals(appointmentForUpdate.AppointmentId))
                    {
                        Appointment newAppointment;
                        DateTime appointmentDate = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime appointmentStartTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                        DateTime appointmentEndTime = appointmentStartTime.AddMinutes(15);

                        if ((appointmentForUpdate.DateAppointment - DateTime.Now).TotalDays <= 2)
                        {
                            appointmentForUpdate.AppointmentState = Appointment.State.UpdateRequest;
							newAppointment = new Appointment(id, this._email, doctorEmail, appointmentDate,
                            appointmentStartTime, appointmentEndTime, Appointment.State.UpdateRequest, Int32.Parse(fields[7]),
                            Appointment.Type.Examination, false);
                          
                            _patientService.RequestService.Requests.Add(newAppointment);
                            _patientService.RequestService.UpdateFile();
                            Console.WriteLine("Zahtev za izmenu je poslat sekretaru!");
                        }
                        else
                        {
                            appointmentForUpdate.AppointmentState = Appointment.State.Updated;
                            appointmentForUpdate.DoctorEmail = doctorEmail;
                            appointmentForUpdate.DateAppointment = appointmentDate;
                            appointmentForUpdate.StartTime = appointmentStartTime;
                            appointmentForUpdate.EndTime = appointmentEndTime;
                            Console.WriteLine("Uspesno ste izvrsili izmenu pregleda!");
                        }
                        lines[i] = appointmentForUpdate.ToString();
                    }
                }

                // saving changes
                File.WriteAllLines(filePath, lines);

                //refresh data
                _patientService.RefreshPatientAppointments(this);

                // append new action in action file
                _patientService.AppendToActionFile(this._email, "update");

                // check number of changed, deleted and created _appointments
                this.AntiTrolMechanism();
            }

        }

        private void CreateAppointment()
        {
            string doctorEmail;
            string newDate;
            string newStartTime;

            do
            {
                // input values to create an new appointment
                Console.Write("\nUnesite email doktora: ");
                doctorEmail = Console.ReadLine();
                Console.Write("Unesite datum (MM/dd/yyyy): ");
                newDate = Console.ReadLine();
                Console.Write("Unesite vreme pocetka pregleda (HH:mm): ");
                newStartTime = Console.ReadLine();
            } while (!_patientService.IsValidInput(doctorEmail, newDate, newStartTime));

            if (_patientService.IsAppointmentFree("0", this._email, doctorEmail, newDate, newStartTime))
            {
                string id = _patientService.GetNewAppointmentId().ToString();
                DateTime appointmentDate = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime appointmentStartTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                DateTime appointmentEndTime = appointmentStartTime.AddMinutes(15);

                Room freeRoom = _patientService.FindFreeRoom(appointmentDate, appointmentStartTime);
                int roomId = Int32.Parse(freeRoom.Id);

                //  created appointment
                Appointment newAppointment = new Appointment(id, this._email, doctorEmail, appointmentDate, 
                    appointmentStartTime, appointmentEndTime, Appointment.State.Created, roomId, 
                    Appointment.Type.Examination,false);

                Console.WriteLine("Uspesno ste kreirali nov pregled!");

                // append new appointment in file
                string filePath = @"..\..\Data\appointments.csv";
                File.AppendAllText(filePath, newAppointment.ToString()+"\n");

                // refresh data
                _patientService.RefreshPatientAppointments(this);

                // append new action in action file
                _patientService.AppendToActionFile(this._email, "create");

                // check number of changed, deleted and created _appointments
                this.AntiTrolMechanism();
            }
        }

        private void AntiTrolMechanism()
        {
            int changed = 0;
            int deleted = 0;
            int created = 0;

            List<UserAction> myCurrentActions = _patientService.LoadMyCurrentActions(this._email);

            foreach (UserAction action in myCurrentActions) {
                if (action.ActionState == UserAction.State.Created)
                    created += 1;
                else if (action.ActionState == UserAction.State.Modified)
                    changed += 1;
                else if (action.ActionState == UserAction.State.Deleted)
                    deleted += 1;
            }

            if (changed > 4)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste izmenili termin.\nPristup aplikaciji Vam je sada blokiran!");
            else if (deleted > 4)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste obrisali termin.\nPristup aplikaciji Vam je sada blokiran!");
            else if (created > 8)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste kreirali termin.\nPristup aplikaciji Vam je sada blokiran!");
            else
                return;
          
            _patientService.BlockAccessApplication(this._email);
            this.LogOut(); //log out from account
        }

        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
