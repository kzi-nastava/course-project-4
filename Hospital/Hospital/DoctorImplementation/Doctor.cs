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
        PatientService helper;
        HealthRecordService healthRecordService = new HealthRecordService();
        List<HealthRecord> healthRecords;
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;
        MedicalRecordService medicalRecordService = new MedicalRecordService();
        

        public Doctor(User currentRegisteredDoctor, PatientService helper)
        {
            this.currentRegisteredDoctor = currentRegisteredDoctor;
            this.helper = helper;
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
            Console.WriteLine("7. Odjava");
            Console.Write(">> ");

        }
        private void PerformingAppointment()
        {
            List<Appointment> appointmentsForPerformanse = TodaysAppointments();
            if (appointmentsForPerformanse.Count != 0)
            {
                this.PrintTodaysAppointments(appointmentsForPerformanse);
                string choice;
                do
                {
                    do
                    {
                        Console.WriteLine("Unesite redni broj pacijenta kojeg želite da pregledate: ");
                        choice = Console.ReadLine();

                    } while (!appointmentService.IsIntegerValid(choice));
                } while (Int32.Parse(choice) > appointmentsForPerformanse.Count);
                Appointment appointmentOfSelected = appointmentsForPerformanse[Int32.Parse(choice) - 1];

                DisplayOfPatientData(appointmentOfSelected.PatientEmail);
                DisplayOfPatientsHealthRecord(appointmentOfSelected.PatientEmail);
                EnteringAnamnesis(appointmentOfSelected);
                return;
            }
            else
            {
                Console.WriteLine("Svi pregledi su obavljeni!");
            }
            
        }

        private void EnteringAnamnesis(Appointment appointment)
        {
            string anamnesisInput;
            Console.WriteLine("Unesite anamnezu: ");
            anamnesisInput = Console.ReadLine();

            //the examination was performed
            appointmentService.PerformAppointment(appointment);
            appointmentService.UpdateFile();

            //updating medical record
            MedicalRecord newMedicalRecord = new MedicalRecord(appointment.AppointmentId, anamnesisInput, " ");
            medicalRecordService.MedicalRecords.Add(newMedicalRecord);
            medicalRecordService.UpdateFile();
            Console.WriteLine("Uspešno ste uneli anamnezu."); 

            //updating health record
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                Console.WriteLine(healthRecord);
                if (healthRecord.EmailPatient.Equals(appointment.PatientEmail))
                {
                    this.UpdatingSelectedHealthRecord(healthRecord);

                }
            }


        }

        private void DisplayOfPatientData(string patientEmail)
        {
            foreach (User user in userService.Users)
            {
                if (user.Email.Equals(patientEmail))
                {
                    Console.WriteLine("Ime: " + user.Name + "\n" + "Prezime: " + user.Surname + "\n");

                }
            }

        }

        private void DisplayOfPatientsHealthRecord(string patientEmail)
        {
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(patientEmail))
                {
                    Console.WriteLine(healthRecord.ToString());
                    Console.WriteLine("Ponovo");
                }
            }
        }

        private void PrintTodaysAppointments(List<Appointment> appointmentsForPerformanse)
        {
            int serialNumberAppointment = 1;
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.", "Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
            foreach (Appointment appointment in appointmentsForPerformanse)
            {
                Console.WriteLine(appointment.ToStringDisplayForDoctor(serialNumberAppointment));
                serialNumberAppointment += 1;
            }
            Console.WriteLine();
        }

        private List<Appointment> TodaysAppointments()
        {
            string dateNow = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime parseDateNow = DateTime.ParseExact(dateNow, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            List<Appointment> appointmentsForPerformanse = new List<Appointment>();
            foreach (Appointment appointmentOwn in this.allMyAppointments)
            {
                if (appointmentOwn.DoctorEmail.Equals(currentRegisteredDoctor.Email) && (appointmentOwn.DateAppointment == parseDateNow)
                    && (appointmentOwn.AppointmentState != Appointment.State.Deleted) && (appointmentOwn.AppointmentPerformed == false))
                {
                    appointmentsForPerformanse.Add(appointmentOwn);
                }

            }
            return appointmentsForPerformanse;

        }


        private void UpdatingSelectedHealthRecord(HealthRecord healthRecordSelected)
        {
            string selectionOfUpdates;
            do
            {

                Console.WriteLine("Da li želite da ažurirate zdravstveni karton pacijenta? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                selectionOfUpdates = Console.ReadLine();
                if (selectionOfUpdates.Equals("1"))
                {
                    this.PrintItemsToChangeHealthRecord(healthRecordSelected);
                }
                else if (selectionOfUpdates.Equals("2"))
                {
                    return;
                }
            } while (!selectionOfUpdates.Equals("1") && !selectionOfUpdates.Equals("2"));


        }

        private void PrintItemsToChangeHealthRecord(HealthRecord healthRecordSelected)
        {
            string patientHeightInput, patientWeightInput, previousIllnessesInput, allergenInput, bloodTypeInput;
            do
            {
                Console.WriteLine("Unesite visinu: ");
                patientHeightInput = Console.ReadLine();

            } while (!appointmentService.IsIntegerValid(patientHeightInput));
            do
            {
                Console.WriteLine("Unesite težinu: ");
                patientWeightInput = Console.ReadLine();
            } while (!appointmentService.IsDoubleValid(patientWeightInput));
            Console.WriteLine("Unesite prethodne bolesti: ");
            previousIllnessesInput = Console.ReadLine();
            Console.WriteLine("Unesite alergene: ");
            allergenInput = Console.ReadLine();
            Console.WriteLine("Unesite krvnu grupu: ");
            bloodTypeInput = Console.ReadLine();
            HealthRecord newHealthRecord = new HealthRecord(healthRecordSelected.IdHealthRecord, healthRecordSelected.EmailPatient, Int32.Parse(patientHeightInput), double.Parse(patientWeightInput), previousIllnessesInput, allergenInput, bloodTypeInput);
            healthRecordService.UpdateHealthRecord(newHealthRecord);
            healthRecordService.UpdateHealthRecordFile();
        }


        private void ExaminingOwnSchedule()
        {
            string dateAppointment;
            do
            {
                Console.WriteLine("Unesite željeni datum: ");
                dateAppointment = Console.ReadLine();
            } while (!appointmentService.IsDateFormValid(dateAppointment));
            this.ReadOwnAppointmentSpecificDate(DateTime.ParseExact(dateAppointment, "MM/dd/yyyy", CultureInfo.InvariantCulture));

        }




        private void ReadOwnAppointmentSpecificDate(DateTime dateSchedule)
        {
            DateTime dateForNextThreeDays = dateSchedule.AddDays(3);
            List<Appointment> appointmentsOfParticularDay = AppointmentForSelectedDate(dateSchedule,dateForNextThreeDays);
            Console.WriteLine("Raspored za datum od " + dateSchedule.Month + "/" + dateSchedule.Day + "/" + dateSchedule.Year + " do " + dateForNextThreeDays.Month + "/" + dateForNextThreeDays.Day + "/" + dateForNextThreeDays.Year);    
            if (appointmentsOfParticularDay.Count != 0)
            {
                int serialNumberAppointment = 1;
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.", "Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
                foreach (Appointment appointmentOwn in  appointmentsOfParticularDay)
                {
                   
                    Console.WriteLine(appointmentOwn.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;
               
                }
            }
            else
            {
                Console.WriteLine("Nemate zakazanih termina za izabrani datum");
            }
            this.AccessToHealthRecord(appointmentsOfParticularDay); 
        }

        private void AccessToHealthRecord(List<Appointment> appointmentsOfParticularDay)
        {
            string choice, serialNumberOfHealthRecord;
            Appointment appointmentOfSelectedPatient;
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
                            serialNumberOfHealthRecord = Console.ReadLine();

                        } while (!appointmentService.IsIntegerValid(serialNumberOfHealthRecord));
                    } while (Int32.Parse(serialNumberOfHealthRecord) > appointmentsOfParticularDay.Count);
                    appointmentOfSelectedPatient = appointmentsOfParticularDay[Int32.Parse(serialNumberOfHealthRecord) - 1];
                    this.PrintPatientsHealthRecord(appointmentOfSelectedPatient.PatientEmail);
                }
                else if (choice.Equals("2"))
                {
                    return;
                }
            } while (!choice.Equals("1") && !choice.Equals("2"));

        }

        private void PrintPatientsHealthRecord(String patientEmail)
        {
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(patientEmail))
                {
                    Console.WriteLine(healthRecord.ToString());
                }
            }

        }

        private List<Appointment> AppointmentForSelectedDate(DateTime selectedDate, DateTime dateForNextThreeDays)
        {
            List<Appointment> appointmentsOfParticularDay = new List<Appointment>();
            foreach (Appointment appointmentOwn in this.allMyAppointments)
            {
                if (appointmentOwn.DoctorEmail.Equals(currentRegisteredDoctor.Email) && (appointmentOwn.DateAppointment >= selectedDate) && (appointmentOwn.DateAppointment <= dateForNextThreeDays))
                {
                    appointmentsOfParticularDay.Add(appointmentOwn);
                }
            }
            return appointmentsOfParticularDay;
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

        private void CreateOwnAppointment()
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
            return new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm), false);
        }

        private Appointment PrintItemsToEnterOperation(string typeOfTerm)
        {
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
            int id = appointmentService.GetNewAppointmentId();
            return new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm), false);
        }


        private Appointment PrintItemsToChangeOperation(Appointment appontment)
        {
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
            return new Appointment(appontment.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, appontment.TypeOfTerm, false);
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
            return new Appointment(appointment.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, appointment.TypeOfTerm, false);
        }





        private void DeleteOwnAppointment()
        {
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
                    } while (!appointmentService.IsIntegerValid(numberAppointment));
                } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);

                Appointment appointmentForDelete = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
                appointmentService.DeleteAppointment(appointmentForDelete);
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
                    } while (!appointmentService.IsIntegerValid(numberAppointment));
                } while (Int32.Parse(numberAppointment) > this.allMyAppointments.Count);
                Appointment appointmentForUpdate = this.allMyAppointments[Int32.Parse(numberAppointment) - 1];
                Console.WriteLine(appointmentForUpdate.ToString());
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
