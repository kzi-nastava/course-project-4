using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

using Hospital.Users.View;
using Hospital.Appointments.Service;
using Hospital.Drugs.Service;
using Hospital.Drugs.Repository;
using Hospital.Appointments.Model;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.View
{
    public class PatientDrugNotification
    {
        private Patient _currentPatient;
        private AppointmentService _appointmentService;
        private DrugNotificationService _drugNotificationService;
        private DrugService _drugService;
        private PrescriptionService _prescriptionService;

        public PatientDrugNotification(Patient patient, AppointmentService appointmentService, DrugNotificationService drugNotificationService)
        {
            this._currentPatient = patient;
            this._appointmentService = appointmentService;
            this._drugNotificationService = drugNotificationService;
            this._drugService = new DrugService(new DrugRepository(new IngredientService(new IngredientRepository())));
            this._prescriptionService = new PrescriptionService();
        }

        public void ShowDrugNotification()
        {
            this.InformPatientAboutDrug();
            string choice;
            Console.WriteLine("\nDa li zelite da izmenite vreme obavestenja?");
            do
            {
                Console.WriteLine("\n1. DA       2. NE");
                Console.Write(">> ");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    this._drugNotificationService.ChangeNotificationTime(this._currentPatient.Email);
                    this._drugNotificationService.Notifications = this._drugNotificationService.DrugNotificationRepository.Load(); // refresh data
                }
                else if (choice.Equals("2"))
                    return;
            } while (choice != "1" && choice != "2");
        }

        private Dictionary<string, List<DateTime>> FindDrugsForPatient()
        {
            Dictionary<string, List<DateTime>> idAndDrugTime = new Dictionary<string, List<DateTime>>();
            List<DateTime> drugTime = new List<DateTime>();
            foreach (Prescription prescription in _prescriptionService.Prescriptions)
            {
                DateTime timeConsuming = prescription.StartConsuming;
                foreach (Appointment appointment in _appointmentService.Appointments)
                {
                    if (appointment.AppointmentId.Equals(prescription.IdAppointment)
                        && appointment.PatientEmail.Equals(this._currentPatient.Email))
                    {
                        int takingDifference = 24 / prescription.Dose;
                        drugTime.Add(prescription.StartConsuming);
                        for (int i = 1; i < prescription.Dose; i++)
                        {
                            timeConsuming = timeConsuming.AddHours(takingDifference);
                            drugTime.Add(timeConsuming);
                        }
                        idAndDrugTime.Add(prescription.IdDrug, drugTime);
                        drugTime = new List<DateTime>();
                    }
                }
            }
            return idAndDrugTime;
        }

        private Dictionary<string, List<DateTime>> FindDrugsNames()
        {
            Dictionary<string, List<DateTime>> drugsNamesAndTime = new Dictionary<string, List<DateTime>>();
            foreach (KeyValuePair<string, List<DateTime>> idAndDrugTime in this.FindDrugsForPatient())
            {
                foreach (Drug drug in this._drugService.Drugs)
                {
                    if (idAndDrugTime.Key.Equals(drug.IdDrug))
                        drugsNamesAndTime.Add(drug.DrugName, idAndDrugTime.Value);
                }
            }
            return drugsNamesAndTime;
        }

        private DateTime FindNotificationTime()
        {
            foreach (DrugNotification notification in this._drugNotificationService.Notifications)
            {
                if (notification.PatientEmail.Equals(this._currentPatient.Email))
                    return notification.TimeNotification; 
            }
            return DateTime.ParseExact("00:00", "HH:mm", CultureInfo.InvariantCulture);
        }

        private void InformPatientAboutDrug()
        {
            DateTime timeNotification = this.FindNotificationTime();
            if (timeNotification.ToString("HH:mm").Equals("00:00"))
            {
                Console.WriteLine("Ne pijete lekove :)");
                return;
            }
            bool isHours = true;
            if (timeNotification.Hour == 0)
                isHours = false;

            this.CheckDrugTime(timeNotification, isHours);
        }

        private void CheckDrugTime(DateTime timeNotification, bool isHours)
        {
            bool isTime = false;
            Dictionary<string, List<DateTime>> drugsNamesAndTime = this.FindDrugsNames();
            foreach (KeyValuePair<string, List<DateTime>> drug in drugsNamesAndTime)
            {
                foreach (DateTime takingTime in drug.Value)
                {
                    if (takingTime.TimeOfDay > DateTime.Now.TimeOfDay && DateTime.Now.AddHours(timeNotification.Hour) >= takingTime && isHours)
                    {
                        Console.WriteLine("Treba da popijete " + drug.Key + " za " + takingTime.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss"));
                        isTime = true;
                    }
                    else if (takingTime.TimeOfDay > DateTime.Now.TimeOfDay && DateTime.Now.AddMinutes(timeNotification.Hour) >= takingTime && !isHours)
                    {
                        Console.WriteLine("Treba da popijete " + drug.Key + " za " + takingTime.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss"));
                        isTime = true;
                    }
                }
                if (!isTime)
                    Console.WriteLine("Jos uvek ne treba da popijete " + drug.Key);
                isTime = false;
            }
        }
    }
}
