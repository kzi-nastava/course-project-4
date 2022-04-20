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
        List<Appointment> myAppointments;
        HelperClass helper;

        public string Email { get { return email; } }
        public List<Appointment> PatientAppointments { get { return myAppointments; } }

        public Patient(string email, List<Appointment> myAppointments, HelperClass helper)
        {
            this.email = email;
            this.myAppointments = myAppointments;
            this.helper = helper;
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
                else if (choice.Equals("3"))
                    this.updateAppointment();
                else if (choice.Equals("4"))
                    this.deleteAppointment();
            } while (choice != "5");

            
        }

        private void readOwnAppointments()
        {
            int i = 1;
            Console.WriteLine("\n\tPREGLEDI");
            Console.Write("--------------------------\n");
            foreach (Appointment appointment in this.myAppointments)
            {
                // check if the appointment is scheduled and has not yet passed
                if (appointment.GetAppointmentState == Appointment.AppointmentState.Created ||
                    appointment.GetAppointmentState == Appointment.AppointmentState.Modified)
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
            } while (Int32.Parse(numberAppointment) > this.myAppointments.Count && numberAppointment.Equals("0") && numberAppointment.Contains("-"));

            Appointment appointmentForDelete = this.myAppointments[Int32.Parse(numberAppointment) - 1];

            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForDelete.AppointmentId)) {
                    if ((DateTime.Now - appointmentForDelete.DateExamination).TotalDays <= 2)
                    {
                        lines[i] = id + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," +
                            fields[5] + "," + (int)Appointment.AppointmentState.DeleteRequest;
                        Console.WriteLine("Zahtev za brisanje je poslat sekretaru!");
                    }
                    else
                    {
                        lines[i] = id + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," +
                            fields[5] + "," + (int)Appointment.AppointmentState.Deleted;
                        Console.WriteLine("Uspesno ste obrisali pregled!");
                    }
                }
            }

            // saving changes
            File.WriteAllLines(filePath, lines);

            //refresh data
            this.myAppointments = helper.refreshPatientAppointments();
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
            } while (Int32.Parse(numberAppointment) > this.myAppointments.Count && numberAppointment.Equals("0") && numberAppointment.Contains("-"));

            Appointment appointmentForUpdate = this.myAppointments[Int32.Parse(numberAppointment) - 1];

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
                        DateTime startTime = DateTime.Parse(newStartTime);
                        DateTime newEndTime = startTime.AddMinutes(15);

                        if ((DateTime.Now - appointmentForUpdate.DateExamination).TotalDays <= 2)
                        {
                            lines[i] = id + "," + fields[1] + "," + doctorEmail + "," + newDate + "," + newStartTime + "," +
                                newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.AppointmentState.ChangeRequest;
                            Console.WriteLine("Zahtev za izmenu je poslat sekretaru!");
                        }
                        else
                        {
                            lines[i] = id + "," + fields[1] + "," + doctorEmail + "," + newDate + "," + newStartTime + "," +
                                newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.AppointmentState.Modified;
                            Console.WriteLine("Uspesno ste izvrsili izmenu pregleda!");
                        }
                    }
                }

                // saving changes
                File.WriteAllLines(filePath, lines);

                //refresh data
                this.myAppointments = helper.refreshPatientAppointments();
            }

        }
    }
}
