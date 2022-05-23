using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using Hospital.Service;
using Hospital.Model;

namespace Hospital.PatientImplementation
{
    class PatientDrugNotification
    {
        Patient _currentPatient;
        DrugNotificationService _drugNotificationService;
        DrugService _drugService;
        PrescriptionService _prescriptionService;
        AppointmentService _appointmentService;

        public PatientDrugNotification(Patient patient)
        {
            this._currentPatient = patient;
            this._drugNotificationService = new DrugNotificationService();
            this._drugService = new DrugService();
            this._prescriptionService = new PrescriptionService();
            this._appointmentService = new AppointmentService();
        }

        private Dictionary<string, List<DateTime>> FindDrugsForPatient()
        {
            Dictionary<string, List<DateTime>> idAndDrugTime = new Dictionary<string, List<DateTime>>();
            List<DateTime> drugTime = new List<DateTime>();
            foreach (Prescription prescription in _prescriptionService.Prescriptions)
            {
                foreach (Appointment appointment in _appointmentService.Appointments)
                {
                    if (appointment.AppointmentId.Equals(prescription.IdAppointment)
                        && appointment.PatientEmail.Equals(this._currentPatient.Email))
                    {
                        int takingDifference = 24 / prescription.Dose;
                        drugTime.Add(prescription.StartConsuming);
                        for (int i = 1; i < prescription.Dose; i++)
                            drugTime.Add(prescription.StartConsuming.AddHours(takingDifference));
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

        public void InformPatientAboutDrug()
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
            Dictionary<string, List<DateTime>> drugsNamesAndTime = this.FindDrugsNames();
            foreach (KeyValuePair<string, List<DateTime>> drug in drugsNamesAndTime)
            {
                foreach (DateTime takingTime in drug.Value)
                {
                    if (takingTime.TimeOfDay > DateTime.Now.TimeOfDay && DateTime.Now.AddHours(timeNotification.Hour) >= takingTime && isHours)
                        Console.WriteLine("Treba da popijete " + drug.Key + "za " + takingTime.Subtract(DateTime.Now).ToString("HH:mm"));
                    else if (takingTime.TimeOfDay > DateTime.Now.TimeOfDay && DateTime.Now.AddMinutes(timeNotification.Hour) >= takingTime && !isHours)
                        Console.WriteLine("Treba da popijete " + drug.Key + "za " + takingTime.Subtract(DateTime.Now).ToString("HH:mm"));
                    else
                    {
                        Console.WriteLine("Jos uvek ne treba da popijete " + drug.Key);
                        break;
                    }
                }
            }
        }

        public void ChangeTimeNotification()
        {
            Console.Write("\nUnesite koliko vremena ranije zelite da dobije obavestenje: ");
            string newTime = Console.ReadLine();

            List<string> lines = new List<string>();
            string line;
            foreach (DrugNotification notification in this._drugNotificationService.Notifications)
            {
                if (notification.PatientEmail.Equals(this._currentPatient.Email))
                    line = notification.PatientEmail + "," + newTime; 
                else
                    line = notification.PatientEmail + "," + notification.TimeNotification.ToString("HH:mm");
                lines.Add(line);
            }
            File.WriteAllLines(@"..\..\Data\drugNotification.csv", lines.ToArray());

            this._drugNotificationService = new DrugNotificationService(); // refresh data  
        }
    }
}
