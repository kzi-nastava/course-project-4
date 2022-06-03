using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.DoctorImplementation
{
    class DoctorReferral
    {
        UserService userService;
        ReferralService referralService;
        MedicalRecordService medicalRecordService;
        public DoctorReferral()
        {
            userService = new UserService();
            referralService = new ReferralService();
            medicalRecordService = new MedicalRecordService();

        }

        public void WritingReferral(Appointment appointment, string anamnesis)
        {
            string choice;
            do
            {
                Console.WriteLine("Da li želite da izdate uput pacijentu? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    this.ChooseDoctor(appointment, anamnesis);
                }
                
            } while (!choice.Equals("1") && !choice.Equals("2"));

            
            DynamicEquipmentRecords dynamicEquipmentRecords = new DynamicEquipmentRecords();
            dynamicEquipmentRecords.DisplayAmountOfDynamicEquipments(appointment.RoomNumber.ToString());


        }

        private void ChooseDoctor(Appointment appointment, string anamnesis)
        {
            string choice;
            do
            {
                Console.WriteLine("Izaberite:  \n1) odredjeni doktor \n2) po specijalnosti\nUnesite 1 ili 2.");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    this.ReferralToSpecificDoctor(appointment, anamnesis);

                }
                else if (choice.Equals("2"))
                {
                    this.ReferralToSelectedSpeciality(appointment, anamnesis);

                }
            } while (!choice.Equals("1") && !choice.Equals("2"));

           


        }
        private void ReferralToSpecificDoctor(Appointment appointment, string anamnesis)
        {
            string doctor;
            List<DoctorUser> doctors = userService.GetDoctors();
            this.PrintDoctors(doctors);
            do
            {
                Console.WriteLine("Unesite redni broj doktora: ");
                doctor = Console.ReadLine();
            } while (Int32.Parse(doctor) > doctors.Count);
            DoctorUser doctorUser = doctors[Int32.Parse(doctor) - 1];
            Appointment.Type type = GetTypeAppointment();
            Referral newReferral = new Referral(referralService.GetNewReferralId().ToString(), appointment.PatientEmail, doctorUser.Email, doctorUser.SpecialityDoctor, type, false);
            referralService.AddReferral(newReferral);
            Console.WriteLine("Uspesno ste uneli uput!");

            //updating medical record
            MedicalRecord newMedicalRecord = new MedicalRecord(appointment.AppointmentId, anamnesis, newReferral.Id);
            medicalRecordService.AddMedicalRecord(newMedicalRecord);


        }

        private void ReferralToSelectedSpeciality(Appointment appointment, string anamnesis)
        {
            string speciality;
            this.PrintSpecialities();
            do
            {
                Console.WriteLine("Unesite redni broj specijalnosti: ");
                speciality = Console.ReadLine();

            } while (Int32.Parse(speciality) > Enum.GetNames(typeof(DoctorUser.Speciality)).Length);
            Appointment.Type type = GetTypeAppointment();
            DoctorUser.Speciality specialitySelected = (DoctorUser.Speciality)int.Parse(speciality);
            Referral newReferral = new Referral(referralService.GetNewReferralId().ToString(), appointment.PatientEmail, "null", specialitySelected, type, false);
            referralService.AddReferral(newReferral);
            Console.WriteLine("Uspesno ste uneli uput!");

            //updating medical record
            MedicalRecord newMedicalRecord = new MedicalRecord(appointment.AppointmentId, anamnesis, newReferral.Id);
            medicalRecordService.AddMedicalRecord(newMedicalRecord);


        }

        private Appointment.Type GetTypeAppointment()
        {
            string choice;
            do
            {
                Console.WriteLine("Uput za: \n1. Pregled\n2.Operaciju\n>>");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    return (Appointment.Type)int.Parse(choice);
                }
                return (Appointment.Type)int.Parse(choice);
            } while (!choice.Equals("1") || !choice.Equals("2"));



        }

        private void PrintDoctors(List<DoctorUser> doctors)
        {
            int serialNumber = 1;
            Console.WriteLine(String.Format("|{0,5}|{1,20}|{2,20}|", "Br.", "Doktor", "Specijalnost"));
            foreach (DoctorUser doctor in doctors)
            {
                Console.WriteLine(String.Format("|{0,5}|{1,20}|{2,20}|", serialNumber, doctor.Name + " " + doctor.Surname, doctor.SpecialityDoctor));
                serialNumber += 1;

            }
        }

        private void PrintSpecialities()
        {
            int serialNumber = 1;
            Console.WriteLine(String.Format("|{0,5}|{1,11}|", "Br.", "Specijalnost"));
            foreach (DoctorUser.Speciality speciality in Enum.GetValues(typeof(DoctorUser.Speciality)))
            {
                Console.WriteLine(String.Format("|{0,5}|{1,11}|", serialNumber, speciality));
                serialNumber += 1;
            }

        }

    }
}
