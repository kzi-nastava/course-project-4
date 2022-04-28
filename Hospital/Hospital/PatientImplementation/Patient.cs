using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using System.IO;
using System.Globalization;

namespace Hospital.PatientImplementation
{
    class Patient
    {
        string email;
        PatientService helper;
        List<Appointment> currentAppointments; 

        public string Email { get { return email; } }
        public List<Appointment> PatientAppointments
        {
            get { return currentAppointments; }
            set { currentAppointments = value; }
        } 

        public Patient(string email, PatientService helper)
        {
            this.email = email;
            this.helper = helper;
            helper.refreshPatientAppointments(this);
        }

        // methods
        public void patientMenu()
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
                    this.readOwnAppointments();
                else if (choice.Equals("2"))
                    this.createAppointment();
                else if (choice.Equals("3"))
                    this.updateAppointment();
                else if (choice.Equals("4"))
                    this.deleteAppointment();
                else if (choice.Equals("5"))
                    this.logOut();
            } while (true);
        }

        private void readOwnAppointments()
        {
            if (!helper.hasPatientAppointment(this))
                return;
                
            int i = 0;
            Console.WriteLine("\n\tPREGLEDI");
            Console.Write("--------------------------\n");
            
            foreach (Appointment appointment in this.currentAppointments)
            {
                i++;
                Console.WriteLine(i + ". " + appointment.ToString());
            }

            Console.WriteLine();
        }

        private void deleteAppointment() 
        {
            // first check if patient has appointments for delete
            if (!helper.hasPatientAppointment(this))
                return;

            // pick appointment for delete
            List<Appointment> appointmentsForDelete = helper.findAppointmentsForDeleteAndUpdate(this, "delete");
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

            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForDelete.AppointmentId)) {

                    if ((appointmentForDelete.DateAppointment - DateTime.Now).TotalDays <= 2)
                    {
                        lines[i] = id + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] 
                            + "," + fields[5] + "," + (int)Appointment.AppointmentState.DeleteRequest;
                        Console.WriteLine("Zahtev za brisanje je poslat sekretaru!");
                    }
                    else
                    {
                        lines[i] = id + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4]
                            + "," + fields[5] + "," + (int)Appointment.AppointmentState.Deleted;
                        Console.WriteLine("Uspesno ste obrisali pregled!");
                    }
                }
            }

            // saving changes
            File.WriteAllLines(filePath, lines);

            //refresh data
            helper.refreshPatientAppointments(this);

            // append new action in action file
            helper.appendToActionFile(this.email, "brisanje");

            // check number of changed, deleted and created appointments
            this.antiTrolMechanism();
        }

        private void updateAppointment()
        {
            // first check if patient has appointments for update
            if (!helper.hasPatientAppointment(this))
                return;

            // pick appointment for update
            List<Appointment> appointmentsForUpdate = helper.findAppointmentsForDeleteAndUpdate(this, "update");
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
            } while (!helper.isValidInput(doctorEmail, newDate, newStartTime));

            if (helper.isAppointmentFree(this.email, doctorEmail, newDate, newStartTime))
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
                        DateTime startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                        DateTime newEndTime = startTime.AddMinutes(15);

                        if ((appointmentForUpdate.DateAppointment - DateTime.Now).TotalDays <= 2)
                        {
                            lines[i] = id + "," + fields[1] + "," + doctorEmail + "," + newDate + "," +
                                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + 
                                (int)Appointment.AppointmentState.UpdateRequest;
                            Console.WriteLine("Zahtev za izmenu je poslat sekretaru!");
                        }
                        else
                        {
                            lines[i] = id + "," + fields[1] + "," + doctorEmail + "," + newDate + "," + 
                                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + 
                                (int)Appointment.AppointmentState.Updated;
                            Console.WriteLine("Uspesno ste izvrsili izmenu pregleda!");
                        }
                    }
                }

                // saving changes
                File.WriteAllLines(filePath, lines);

                //refresh data
                helper.refreshPatientAppointments(this);

                // append new action in action file
                helper.appendToActionFile(this.email, "izmena");

                // check number of changed, deleted and created appointments
                this.antiTrolMechanism();
            }

        }

        private void createAppointment()
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
            } while (!helper.isValidInput(doctorEmail, newDate, newStartTime));

            if (helper.isAppointmentFree(this.email, doctorEmail, newDate, newStartTime))
            {
                int id = helper.getNewAppointmentId();
                string schedulingDate = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                DateTime startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                DateTime newEndTime = startTime.AddMinutes(15);

                string newAppointment = "\n" + id + "," + this.email + "," + doctorEmail + "," + schedulingDate + "," + newDate + "," + 
                    newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.AppointmentState.Created;

                // append new appointment in file
                string filePath = @"..\..\Data\appointments.csv";
                File.AppendAllText(filePath, newAppointment);

                // refresh data
                helper.refreshPatientAppointments(this);

                // append new action in action file
                helper.appendToActionFile(this.email, "kreiranje");

                // check number of changed, deleted and created appointments
                this.antiTrolMechanism();
            }
        }

        private void antiTrolMechanism()
        {
            int changed = 0;
            int deleted = 0;
            int created = 0;

            List<UserAction> myCurrentActions = helper.loadMyCurrentActions(this.email);

            foreach (UserAction action in myCurrentActions) {
                if (action.GetActionState == UserAction.ActionState.Created)
                    created += 1;
                else if (action.GetActionState == UserAction.ActionState.Modified)
                    changed += 1;
                else if (action.GetActionState == UserAction.ActionState.Deleted)
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
          
            helper.blockAccessApplication(this.email);
            this.logOut(); //log out from account
        }

        private void logOut()
        {
            Login loging = new Login();
            loging.logIn();
        }
    }
}
