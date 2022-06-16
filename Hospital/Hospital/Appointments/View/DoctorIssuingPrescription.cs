using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Appointments.Service;
using Hospital.Appointments.Model;

using Autofac;
using Hospital;

namespace Hospital.Appointments.View
{
    public class DoctorIssuingPrescription
    {
        private IPrescriptionService prescriptionService;
        private IAppointmentService appointmentService;

        public DoctorIssuingPrescription()
        {
            prescriptionService = Globals.container.Resolve<IPrescriptionService>();
            appointmentService = Globals.container.Resolve<IAppointmentService>();

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
            string drug, startConsuming, dose, timeOfCnsuming;
            do
            {
                drug = this.EnterDrug();
                startConsuming = this.EnterStartConsuming();
                dose = this.EnterDose();
                timeOfCnsuming = this.EnterTimeOfConsuming();
            } while (!prescriptionService.CheckAllergicToDrug(healthRecord, drug));

            //saving precription
            Prescription newPrescription = new Prescription(appointment.AppointmentId, prescriptionService.GetId(drug), DateTime.ParseExact(startConsuming, "HH:mm", CultureInfo.InvariantCulture), Int32.Parse(dose), (Prescription.TimeOfConsuming)int.Parse(timeOfCnsuming));
            prescriptionService.Add(newPrescription);
        }

        private string EnterDrug()
        {
            string drug;
            do
            {
                Console.WriteLine("Unesite naziv leka: ");
                drug = Console.ReadLine();
            } while (!prescriptionService.IsDrugValid(drug));
            return drug;

        }

        private string EnterStartConsuming()
        {
            string startConsuming;
            do
            {
                Console.WriteLine("Unesite vreme kada pacijent treba da krene da pije lek (HH:mm): ");
                startConsuming = Console.ReadLine();
            } while (!Utils.IsTimeFormValid(startConsuming));
            return startConsuming;
        }

        private string EnterDose()
        {
            int tryIntConvert;
            string dose;
            do
            {
                Console.WriteLine("Koliko puta na dan treba da pije: ");
                dose = Console.ReadLine();
            } while (!int.TryParse(dose, out tryIntConvert));
            return dose;
        }

        private string EnterTimeOfConsuming()
        {
            string timeOfConsuming;
            do
            {
                Console.WriteLine("Da li da pije:\n1)Tokom obroka\n2)Posle obroka\n3)Pre obroka\n4)Nije bitno\n>> ");
                timeOfConsuming = Console.ReadLine();
            } while (!prescriptionService.IsTimeOfConsumingValid(timeOfConsuming));
            return timeOfConsuming;
        }
    }
}
