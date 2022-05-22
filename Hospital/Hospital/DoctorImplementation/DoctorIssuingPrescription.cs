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
    class DoctorIssuingPrescription
    {
        PrescriptionService prescriptionService;
        AppointmentService appointmentService;

        public DoctorIssuingPrescription(AppointmentService serviceAppointment)
        {
            prescriptionService = new PrescriptionService();
            appointmentService = serviceAppointment;

        }

        public void IssuingPrescription(Appointment appointment, HealthRecord healthRecord)
        {
            string choice;
            do
            {
                Console.WriteLine("Da li želite da izdate recept pacijentu? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    this.WritingPrescription(appointment, healthRecord);
                    Console.WriteLine("Uspesno ste upisali recept!");

                }
                else if (choice.Equals("2"))
                {
                    return;
                }
            } while (!choice.Equals("1") && !choice.Equals("2"));

        }

        private void WritingPrescription(Appointment appointment, HealthRecord healthRecord)
        {
            string drug, startConsuming, dose, timeOfconsuming;
            do
            {
                do
                {
                    Console.WriteLine("Unesite naziv leka: ");
                    drug = Console.ReadLine();
                } while (!prescriptionService.IsDrugValid(drug));
                do
                {
                    Console.WriteLine("Unesite vreme kada pacijent treba da krene da pije lek (HH:mm): ");
                    startConsuming = Console.ReadLine();
                } while (!appointmentService.IsTimeFormValid(startConsuming));
                do
                {
                    Console.WriteLine("Koliko puta na dan treba da pije: ");
                    dose = Console.ReadLine();
                } while (!appointmentService.IsIntegerValid(dose));
                do
                {
                    Console.WriteLine("Da li da pije:\n1)Tokom obroka\n2)Posle obroka\n3)Pre obroka\n4)Nije bitno\n>> ");
                    timeOfconsuming = Console.ReadLine();
                } while (!prescriptionService.IsTimeOfConsumingValid(timeOfconsuming));
            } while (!prescriptionService.CheckAllergicToDrug(healthRecord, drug));

            //saving precription
            Prescription newPrescription = new Prescription(appointment.AppointmentId, prescriptionService.GetIdDrug(drug), DateTime.ParseExact(startConsuming, "HH:mm", CultureInfo.InvariantCulture), Int32.Parse(dose), (Prescription.TimeOfConsuming)int.Parse(timeOfconsuming));
            prescriptionService.Prescriptions.Add(newPrescription);
            prescriptionService.UpdateFile();
        }
    }
}
