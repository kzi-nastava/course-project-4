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
using Hospital.Users.Repository;
using Hospital;
using Autofac;
namespace Hospital.Users.View
{
    public class Patient : IMenuView
    {
        private string _email;
        private PatientAppointmentsService _patientService; //ovde
        private IAppointmentService _appointmentService;
        private PatientSchedulingAppointment _patientScheduling;
        private PatientAnamnesis _patientAnamnesis;
        private PatientDoctorSearch _doctorSearch;
        private PatientDoctorSurvey _doctorSurvey;
        private List<Appointment> _currentAppointments;
        private PatientDrugNotification _drugNotification;
        private IUserService _userService;
        private IUserActionService _userActionService;
        private IDrugNotificationService _drugNotificationService;
        private PatientModifyAppointment _modifyAppointment;
        private RecommendedAppointment _recommendedAppointment;
        private HospitalSurveyService _hospitalSurveyService;

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
            this._appointmentService = Globals.container.Resolve<IAppointmentService>();
            this._drugNotificationService = Globals.container.Resolve<IDrugNotificationService>();
            this._userService = Globals.container.Resolve<IUserService>();
            this._userActionService = Globals.container.Resolve<IUserActionService>();
            this._patientService = new PatientAppointmentsService(this);
            this._patientScheduling = new PatientSchedulingAppointment(this, _patientService);
            this._patientAnamnesis = new PatientAnamnesis(this, _patientService);
            this._currentAppointments = this._patientService.RefreshPatientAppointments();
            this._doctorSearch = new PatientDoctorSearch(this, _patientScheduling);
            this._doctorSurvey = new PatientDoctorSurvey(_patientService);
            this._drugNotification = new PatientDrugNotification(this);
            this._modifyAppointment = new PatientModifyAppointment(this, _patientService);
            this._recommendedAppointment = new RecommendedAppointment(this, _patientService, _patientScheduling);
            this._hospitalSurveyService = new HospitalSurveyService(this._email);
        }

        // methods
        public void PatientMenu()
        {
            // the menu
            string choice;
            Console.WriteLine("\n\tMENI");
            Console.Write("-------------------------");
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
                Console.WriteLine("9. Anketa bolnice");
                Console.WriteLine("10. Oceni doktora");
                Console.WriteLine("11. Odjava");
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
                    this._hospitalSurveyService.EvaluateHospitalSurvey();
                else if (choice.Equals("10"))
                    this._doctorSurvey.GetAppointmentsForEvaluation();
                else if (choice.Equals("11"))
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