using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.DoctorImplementation
{
    class DoctorPerformingAppointment
    {
        AppointmentService appointmentService;
        List<HealthRecord> healthRecords;
        IngredientService ingredientService;
        HealthRecordService healthRecordService;
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;
        UserService userService;
        public DoctorPerformingAppointment(AppointmentService service, List<HealthRecord> allHealthRecords,HealthRecordService serviceHealthRecord, User doctor, List<Appointment> appointments, UserService serviceUser)
        {
            appointmentService = service;
            healthRecords = allHealthRecords;
            ingredientService = new IngredientService();
            healthRecordService = serviceHealthRecord;
            currentRegisteredDoctor = doctor;
            allMyAppointments = appointments;
            userService = serviceUser;
        }

        public void PerformingAppointment()
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

                userService.DisplayOfPatientData(appointmentOfSelected.PatientEmail);
                healthRecordService.DisplayOfPatientsHealthRecord(appointmentOfSelected.PatientEmail);
                this.EnteringAnamnesis(appointmentOfSelected);

                return;
            }
            else
            {
                Console.WriteLine("Svi pregledi su obavljeni!");
            }

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




        private void EnteringAnamnesis(Appointment appointment)
        {
            string anamnesisInput;
            Console.WriteLine("Unesite anamnezu: ");
            anamnesisInput = Console.ReadLine();

            //the examination was performed
            appointmentService.PerformAppointment(appointment);
            appointmentService.UpdateFile();

            Console.WriteLine("Uspešno ste uneli anamnezu.");

            //updating health record
            this.UpdatingHealthRecord(appointment);
            DoctorReferral doctorReferral = new DoctorReferral();
            doctorReferral.WritingReferral(appointment, anamnesisInput);


        }

        private void UpdatingHealthRecord(Appointment appointment)
        {
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(appointment.PatientEmail))
                {
                    DoctorIssuingPrescription doctorIssuing = new DoctorIssuingPrescription(appointmentService);
                    doctorIssuing.IssuingPrescription(appointment, healthRecord);
                    this.UpdatingSelectedHealthRecord(healthRecord);

                }
            }
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
            do
            {
                Console.WriteLine("Unesite alergenu: ");
                allergenInput = Console.ReadLine();
            } while (!ingredientService.IsIngredientNameValid(allergenInput));
            Console.WriteLine("Unesite krvnu grupu: ");
            bloodTypeInput = Console.ReadLine();

            HealthRecord newHealthRecord = new HealthRecord(healthRecordSelected.IdHealthRecord, healthRecordSelected.EmailPatient, Int32.Parse(patientHeightInput), double.Parse(patientWeightInput), previousIllnessesInput, allergenInput, bloodTypeInput);
            healthRecordService.UpdateHealthRecord(newHealthRecord);
            healthRecordService.UpdateHealthRecordFile();
        }
    }
}
