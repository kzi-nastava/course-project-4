using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.PatientImplementation;
using Hospital.Model;
using Hospital.Service;
using System.Globalization;
using System.IO;

namespace Hospital.DoctorImplementation
{
    class Doctor
    {
        AppointmentService appointmentService = new AppointmentService();  // loading all _appointments
        UserService userService = new UserService();
        PatientService helper;
        HealthRecordService healthRecordService = new HealthRecordService();
        List<HealthRecord> healthRecords;
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;

        public Doctor(User currentRegisteredDoctor, PatientService helper)
        {
            this.currentRegisteredDoctor = currentRegisteredDoctor;
            this.helper = helper;
            allMyAppointments = appointmentService.GetDoctorAppointment(currentRegisteredDoctor);
            healthRecords = healthRecordService.HealthRecords;

        }
        public void doctorMenu()
        {
            // meni
            string choice;
            Console.WriteLine("\n\tMENI");
            Console.WriteLine("----------------------------------------");
            do
            {
                Console.WriteLine("1. Pregledaj sopstvene preglede/operacije");
                Console.WriteLine("2. Kreiraj sopstveni pregled/operaciju");
                Console.WriteLine("3. Izmeni sopstveni pregled/operaciju");
                Console.WriteLine("4. Obrisi sopstveni pregled/operaciju");
                Console.WriteLine("5. Ispitivanje sopstvenog rasporeda");
                Console.WriteLine("6. Izvodjenje pregleda");

                Console.WriteLine("7. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                // my choice
                if (choice.Equals("1"))
                {
                    Console.WriteLine("\n1.Pregledaj sopstvene preglede/operacije");
                    this.readOwnAppointment();
                }
                else if (choice.Equals("2"))
                {
                    Console.WriteLine("\n2.Kreiraj sopstveni pregled/operaciju");
                    this.createOwnAppointment();
                }
                else if (choice.Equals("3"))
                {
                    Console.WriteLine("\n3.Izmeni sopstveni pregled/operaciju");
                    this.updateOwnAppointment();
                }
                else if (choice.Equals("4"))
                {
                    Console.WriteLine("\n4. Obrisi sopstveni pregled/operaciju");
                    this.deleteOwnAppointment();
                }
                else if (choice.Equals("5"))
                {
                    Console.WriteLine("5. Ispitivanje sopstvenog rasporeda");
                    this.examiningOwnSchedule();
                }
                else if (choice.Equals("6"))
                    Console.WriteLine("6. Izvodjenje pregleda");
                else if (choice.Equals("7"))
                {
                    this.logOut();
                }
            } while (true);
        }

        private void examiningOwnSchedule()
        {
            string dateAppointment;
            do
            {
                Console.WriteLine("Unesite željeni datum: ");
                dateAppointment = Console.ReadLine();
            } while (!appointmentService.IsDateFormValid(dateAppointment));

            this.readOwnAppointmentSpecificDate(DateTime.ParseExact(dateAppointment, "MM/dd/yyyy", CultureInfo.InvariantCulture));

        }




        private void readOwnAppointmentSpecificDate(DateTime dateSchedule)
        {
            DateTime dateForNextThreeDays = dateSchedule.AddDays(3);
            List<Appointment> appointmentsOfParticularDay = new List<Appointment>();
            Console.WriteLine("Raspored za datum od " + dateSchedule.Month + "/" + dateSchedule.Day + "/" + dateSchedule.Year + " do " + dateForNextThreeDays.Month + "/" + dateForNextThreeDays.Day + "/" + dateForNextThreeDays.Year);
            foreach (Appointment appointmentOwn in this.allMyAppointments)
            {
                if (appointmentOwn.DoctorEmail.Equals(currentRegisteredDoctor.Email) && (appointmentOwn.DateAppointment >= dateSchedule)&&(appointmentOwn.DateAppointment <= dateForNextThreeDays))
                {
                    appointmentsOfParticularDay.Add(appointmentOwn);
                }
                
            }
            if (appointmentsOfParticularDay.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate termine za odredjeni datum!");
                return;
            }
            else
            {
                int serialNumberAppointment = 1;
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.", "Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
                foreach (Appointment appointmentOwn in  appointmentsOfParticularDay)
                {
                   
                    Console.WriteLine(appointmentOwn.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;
               
                }
                Console.WriteLine();

            }
            string choice;
            Appointment appointmentOfSelectedPatient;
            string serialNumberOfMedicalRecord;
            do
            {
                
                Console.WriteLine("Da li želite da pristupite zdravstvenom kartonu od nekog pacijenta? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    do
                    {
                        do
                        {
                            Console.WriteLine("Unesite redni broj pacijenta čiji zdravstveni karton želite da pogledate: ");
                            serialNumberOfMedicalRecord = Console.ReadLine();
                            
                        } while (!appointmentService.IsIntegerValid(serialNumberOfMedicalRecord));
                    } while (Int32.Parse(serialNumberOfMedicalRecord) > appointmentsOfParticularDay.Count);
                    appointmentOfSelectedPatient = appointmentsOfParticularDay[Int32.Parse(serialNumberOfMedicalRecord) - 1];
                    foreach (HealthRecord healthRecord in this.healthRecords)
                    {
                        if (healthRecord.EmailPatient.Equals(appointmentOfSelectedPatient.PatientEmail))
                        {
                            Console.WriteLine(healthRecord.ToString());

                        }
                    }
                    Console.WriteLine();
                }
                else if (choice.Equals("2"))
                {
                    return;
                }
            } while (!choice.Equals("1") && !choice.Equals("2"));
            



        }

        private void readOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }

            Console.WriteLine("\n\tPREGLEDI I OPERACIJE\n");
            int serialNumberAppointment = 1;
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.","Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
            foreach (Appointment appointment in this.allMyAppointments)
            {
                if ((appointment.AppointmentState == Appointment.State.Created ||
                    appointment.AppointmentState == Appointment.State.Updated) &&
                    appointment.DateAppointment > DateTime.Now)
                {
                    Console.WriteLine(appointment.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;
                }
                


            }
            Console.WriteLine();
            
        }

        private void createOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
            string patientEmail;
            string newDate;
            string newStartTime;
            string newRoomNumber;
            string typeOfTerm;
            string newDurationOperation;
            DateTime dateOfAppointment;
            DateTime startTime;
            DateTime newEndTime;
            int roomNumber;
            Console.WriteLine("Šta želite da kreirate: ");
            Console.WriteLine("1. Pregled");
            Console.WriteLine("2. Operaciju");
            Console.WriteLine(">> ");
            typeOfTerm = Console.ReadLine();
            if (typeOfTerm.Equals("1"))
            {
                do
                {
                    
                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.IsPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.IsDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.IsTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.IsRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(15);
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.IsAppointmentFreeForDoctor(currentRegisteredDoctor.Email,patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));


            }
            else if (typeOfTerm.Equals("2"))
            {
                do
                {
        
                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.IsPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.IsDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.IsTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Koliko će trajati operacija (u minutima): ");
                        newDurationOperation = Console.ReadLine();

                    }while(!appointmentService.IsIntegerValid(newDurationOperation));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.IsRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.IsAppointmentFreeForDoctor(currentRegisteredDoctor.Email, patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));
            }
            else
            {
                Console.WriteLine("Unesite validnu radnju!");
                return;
            }

            int id = helper.GetNewAppointmentId();
            Appointment appointment = new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm));
            string newAppointment = "\n" + id + "," + patientEmail + "," + currentRegisteredDoctor.Email + ","  + newDate + "," +
                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.State.Created + "," + newRoomNumber + "," + typeOfTerm;

            // append new appointment in file
            string filePath = @"..\..\Data\_appointments.csv";
            File.AppendAllText(filePath, newAppointment);
            Console.WriteLine("Uspešno ste zakazali termin.");

            // add to _appointments
            appointmentService.Appointments.Add(appointment);
            allMyAppointments.Add(appointment);
            




        }

        private void deleteOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
            this.readOwnAppointment();
            string numberAppointment;
            do
            {
                do
                {
                    Console.WriteLine("Unesite redni broj koji želite da obrišete: ");
                    numberAppointment = Console.ReadLine();
                } while (!appointmentService.IsIntegerValid(numberAppointment));
            } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);

            Appointment appointmentForDelete = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
            this.allMyAppointments.Remove(appointmentForDelete);
            appointmentService.Appointments.Remove(appointmentForDelete);
            appointmentService.DeleteAppointment(appointmentForDelete);

        }

        private void updateOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
            this.readOwnAppointment();
            string numberAppointment;
            string patientEmail;
            string newDate;
            string newStartTime;
            string newRoomNumber;
            string newDurationOperation;
            DateTime dateOfAppointment;
            DateTime startTime;
            DateTime newEndTime;
            int roomNumber;
            do
            {
                do
                {
                    Console.WriteLine("Unesite redni broj koji želite da promenite: ");
                    numberAppointment = Console.ReadLine();
                } while (!appointmentService.IsIntegerValid(numberAppointment));
            } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);
            Appointment appointmentForUpdate = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
            if (appointmentForUpdate.TypeOfTerm == Appointment.Type.Examination)
            {
                do
                {

                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.IsPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.IsDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.IsTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.IsRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(15);
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.IsAppointmentFreeForDoctor(currentRegisteredDoctor.Email, patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));

            }
            else
            {
                do
                {

                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.IsPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.IsDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.IsTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Koliko će trajati operacija (u minutima): ");
                        newDurationOperation = Console.ReadLine();

                    } while (!appointmentService.IsIntegerValid(newDurationOperation));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.IsRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.IsAppointmentFreeForDoctor(currentRegisteredDoctor.Email, patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));

            }
            appointmentService.Appointments.Remove(appointmentForUpdate);
            this.allMyAppointments.Remove(appointmentForUpdate);
            Appointment newAppointment = new Appointment(appointmentForUpdate.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Updated, roomNumber, appointmentForUpdate.TypeOfTerm);
            appointmentService.Appointments.Add(newAppointment);
            this.allMyAppointments.Add(newAppointment);
            appointmentService.UpdateAppointment(appointmentForUpdate);



        }
        private void logOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
