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
        AppointmentService appointmentService = new AppointmentService();  // loading all appointments
        UserService userService = new UserService();
        Helper helper;
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;

        public Doctor(User currentRegisteredDoctor, Helper helper)
        {
            this.currentRegisteredDoctor = currentRegisteredDoctor;
            this.helper = helper;
            allMyAppointments = appointmentService.GetDoctorAppointment(currentRegisteredDoctor);

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
            } while (choice != "7");
        }

        private void examiningOwnSchedule()
        {
            string dateAppointment;
            do
            {
                Console.WriteLine("Unesite željeni datum: ");
                dateAppointment = Console.ReadLine();
            } while (!appointmentService.isDateFormValid(dateAppointment));

            this.readOwnAppointmentSpecificDate(DateTime.ParseExact(dateAppointment, "MM/dd/yyyy", CultureInfo.InvariantCulture));
        }


        private void readOwnAppointmentSpecificDate(DateTime dateSchedule)
        {
            DateTime dateForNextThreeDays = dateSchedule.AddDays(3);
            List<Appointment> appointmentsOfParticularDay = new List<Appointment>();
            Console.WriteLine("Raspored za datum od " + dateSchedule.Month + "/" + dateSchedule.Day + "/" + dateSchedule.Year + " do " + dateForNextThreeDays.Month + "/" + dateForNextThreeDays.Day + "/" + dateForNextThreeDays.Year);
            foreach (Appointment appointment in this.allMyAppointments)
            {
                if (appointment.DoctorEmail.Equals(currentRegisteredDoctor.Email) && (appointment.DateAppointment >= dateSchedule)&&(appointment.DateAppointment <= dateForNextThreeDays))
                {
                    appointmentsOfParticularDay.Add(appointment);
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
                foreach (Appointment appointment in  appointmentsOfParticularDay)
                {
                   
                    Console.WriteLine(appointment.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;
               
                }
                Console.WriteLine();

            }


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
                if ((appointment.GetAppointmentState == Appointment.AppointmentState.Created ||
                    appointment.GetAppointmentState == Appointment.AppointmentState.Updated) &&
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
                    } while (!appointmentService.isPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.isDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.isTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.isRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(15);
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.isAppointmentFreeForDoctor(currentRegisteredDoctor.Email,patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));


            }
            else if (typeOfTerm.Equals("2"))
            {
                do
                {
        
                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.isPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.isDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.isTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Koliko će trajati operacija (u minutima): ");
                        newDurationOperation = Console.ReadLine();

                    }while(!appointmentService.isIntegerValid(newDurationOperation));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.isRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.isAppointmentFreeForDoctor(currentRegisteredDoctor.Email, patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));
            }
            else
            {
                Console.WriteLine("Unesite validnu radnju!");
                return;
            }

            int id = helper.getNewAppointmentId();
            Appointment appointment = new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.AppointmentState.Created, roomNumber, (Appointment.TypeOfTerm)int.Parse(typeOfTerm));
            string newAppointment = "\n" + id + "," + patientEmail + "," + currentRegisteredDoctor.Email + ","  + newDate + "," +
                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.AppointmentState.Created + "," + newRoomNumber + "," + typeOfTerm;

            // append new appointment in file
            string filePath = @"..\..\Data\appointments.csv";
            File.AppendAllText(filePath, newAppointment);
            Console.WriteLine("Uspešno ste zakazali termin.");

            // add to appointments
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
                } while (!appointmentService.isIntegerValid(numberAppointment));
            } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);

            Appointment appointmentForDelete = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
            this.allMyAppointments.Remove(appointmentForDelete);
            appointmentService.Appointments.Remove(appointmentForDelete);
            appointmentService.deleteAppointment(appointmentForDelete);

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
                } while (!appointmentService.isIntegerValid(numberAppointment));
            } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);
            Appointment appointmentForUpdate = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
            if (appointmentForUpdate.GetTypeOfTerm == Appointment.TypeOfTerm.Examination)
            {
                do
                {

                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.isPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.isDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.isTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.isRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(15);
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.isAppointmentFreeForDoctor(currentRegisteredDoctor.Email, patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));

            }
            else
            {
                do
                {

                    do
                    {
                        Console.WriteLine("Unesite email pacijenta: ");
                        patientEmail = Console.ReadLine();
                    } while (!appointmentService.isPatientEmailValid(patientEmail));
                    do
                    {
                        Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                        newDate = Console.ReadLine();
                    } while (!appointmentService.isDateFormValid(newDate));
                    do
                    {
                        Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                        newStartTime = Console.ReadLine();
                    } while (!appointmentService.isTimeFormValid(newStartTime));
                    do
                    {
                        Console.WriteLine("Koliko će trajati operacija (u minutima): ");
                        newDurationOperation = Console.ReadLine();

                    } while (!appointmentService.isIntegerValid(newDurationOperation));
                    do
                    {
                        Console.WriteLine("Unesite broj sobe: ");
                        newRoomNumber = Console.ReadLine();
                    } while (!appointmentService.isRoomNumberValid(newRoomNumber));
                    dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
                    newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
                    roomNumber = Int32.Parse(newRoomNumber);
                } while (!appointmentService.isAppointmentFreeForDoctor(currentRegisteredDoctor.Email, patientEmail, dateOfAppointment, startTime, newEndTime, roomNumber));

            }
            appointmentService.Appointments.Remove(appointmentForUpdate);
            this.allMyAppointments.Remove(appointmentForUpdate);
            Appointment newAppointment = new Appointment(appointmentForUpdate.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.AppointmentState.Updated, roomNumber, appointmentForUpdate.GetTypeOfTerm);
            appointmentService.Appointments.Add(newAppointment);
            this.allMyAppointments.Add(newAppointment);
            appointmentService.updateAppointment(appointmentForUpdate);



        }
    }
}
