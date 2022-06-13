using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

using Hospital.Appointments.View;
using Hospital.Appointments.Service;
using Hospital.Appointments.Model;
using Hospital.Drugs.View;
using Hospital.Users.Service;
using Hospital.Drugs.Service;

namespace Hospital.Users.View
{
    public class Patient : IMenuView
    {
        private string _email;
        private PatientAppointmentsService _patientService;
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
        private PatientModifyAppointment _modifyAppointment;
        private RecommendedAppointment _recommendedAppointment;

        public string Email { get { return _email; } }
        public List<Appointment> PatientAppointments
        {
            get { return _currentAppointments; }
            set { _currentAppointments = value; }
        }
        public PatientDoctorSurvey PatientDoctorSurvey { get { return _doctorSurvey; } }

        public Patient(string email)
        {
            this._email = email;
            this._appointmentService = new AppointmentService();
            this._drugNotificationService = new DrugNotificationService();
            this._userService = new UserService();
            this._userActionService = new UserActionService(this);
            this._patientService = new PatientAppointmentsService(this, _appointmentService);
            this._patientScheduling = new PatientSchedulingAppointment(this, _appointmentService, _userService, _patientService, _userActionService);
            this._patientAnamnesis = new PatientAnamnesis(this, _appointmentService, _userService);
            this._currentAppointments = this._patientService.RefreshPatientAppointments();
            this._doctorSearch = new PatientDoctorSearch(this, _userService, _patientScheduling);
            this._doctorSurvey = new PatientDoctorSurvey(_userService);
            this._drugNotification = new PatientDrugNotification(this, _appointmentService, _drugNotificationService);
            this._modifyAppointment = new PatientModifyAppointment(this, _patientService, _userActionService, _appointmentService);
            this._recommendedAppointment = new RecommendedAppointment(this, _userService, _patientService, _userActionService, _patientScheduling, _appointmentService);
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
                    this._patientService.ReadOwnAppointments();
                else if (choice.Equals("2"))
                    this._patientScheduling.SchedulingAppointment("");
                else if (choice.Equals("3"))
                    this._modifyAppointment.UpdateOwnAppointment();
                else if (choice.Equals("4"))
                    this._modifyAppointment.DeleteOwnAppointment();
                else if (choice.Equals("5"))
                    this._recommendedAppointment.RecommendationFreeAppointments();
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
    }
}
