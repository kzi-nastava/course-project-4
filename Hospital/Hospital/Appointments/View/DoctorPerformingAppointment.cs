using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Hospital;
using Hospital.Appointments.Service;
using Hospital.Appointments.Model;
using Hospital.Drugs.Service;
using Hospital.Users.Model;
using Hospital.Users.Service;

namespace Hospital.Appointments.View
{
    public class DoctorPerformingAppointment
    {
        private IAppointmentService appointmentService;
        private List<HealthRecord> healthRecords;
        private IIngredientService ingredientService;
        private IHealthRecordService healthRecordService;
        private List<Appointment> allMyAppointments;
        private User currentRegisteredDoctor;
        private IUserService userService;
        public DoctorPerformingAppointment(List<HealthRecord> allHealthRecords, User doctor, List<Appointment> appointments)
        {
            appointmentService = Globals.container.Resolve<IAppointmentService>();
            healthRecords = allHealthRecords;
            ingredientService = Globals.container.Resolve<IIngredientService>();
            healthRecordService = Globals.container.Resolve<IHealthRecordService>();
            currentRegisteredDoctor = doctor;
            allMyAppointments = appointments;
            userService = Globals.container.Resolve<IUserService>();
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
                    choice = this.EnterChoice();
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

        private string EnterChoice()
        {
            string choice;
            int tryIntConvert;
            do
            {
                Console.WriteLine("Unesite redni broj pacijenta kojeg želite da pregledate: ");
                choice = Console.ReadLine();

            } while (!int.TryParse(choice, out tryIntConvert));
            return choice;
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
            appointmentService.Save();

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
                    DoctorIssuingPrescription doctorIssuing = new DoctorIssuingPrescription();
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
            string patientHeightInput = this.EnterPatientHeight();
            string patientWeightInput = this.EnterPatientWeight();
            Console.WriteLine("Unesite prethodne bolesti: ");
            string previousIllnessesInput = Console.ReadLine();
            string allergenInput = this.EnterAllergen();
            Console.WriteLine("Unesite krvnu grupu: ");
            string bloodTypeInput = Console.ReadLine();

            HealthRecord newHealthRecord = new HealthRecord(healthRecordSelected.IdHealthRecord, healthRecordSelected.EmailPatient, Int32.Parse(patientHeightInput), double.Parse(patientWeightInput), previousIllnessesInput, allergenInput, bloodTypeInput);
            healthRecordService.Update(newHealthRecord);
        }
        private string EnterPatientHeight()
        {
            string patientHeightInput;
            int tryIntConvert;
            do
            {
                Console.WriteLine("Unesite visinu: ");
                patientHeightInput = Console.ReadLine();

            } while (!int.TryParse(patientHeightInput, out tryIntConvert));
            return patientHeightInput;
        }

        private string EnterPatientWeight()
        {
            string patientWeightInput;
            double convertDouble;
            do
            {
                Console.WriteLine("Unesite težinu: ");
                patientWeightInput = Console.ReadLine();
            } while (!double.TryParse(patientWeightInput, out convertDouble));
            return patientWeightInput;

        }

        private string EnterAllergen()
        {
            string allergenInput;
            do
            {
                Console.WriteLine("Unesite alergenu: ");
                allergenInput = Console.ReadLine();
            } while (!ingredientService.IsIngredientNameValid(allergenInput));
            return allergenInput;
        }
    }
}
