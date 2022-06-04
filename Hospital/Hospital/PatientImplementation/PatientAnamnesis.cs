using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using Hospital.Repository;

namespace Hospital.PatientImplementation
{
    class PatientAnamnesis
    {
        private AppointmentService _appointmentService;
        private MedicalRecordService _medicalRecordService; 
        private HealthRecordService _healthRecordService;
        private UserService _userService;
        private Patient _currentRegisteredUser;

        public PatientAnamnesis(Patient patient, AppointmentService appointmentService, UserService userService)
        {
            this._currentRegisteredUser = patient;
            this._userService = userService;
            this._appointmentService = appointmentService;
            this._medicalRecordService = new MedicalRecordService();
            this._healthRecordService = new HealthRecordService();
        }

        public void MainMenuForSearch()
        {
            string choice;
            Console.Write("\nIzaberite opciju");
            do
            {
                Console.WriteLine("\n1. Pregled zdravstvenog kartona");
                Console.WriteLine("2. Istorija pregleda");
                Console.WriteLine("3. Nazad");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    this.GetHealthRecordForPatient();
                else if (choice.Equals("2"))
                    this.AppointmentHistory();
                else if (choice.Equals("3"))
                    return;
            } while (true);
        }

        public void GetHealthRecordForPatient()
        {
            foreach (HealthRecord healthRecord in _healthRecordService.HealthRecords)
            {
                if (healthRecord.EmailPatient.Equals(this._currentRegisteredUser.Email))
                    Console.WriteLine("\n"+healthRecord.ToString());
            }
        }

        public void AppointmentHistory()
        {
            List<Appointment> performedAppointment = this.GetPerformedAppointmentForPatient();
            List<MedicalRecord> patientMedicalRecords = this.GetMedicalRecordForPatient(performedAppointment);
            string choice = this.MenuForAppointmentHistory();
            if (choice == "1")
                this.ShowAnamnesisForSelectedAppointment(patientMedicalRecords);
            else if (choice == "2")
                this.SearchAnamnesisBasedOnKeyword(patientMedicalRecords);
            else if (choice == "3")
                this.SortAnamnesis(performedAppointment, patientMedicalRecords);
        }

        public List<Appointment> GetPerformedAppointmentForPatient()
        {
            List<Appointment> performedAppointment = new List<Appointment>();
            this._currentRegisteredUser.TableHeaderForPatient();
            foreach (Appointment appointment in this._appointmentService.Appointments)
            {
                if (appointment.PatientEmail.Equals(this._currentRegisteredUser.Email) && appointment.AppointmentPerformed)
                {
                    performedAppointment.Add(appointment);
                    Console.Write("\n"+performedAppointment.Count + ". " + appointment.DisplayOfPatientAppointment());
                }
            }
            return performedAppointment;
        }

        public List<MedicalRecord> GetMedicalRecordForPatient(List<Appointment> performedAppointments)
        {
            List<MedicalRecord> medicalRecordsForPatient = new List<MedicalRecord>();
            foreach (Appointment appointment in performedAppointments)
            {
                foreach (MedicalRecord medicalRecord in this._medicalRecordService.MedicalRecords)
                {
                    if (medicalRecord.IdAppointment.Equals(appointment.AppointmentId))
                    {
                        medicalRecordsForPatient.Add(medicalRecord);
                        break;
                    }
                }
            }
            return medicalRecordsForPatient;
        }

        public string MenuForAppointmentHistory()
        {
            string choice;
            Console.Write("\n\nIzaberite opciju");
            do
            {
                Console.WriteLine("\n1. Pregled anamneze za odredjeni pregled");
                Console.WriteLine("2. Pretraga anamneze po kljucnoj reci");
                Console.WriteLine("3. Pregled svih anamneza");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    return "1";
                else if (choice.Equals("2"))
                    return "2";
                else if (choice.Equals("3"))
                    return "3";
            } while (true);
        }

        private void ShowAnamnesisForSelectedAppointment(List<MedicalRecord> medicalRecords)
        {
            int numAnamnesis;
            string choice;
            do
            {
                Console.WriteLine("\nUnesite broj pregleda za koji zelite da vidite anamnezu");
                Console.Write(">> ");
                choice = Console.ReadLine();
            } while (!int.TryParse(choice, out numAnamnesis) || numAnamnesis < 1 || numAnamnesis > medicalRecords.Count);

            Console.WriteLine("Anamneza:" +medicalRecords[numAnamnesis - 1].Anamnesis);
        }

        private void SearchAnamnesisBasedOnKeyword(List<MedicalRecord> medicalRecords)
        {
            Console.WriteLine("\nUnesite rec za pretragu");
            string keyWord = Console.ReadLine().ToLower();

            bool found = false;
            foreach (MedicalRecord medicalRecord in medicalRecords)
            {
                if (medicalRecord.Anamnesis.ToLower().Contains(keyWord) && keyWord != "")
                {
                    Console.WriteLine("Anamneza: " + medicalRecord.Anamnesis);
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("\nNe postoji ni jedna anamneza koja sadrzi rec " + keyWord);
        }

        private void SortAnamnesis(List<Appointment> preformedAppointments, List<MedicalRecord> medicalRecords)
        {
            foreach(MedicalRecord md in medicalRecords)
                Console.Write("\nAnamneza: " + md.Anamnesis);

            string choice;
            Console.Write("\n\nSortiraj po");
            do
            {
                Console.WriteLine("\n1. Datumu");
                Console.WriteLine("2. Doktoru");
                Console.WriteLine("3. Specijalizaciji");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    this.SortByDate(preformedAppointments);
                else if (choice.Equals("2"))
                    this.SortByDoctor(preformedAppointments);
                else if (choice.Equals("3"))
                    this.SortBySpecialization(preformedAppointments);
            } while (choice != "1" && choice != "2" && choice != "3");
        }

        private void SortByDate(List<Appointment> preformedAppointments)
        {
            preformedAppointments.Sort((x, y) => x.DateAppointment.Date.CompareTo(y.DateAppointment.Date));
            this.PrintAppointmentAndAnamnesis(preformedAppointments);
        }

        private void SortByDoctor(List<Appointment> preformedAppointments)
        {
            preformedAppointments.Sort((x, y) => x.DoctorEmail.CompareTo(y.DoctorEmail));
            this.PrintAppointmentAndAnamnesis(preformedAppointments);    
        }

        private void PrintAppointmentAndAnamnesis(List<Appointment> preformedAppointments)
        {
            this._currentRegisteredUser.TableHeaderForPatient();
            for (int i = 0; i < preformedAppointments.Count; i++)
                Console.Write("\n" + (i + 1) + ". " + preformedAppointments[i].DisplayOfPatientAppointment());

            Console.WriteLine();
            foreach (MedicalRecord medicalRecord in this.GetMedicalRecordForPatient(preformedAppointments))
                Console.WriteLine("Anamneza: " + medicalRecord.Anamnesis);
        }

        private void SortBySpecialization(List<Appointment> preformedAppointments)
        {
            this._userService.UsersRepository.DoctorUsers.Sort((x, y) => x.SpecialityDoctor.CompareTo(y.SpecialityDoctor));
            List<Appointment> sortedAppointments = new List<Appointment>();

            this._currentRegisteredUser.TableHeaderForPatient();
            Console.WriteLine("|  Specijalizacija");
            foreach (DoctorUser doctor in this._userService.UsersRepository.DoctorUsers)
            {
                foreach (Appointment appointment in preformedAppointments)
                {
                    if (appointment.DoctorEmail.Equals(doctor.Email))
                    {
                        sortedAppointments.Add(appointment);
                        Console.WriteLine(sortedAppointments.Count + ". " + appointment.DisplayOfPatientAppointment()
                            +"|    "+doctor.SpecialityDoctor);
                    }
                }
            }

            foreach (MedicalRecord medicalRecord in this.GetMedicalRecordForPatient(preformedAppointments))
                Console.WriteLine("Anamneza: " + medicalRecord.Anamnesis);
        }
    }
}
