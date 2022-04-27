using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.PatientImplementation
{
    class Patient
    {
        string email;
        Helper helper;
        List<Appointment> allMyAppointments;

        public string Email { get { return email; } }
        public List<Appointment> PatientAppointments { get { return allMyAppointments; } }

        public Patient(string email, Helper helper)
        {
            this.email = email;
            this.helper = helper;
            this.allMyAppointments = helper.refreshPatientAppointments();
        }

        // methods
        public void patientMeni()
        {
            // meni
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

                // my choice
                if (choice.Equals("1"))
                    this.readOwnAppointments();
                else if (choice.Equals("2"))
                    this.createAppointment();
                else if (choice.Equals("3"))
                    this.updateAppointment();
                else if (choice.Equals("4"))
                    this.deleteAppointment();
            } while (choice != "5");

            
        }

        private void readOwnAppointments()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan pregled!");
                return;
            }
                
            int i = 1;
            Console.WriteLine("\n\tPREGLEDI");
            Console.Write("--------------------------\n");
            foreach (Appointment appointment in this.allMyAppointments)
            {
                // check if the appointment is scheduled and has not yet passed
                if ((appointment.GetAppointmentState == Appointment.AppointmentState.Created ||
                    appointment.GetAppointmentState == Appointment.AppointmentState.Modified) &&
                    appointment.DateAppointment > DateTime.Now)
                {
                    Console.WriteLine(i + ". " + appointment.ToString());
                    i++;
                }

            }
            Console.WriteLine();
        }

        private void deleteAppointment() 
        {
            // pick appointment for delete
            this.readOwnAppointments();
            string numberAppointment;
            do
            {
                Console.WriteLine("Unesite broj pregleda za brisanje");
                Console.Write(">> ");
                numberAppointment = Console.ReadLine();
            } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count && numberAppointment.Equals("0") 
            && numberAppointment.Contains("-"));

            Appointment appointmentForDelete = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];

            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForDelete.AppointmentId)) {

                    string deletionDate = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;

                    if ((DateTime.Now - appointmentForDelete.DateAppointment).TotalDays <= 2)
                    {
                        lines[i] = id + "," + fields[1] + "," + fields[2] + "," + deletionDate + "," + fields[4] + "," + fields[5] 
                            + "," + fields[6] + "," + (int)Appointment.AppointmentState.DeleteRequest;
                        Console.WriteLine("Zahtev za brisanje je poslat sekretaru!");
                    }
                    else
                    {
                        lines[i] = id + "," + fields[1] + "," + fields[2] + "," + deletionDate + "," + fields[4] + "," + fields[5] 
                            + "," + fields[6] + "," + (int)Appointment.AppointmentState.Deleted;
                        Console.WriteLine("Uspesno ste obrisali pregled!");
                    }
                }
            }

            // saving changes
            File.WriteAllLines(filePath, lines);

            //refresh data
            this.allMyAppointments = helper.refreshPatientAppointments();

            // check number of changed deleted and created appointments
            this.antiTrolMechanism();
        }

        private void updateAppointment()
        {
            // pick appointment for delete
            this.readOwnAppointments();
            string numberAppointment;
            do
            {
                Console.WriteLine("Unesite broj pregleda za izmenu");
                Console.Write(">> ");
                numberAppointment = Console.ReadLine();
            } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count && numberAppointment.Equals("0") && 
            numberAppointment.Contains("-"));

            Appointment appointmentForUpdate = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];

            // update
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
            } while (!helper.isValidInput(doctorEmail, newDate, newStartTime));

            if (helper.isAppointmentFree(doctorEmail, newDate, newStartTime))
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
                        string modificationDate = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                        DateTime startTime = DateTime.Parse(newStartTime);
                        DateTime newEndTime = startTime.AddMinutes(15);

                        if ((DateTime.Now - appointmentForUpdate.DateAppointment).TotalDays <= 2)
                        {
                            lines[i] = id + "," + fields[1] + "," + doctorEmail + "," + modificationDate + "," + newDate + "," +
                                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + 
                                (int)Appointment.AppointmentState.ChangeRequest;
                            Console.WriteLine("Zahtev za izmenu je poslat sekretaru!");
                        }
                        else
                        {
                            lines[i] = id + "," + fields[1] + "," + doctorEmail + "," + modificationDate + "," + newDate + "," + 
                                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + 
                                (int)Appointment.AppointmentState.Modified;
                            Console.WriteLine("Uspesno ste izvrsili izmenu pregleda!");
                        }
                    }
                }

                // saving changes
                File.WriteAllLines(filePath, lines);

                //refresh data
                this.allMyAppointments = helper.refreshPatientAppointments();

                // check number of changed deleted and created appointments
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
                Console.Write("\nUnesite email doktora: ");
                doctorEmail = Console.ReadLine();
                Console.Write("Unesite datum (MM/dd/yyyy): ");
                newDate = Console.ReadLine();
                Console.Write("Unesite vreme pocetka pregleda (HH:mm): ");
                newStartTime = Console.ReadLine();
            } while (!helper.isValidInput(doctorEmail, newDate, newStartTime));

            if (helper.isAppointmentFree(doctorEmail, newDate, newStartTime))
            {
                int id = helper.getNewAppointmentId();
                string schedulingDate = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                DateTime startTime = DateTime.Parse(newStartTime);
                DateTime newEndTime = startTime.AddMinutes(15);

                string newAppointment = "\n" + id + "," + this.email + "," + doctorEmail + "," + schedulingDate + "," + newDate + "," + 
                    newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.AppointmentState.Created;

                // append new appointment in file
                string filePath = @"..\..\Data\appointments.csv";
                File.AppendAllText(filePath, newAppointment);

                // refresh data
                this.allMyAppointments = helper.refreshPatientAppointments();

                // check number of changed deleted and created appointments
                this.antiTrolMechanism();
            }
        }

        private void antiTrolMechanism()
        {
            int changed = 0;
            int deleted = 0;
            int created = 0;

            foreach (Appointment appointment in allMyAppointments) {
                if ((DateTime.Now - appointment.DateAppointment).TotalDays <= 30) {
                    if (appointment.GetAppointmentState == Appointment.AppointmentState.Created)
                        created += 1;
                    else if (appointment.GetAppointmentState == Appointment.AppointmentState.Modified)
                        changed += 1;
                    else if (appointment.GetAppointmentState == Appointment.AppointmentState.Deleted)
                        deleted += 1;
                }
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
            System.Environment.Exit(0); //exit from application
        }
    }
}
