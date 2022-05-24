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
        AppointmentService appointmentService = new AppointmentService();
        UserService userService = new UserService();
        ReferralService referralService = new ReferralService();
        HealthRecordService healthRecordService = new HealthRecordService();
        List<HealthRecord> healthRecords;
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;
        
        public Doctor(User currentRegisteredDoctor)
        {
            this.currentRegisteredDoctor = currentRegisteredDoctor;
            allMyAppointments = appointmentService.GetDoctorAppointment(currentRegisteredDoctor);
            healthRecords = healthRecordService.HealthRecords;

        }
        public void DoctorMenu()
        {
            string choice;
            do
            {
                PrintDoctorMenu();
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                {
                    this.ReadOwnAppointment();
                }
                else if (choice.Equals("2"))
                {
                    this.CreateOwnAppointment();
                }
                else if (choice.Equals("3"))
                {
                    this.UpdateOwnAppointment();
                }
                else if (choice.Equals("4"))
                {
                    this.DeleteOwnAppointment();
                }
                else if (choice.Equals("5"))
                {
                    this.ExaminingOwnSchedule();
                }
                else if (choice.Equals("6"))
                    {
                    this.PerformingAppointment();
                }
                else if (choice.Equals("7"))
                {
                    this.DrugManagement();
                }
                else if (choice.Equals("8"))
                {
                    this.LogOut();
                }
                else
                {
                    Console.WriteLine("Unesite validnu radnju!");
                }
            } while (true);
        }

        private void PrintDoctorMenu()
        {
            Console.WriteLine("\n\tMENI");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Pregledaj sopstvene preglede/operacije");
            Console.WriteLine("2. Kreiraj sopstveni pregled/operaciju");
            Console.WriteLine("3. Izmeni sopstveni pregled/operaciju");
            Console.WriteLine("4. Obrisi sopstveni pregled/operaciju");
            Console.WriteLine("5. Ispitivanje sopstvenog rasporeda");
            Console.WriteLine("6. Izvodjenje pregleda");
            Console.WriteLine("7. Upravljanje lekovima");
            Console.WriteLine("8. Odjava");
            Console.Write(">> ");

        }

        private void DrugManagement()
        {
            DrugVerification drugVerification = new DrugVerification();
            drugVerification.DisplayDrugsForVerification();
        }
        private void PerformingAppointment()
        {
            DoctorPerformingAppointment doctorPerforming = new DoctorPerformingAppointment(appointmentService, healthRecords, healthRecordService,currentRegisteredDoctor,allMyAppointments,userService);
            doctorPerforming.PerformingAppointment();
        }
 
        private void ExaminingOwnSchedule()
        {
            string dateAppointment;
            do
            {
                Console.WriteLine("Unesite željeni datum: ");
                dateAppointment = Console.ReadLine();
            } while (!appointmentService.IsDateFormValid(dateAppointment));
            DoctorSchedule doctorSchedule = new DoctorSchedule(appointmentService, healthRecords, allMyAppointments, currentRegisteredDoctor);
            doctorSchedule.ReadOwnAppointmentSpecificDate(DateTime.ParseExact(dateAppointment, "MM/dd/yyyy", CultureInfo.InvariantCulture));

        }


        private void ReadOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
            Console.WriteLine("\n\tPREGLEDI I OPERACIJE\n");
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.", "Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
            int serialNumberAppointment = 1;
            foreach (Appointment appointment in this.allMyAppointments)
            {
                if (CheckOwnAppointment(appointment))
                {
                    Console.WriteLine(appointment.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;
                }
            }
        }

        private bool CheckOwnAppointment(Appointment appointment)
        {
            if ((appointment.AppointmentState == Appointment.State.Created ||
                    appointment.AppointmentState == Appointment.State.Updated) &&
                    appointment.DateAppointment > DateTime.Now)
            {
                return true;

            }
            return false;
        }

        public void CreateOwnAppointment()
        { 
            string typeOfTerm;
            Appointment newAppointment;
            Console.WriteLine("Šta želite da kreirate: ");
            Console.WriteLine("1. Pregled");
            Console.WriteLine("2. Operaciju");
            Console.WriteLine(">> ");
            typeOfTerm = Console.ReadLine();
            if (typeOfTerm.Equals("1"))
            {
                do{
                    newAppointment = PrintItemsToEnterExamination(typeOfTerm);  
                } while (!appointmentService.IsAppointmentFreeForDoctor(newAppointment));

            }
            else if (typeOfTerm.Equals("2"))
            {
                do{    
                    newAppointment = PrintItemsToEnterOperation(typeOfTerm);
                } while (!appointmentService.IsAppointmentFreeForDoctor(newAppointment));
            }
            else{
                Console.WriteLine("Unesite validnu radnju!");
                return;
            }
            appointmentService.Appointments.Add(newAppointment);
            appointmentService.UpdateFile();
            this.allMyAppointments = appointmentService.GetDoctorAppointment(this.currentRegisteredDoctor);
            Console.WriteLine("Uspešno ste zakazali termin.");
        }

        private Appointment PrintItemsToEnterExamination(string typeOfTerm)
        {
            string patientEmail, newDate, newStartTime, newRoomNumber;
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
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
            int id = appointmentService.GetNewAppointmentId();
            return new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm), false, false);
        }

        private Appointment PrintItemsToEnterOperation(string typeOfTerm)
        {
            int temp;
            string patientEmail, newDate, newStartTime, newRoomNumber, newDurationOperation;
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
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

            } while (!int.TryParse(newDurationOperation, out temp));
            do
            {
                Console.WriteLine("Unesite broj sobe: ");
                newRoomNumber = Console.ReadLine();
            } while (!appointmentService.IsRoomNumberValid(newRoomNumber));
            dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
            newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
            roomNumber = Int32.Parse(newRoomNumber);
            int id = appointmentService.GetNewAppointmentId();
            return new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm), false, false);
        }


        private Appointment PrintItemsToChangeOperation(Appointment appontment)
        {
            int temp;
            string patientEmail, newDate, newStartTime, newRoomNumber, newDurationOperation;
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
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

            } while (!int.TryParse(newDurationOperation, out temp));
            do
            {
                Console.WriteLine("Unesite broj sobe: ");
                newRoomNumber = Console.ReadLine();
            } while (!appointmentService.IsRoomNumberValid(newRoomNumber));
            dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
            newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
            roomNumber = Int32.Parse(newRoomNumber);
            return new Appointment(appontment.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Updated, roomNumber, appontment.TypeOfTerm, false, appontment.Urgent);
        }

        private Appointment PrintItemsToChangeExamination(Appointment appointment)
        {
            string patientEmail, newDate, newStartTime, newRoomNumber;
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
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
            return new Appointment(appointment.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Updated, roomNumber, appointment.TypeOfTerm, false, appointment.Urgent);
        }


        private void DeleteOwnAppointment()
        {
            int temp;
            if (this.allMyAppointments.Count != 0)
            {
                this.ReadOwnAppointment();
                string numberAppointment;
                do
                {
                    do
                    {
                        Console.WriteLine("Unesite redni broj koji želite da obrišete: ");
                        numberAppointment = Console.ReadLine();
                    } while (!int.TryParse(numberAppointment, out temp));
                } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);

                Appointment appointmentForDelete = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
                appointmentForDelete.AppointmentState = Appointment.State.Deleted;
                appointmentService.UpdateAppointment(appointmentForDelete);
                appointmentService.UpdateFile();
                this.allMyAppointments = appointmentService.GetDoctorAppointment(this.currentRegisteredDoctor);
                Console.WriteLine("Uspesno ste obrisali termin!");
            }
            else
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
            }
            
           
        }

        private void UpdateOwnAppointment()
        {
            int tryInt;
            if (this.allMyAppointments.Count != 0)
            {
                this.ReadOwnAppointment();
                string numberAppointment;
                do
                {
                    do
                    {
                        Console.WriteLine("Unesite redni broj koji želite da promenite: ");
                        numberAppointment = Console.ReadLine();
                    } while (!int.TryParse(numberAppointment, out tryInt));
                } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);
                Appointment appointmentForUpdate = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
                Appointment newAppointment;
                if (appointmentForUpdate.TypeOfTerm == Appointment.Type.Examination)
                {
                    newAppointment = this.PrintItemsToChangeExamination(appointmentForUpdate);
                }
                else
                {
                    newAppointment = this.PrintItemsToChangeOperation(appointmentForUpdate);
                }
                
                appointmentService.UpdateAppointment(newAppointment);
                appointmentService.UpdateFile();
                this.allMyAppointments = appointmentService.GetDoctorAppointment(this.currentRegisteredDoctor);
                Console.WriteLine("Uspesno ste izmenili termin!");
            }
            else
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
            }
        }
        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
