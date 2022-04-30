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
        HealthRecordService healthRecordService = new HealthRecordService();
        MedicalRecordService medicalRecordService = new MedicalRecordService();
        List<MedicalRecord> medicalRecords;
        List<HealthRecord> healthRecords;
        Helper helper;
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;

        public Doctor(User currentRegisteredDoctor, Helper helper)
        {
            this.currentRegisteredDoctor = currentRegisteredDoctor;
            this.helper = helper;
            allMyAppointments = appointmentService.GetDoctorAppointment(currentRegisteredDoctor);
            healthRecords = healthRecordService.GetHealthRecords;
            medicalRecords = medicalRecordService.GetMedicalRecords;

        }
        public void DoctorMenu()
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
                    this.ReadOwnAppointment();
                }
                else if (choice.Equals("2"))
                {
                    Console.WriteLine("\n2.Kreiraj sopstveni pregled/operaciju");
                    this.CreateOwnAppointment();
                }
                else if (choice.Equals("3"))
                {
                    Console.WriteLine("\n3.Izmeni sopstveni pregled/operaciju");
                    this.UpdateOwnAppointment();
                }
                else if (choice.Equals("4"))
                {
                    Console.WriteLine("\n4. Obrisi sopstveni pregled/operaciju");
                    this.DeleteOwnAppointment();
                }
                else if (choice.Equals("5"))
                {
                    Console.WriteLine("5. Ispitivanje sopstvenog rasporeda");
                    this.ExaminingOwnSchedule();
                }
                else if (choice.Equals("6"))
                {
                    Console.WriteLine("6. Izvodjenje pregleda/operacija");
                    this.PerformingAppointment();
                }
                else if (choice.Equals("7"))
                {
                    this.LogOut();
                }
            } while (true);
        }

        private void PerformingAppointment()
        {
            string dateNow = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime parseDateNow = DateTime.ParseExact(dateNow, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            List<Appointment> appointmentsForPerformanse = new List<Appointment>();
            foreach (Appointment appointmentOwn in this.allMyAppointments)
            {
                if (appointmentOwn.DoctorEmail.Equals(currentRegisteredDoctor.Email) &&(appointmentOwn.DateAppointment == parseDateNow)
                    &&(appointmentOwn.GetAppointmentState != Appointment.AppointmentState.Deleted)&&(appointmentOwn.AppointmentPerformed == false))
                {
                    appointmentsForPerformanse.Add(appointmentOwn);
                }

            }

            if (appointmentsForPerformanse.Count == 0)
            {
                Console.WriteLine("\nSvi pregledi/operacije su obavljenji!");
                return;
            }
            else
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
            string choice;
            do
            {
                do
                {
                    Console.WriteLine("Unesite redni broj pacijenta kojeg želite da pregledate: ");
                    choice= Console.ReadLine();

                } while (!appointmentService.IsIntegerValid(choice));
            } while (Int32.Parse(choice) > appointmentsForPerformanse.Count);
            Appointment appointmentOfSelected = appointmentsForPerformanse[Int32.Parse(choice) - 1];
            //set the appointment to be performed
            appointmentOfSelected.SetAppointmentPerformed(true);
           
            foreach (User user in userService.Users)
            {
                if (user.Email.Equals(appointmentOfSelected.PatientEmail))
                {
                    Console.WriteLine("Ime: " + user.Name + "\n" + "Prezime: " + user.Surname + "\n");

                }
            }
            
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(appointmentOfSelected.PatientEmail))
                {
                    Console.WriteLine(healthRecord.ToString());

                }
            }

            Console.WriteLine();
            string anamnesisInput;
            Console.WriteLine("Unesite anamnezu: ");
            anamnesisInput = Console.ReadLine();

            //update appointments.csv file
            appointmentService.RemakePerformedAppointment(appointmentOfSelected);
           
            MedicalRecord newMedicalRecord = new MedicalRecord(appointmentOfSelected.AppointmentId, anamnesisInput, " ");
            string medicalRecordToWriteToFile = "\n" + appointmentOfSelected.AppointmentId + ";" + anamnesisInput + ";" + " ";
            // append new medical record in file
            string filePathForMedicalRecords = @"..\..\Data\medicalRecords.csv";
            File.AppendAllText(filePathForMedicalRecords, medicalRecordToWriteToFile);
            Console.WriteLine("Uspešno ste uneli anamnezu.");

            // add to medicalRecords
            medicalRecords.Add(newMedicalRecord);

            //updating health record
            HealthRecord healthRecordSelectedPatient;
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(appointmentOfSelected.PatientEmail))
                {
                    healthRecordSelectedPatient = healthRecord;
                    Console.WriteLine(healthRecord.ToString());
                    this.UpdatingSelectedHealthRecord(healthRecordSelectedPatient);

                }
            }
            





        }

        private void ExaminingOwnSchedule()
        {
            string dateAppointment;
            do
            {
                Console.WriteLine("Unesite željeni datum(MM/dd/yyyy): ");
                dateAppointment = Console.ReadLine();
            } while (!appointmentService.IsDateFormValid(dateAppointment));

            this.ReadOwnAppointmentSpecificDate(DateTime.ParseExact(dateAppointment, "MM/dd/yyyy", CultureInfo.InvariantCulture));

        }




        private void ReadOwnAppointmentSpecificDate(DateTime dateSchedule)
        {
            DateTime dateForNextThreeDays = dateSchedule.AddDays(3);
            List<Appointment> appointmentsOfParticularDay = new List<Appointment>();
            Console.WriteLine("Raspored za datum od " + dateSchedule.Month + "/" + dateSchedule.Day + "/" + dateSchedule.Year + " do " + dateForNextThreeDays.Month + "/" + dateForNextThreeDays.Day + "/" + dateForNextThreeDays.Year);
            foreach (Appointment appointmentOwn in this.allMyAppointments)
            {
                if (appointmentOwn.DoctorEmail.Equals(currentRegisteredDoctor.Email) && (appointmentOwn.DateAppointment >= dateSchedule)&&(appointmentOwn.DateAppointment <= dateForNextThreeDays) && (appointmentOwn.GetAppointmentState != Appointment.AppointmentState.Deleted))
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

        private void ReadOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }

            Console.WriteLine("\n\tPREGLEDI I OPERACIJE\n");
            int serialNumberAppointment = 1;
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}|{7,10}", "Br.","Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina","Stanj"));
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

        private void CreateOwnAppointment()
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

            int id = helper.getNewAppointmentId();
            Appointment appointment = new Appointment(id.ToString(), patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.AppointmentState.Created, roomNumber, (Appointment.TypeOfTerm)int.Parse(typeOfTerm),false);
            string newAppointment = "\n" + id + "," + patientEmail + "," + currentRegisteredDoctor.Email + ","  + newDate + "," +
                newStartTime + "," + newEndTime.Hour + ":" + newEndTime.Minute + "," + (int)Appointment.AppointmentState.Created + "," + newRoomNumber + "," + typeOfTerm + "," + "false";

            // append new appointment in file
            string filePath = @"..\..\Data\appointments.csv"; 
            File.AppendAllText(filePath, newAppointment);
            Console.WriteLine("Uspešno ste zakazali termin.");

            // add to appointments
            appointmentService.Appointments.Add(appointment);
            allMyAppointments.Add(appointment);
            




        }

        private void DeleteOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
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
            this.UpdatingAllMyAppointment(appointmentForDelete);

           
            

            

        }

        private void UpdateOwnAppointment()
        {
            if (this.allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
            this.ReadOwnAppointment();
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
            if (appointmentForUpdate.GetTypeOfTerm == Appointment.TypeOfTerm.Examination)
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
            //remove the selected appointment from the list
            this.UpdatingAllMyAppointment(appointmentForUpdate);
            Appointment newAppointment = new Appointment(appointmentForUpdate.AppointmentId, patientEmail, currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.AppointmentState.Updated, roomNumber, appointmentForUpdate.GetTypeOfTerm, false);
           
            //adding the modified element to the list and write  to the file
            appointmentService.Appointments.Add(newAppointment);
            this.allMyAppointments.Add(newAppointment);
            appointmentService.UpdateAppointment(appointmentForUpdate);



        }
        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }

        private void UpdatingAllMyAppointment(Appointment appointmentForUpdate)
        {
            List<Appointment> undeletedOwnAppointment = new List<Appointment>();
            foreach(Appointment appointment in this.allMyAppointments)
            {
                if (!appointment.AppointmentId.Equals(appointmentForUpdate.AppointmentId))
                {
                   
                    undeletedOwnAppointment.Add(appointment);
                }
            }
            this.allMyAppointments = undeletedOwnAppointment;
        }

        private void UpdatingSelectedHealthRecord(HealthRecord healthRecordSelected)
        {
            string selectionOfUpdates;
            string patientHeightInput;
            string patientWeightInput;
            string previousIllnessesInput;
            string allergenInput;
            string bloodTypeInput;
            do
            {

                Console.WriteLine("Da li želite da ažurirate zdravstveni karton pacijenta? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                selectionOfUpdates = Console.ReadLine();
                if (selectionOfUpdates.Equals("1"))
                {
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

                }
                else if (selectionOfUpdates.Equals("2"))
                {
                    return;
                }
            } while (!selectionOfUpdates.Equals("1") && !selectionOfUpdates.Equals("2"));

        }


    }
}
