using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using System.IO;
using System.Globalization;
using Hospital.Service;

namespace Hospital.PatientImplementation
{
    class Patient
    {
        private string _email;
        private PatientAppointments _patientService;
        private AppointmentService _appointmentService;
        private PatientSchedulingAppointment _patientScheduling;
        private PatientAnamnesis _patientAnamnesis;
        private PatientDoctorSearch _doctorSearch;
        private PatientDoctorSurvey _doctorSurvey;
        private List<Appointment> _currentAppointments;
        private PatientDrugNotification _drugNotification;
        private UserService _userService;
        private UserActionService _userActionService;
        private DrugNotificationService _drugNotificationService;

        public string Email { get { return _email; } }
        public List<Appointment> PatientAppointments
        {
            get { return _currentAppointments; }
            set { _currentAppointments = value; }
        }
        public PatientDoctorSurvey PatientDoctorSurvey { get { return _doctorSurvey; } }

        public Patient(string email, PatientSchedulingAppointment patientScheduling)
        {
            this._email = email;
            this._appointmentService = new AppointmentService();
            this._drugNotificationService = new DrugNotificationService();
            this._userService = new UserService();
            this._patientService = new PatientAppointments(this, _appointmentService);
            this._patientScheduling = patientScheduling;
            this._patientScheduling.RegisteredUser = this;
            this._patientAnamnesis = new PatientAnamnesis(this, _appointmentService, _userService);
            this._currentAppointments = this._patientService.RefreshPatientAppointments();
            this._doctorSearch = new PatientDoctorSearch(this, _userService);
            this._doctorSurvey = new PatientDoctorSurvey(_userService);
            this._drugNotification = new PatientDrugNotification(this, _appointmentService, _drugNotificationService);
            this._userActionService = new UserActionService(this);
        }

        // methods
        public void PatientMenu()
        {
            // the menu
            string choice;
            Console.WriteLine("\n\tMENI");
            Console.Write("------------------");
            do
            {
                Console.WriteLine("\n1. Lista sopstvenih pregleda");
                Console.WriteLine("2. Zakazi pregled");
                Console.WriteLine("3. Izmeni pregled");
                Console.WriteLine("4. Obrisi pregled");
                Console.WriteLine("5. Preporuka termina pregleda");
                Console.WriteLine("6. Pregled i pretraga anamneza");
                Console.WriteLine("7. Pretrazi doktora");
                Console.WriteLine("8. Pogledaj obavestenja");
                Console.WriteLine("9. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                // patient choice
                if (choice.Equals("1"))
                    this.ReadOwnAppointments();
                else if (choice.Equals("2"))
                    this.SchedulingAppointment("");
                else if (choice.Equals("3"))
                    this.UpdateOwnAppointment();
                else if (choice.Equals("4"))
                    this.DeleteOwnAppointment();
                else if (choice.Equals("5"))
                    this.RecommendationFreeAppointments();
                else if (choice.Equals("6"))
                    this._patientAnamnesis.MainMenuForSearch();
                else if (choice.Equals("7"))
                    this._doctorSearch.MenuForDoctorSearch();
                else if (choice.Equals("8"))
                    this._drugNotification.ShowDrugNotification();
                else if (choice.Equals("9"))
                    this.LogOut();
            } while (true);
        }

        public void TableHeaderForPatient()
        {
            Console.WriteLine();
            Console.Write(String.Format("{0,3}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}|{7,10}",
                "Br.", "Doktor", "Datum", "Pocetak", "Kraj", "Soba", "Tip", "Stanje"));
        }

        private void ReadOwnAppointments()
        {
            if (!_patientService.HasPatientAppointment(this._currentAppointments)) 
                return;
                
            int serialNumber = 0;

            this.TableHeaderForPatient();
            Console.WriteLine();
            
            foreach (Appointment appointment in this._currentAppointments)
            {
                serialNumber++;
                Console.WriteLine(serialNumber + ". " + appointment.DisplayOfPatientAppointment());
            }
            Console.WriteLine();
        }

        private void DeleteOwnAppointment() 
        {
            Appointment appointmentForDelete = _patientService.PickAppointmentForDeleteOrUpdate();

            if (appointmentForDelete == null)
                return;

            this._patientService.DeleteAppointment(appointmentForDelete);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            this._userActionService.ActionRepository.AppendToActionFile("delete");
            this._userActionService.AntiTrolMechanism();
        }

        private void UpdateOwnAppointment()
        {
            Appointment appointmentForUpdate = _patientService.PickAppointmentForDeleteOrUpdate();

            if (appointmentForUpdate == null)
                return;

            string[] inputValues = _patientService.InputValuesForAppointment("");

            if (!_patientScheduling.IsAppointmentFree(appointmentForUpdate.AppointmentId, inputValues))
            {
                Console.WriteLine("Nije moguce izmeniti pregled u izabranom terminu!");
                return;
            }

            this._patientService.UpdateAppointment(appointmentForUpdate, inputValues);
            this._userActionService.ActionRepository.AppendToActionFile("update");
            this._userActionService.AntiTrolMechanism();
        }

        public void SchedulingAppointment(string doctorEmail)
        {
            string[] inputValues = _patientService.InputValuesForAppointment(doctorEmail);

            if (!_patientScheduling.IsAppointmentFree("0", inputValues))
            {
                Console.WriteLine("Nije moguce zakazati pregled u izabranom terminu!");
                return;
            }

            Appointment newAppointment = _patientScheduling.CreateAppointment(inputValues);
            this._appointmentService.AddAppointment(newAppointment);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            this._userActionService.ActionRepository.AppendToActionFile("create");
            this._userActionService.AntiTrolMechanism();

            Console.WriteLine("Uspesno ste kreirali nov pregled!");
        }

        private void RecommendationFreeAppointments()
        {
            string[] inputValues = _patientService.InputValueForRecommendationAppointments();
            string priority = _patientScheduling.CheckPriority();
            Appointment newAppointment;

            if (priority.Equals("1"))
                newAppointment = _patientScheduling.FindAppointmentAtChosenDoctor(inputValues);
            else
                newAppointment = _patientScheduling.FindAppointmentInTheSelectedRange(inputValues);

            if (newAppointment == null)
            {
                Console.WriteLine("Zakazivanje je odbijeno!");
                return;
            }
            if (_patientScheduling.AcceptAppointment(newAppointment).Equals("2"))
                return;

            this._patientService.AppointmentService.AppendNewAppointmentInFile(newAppointment);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            this._userActionService.ActionRepository.AppendToActionFile("create");
            this._userActionService.AntiTrolMechanism();
        }

        public void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
