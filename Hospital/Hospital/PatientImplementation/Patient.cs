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
        string _email;
        PatientService _patientService;
        PatientSchedulingAppointment _patientScheduling;
        PatientAnamnesis _patientAnamnesis;
        List<Appointment> _currentAppointments; 

        public string Email { get { return _email; } }
        public List<Appointment> PatientAppointments
        {
            get { return _currentAppointments; }
            set { _currentAppointments = value; }
        } 

        public Patient(string email, PatientService patientService, PatientSchedulingAppointment patientScheduling, PatientAnamnesis patientAnamnesis)
        {
            this._email = email;
            this._patientService = patientService;
            this._patientScheduling = patientScheduling;
            this._patientAnamnesis = patientAnamnesis;
            this._currentAppointments = patientService.RefreshPatientAppointments();
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
                Console.WriteLine("7. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                // patient choice
                if (choice.Equals("1"))
                    this.ReadOwnAppointments();
                else if (choice.Equals("2"))
                    this.SchedulingAppointment();
                else if (choice.Equals("3"))
                    this.UpdateAppointment();
                else if (choice.Equals("4"))
                    this.DeleteAppointment();
                else if (choice.Equals("5"))
                    this.RecommendationFreeAppointments();
                else if (choice.Equals("6"))
                    this.AnamnesisSearch();
                else if (choice.Equals("7"))
                    this.LogOut();
            } while (true);
        }

        private void ReadOwnAppointments()
        {
            if (!_patientService.HasPatientAppointmen(this._currentAppointments)) 
                return;
                
            int serialNumber = 0;

            _patientService.AppointmentService.TableHeaderForPatient();
            Console.WriteLine();
            
            foreach (Appointment appointment in this._currentAppointments)
            {
                serialNumber++;
                Console.WriteLine(serialNumber + ". " + appointment.DisplayOfPatientAppointment());
            }
            Console.WriteLine();
        }

        private void DeleteAppointment() 
        {
            Appointment appointmentForDelete = _patientService.PickAppointmentForDeleteOrUpdate(this);

            if (appointmentForDelete == null)
                return;

            _patientService.ReadFileForDeleteAppointment(appointmentForDelete);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            _patientService.AppendToActionFile("delete");
            this.AntiTrolMechanism();
        }

        private void UpdateAppointment()
        {
            Appointment appointmentForUpdate = _patientService.PickAppointmentForDeleteOrUpdate(this);

            if (appointmentForUpdate == null)
                return;

            string[] inputValues = _patientService.InputValuesForAppointment();

            if (!_patientScheduling.IsAppointmentFree(appointmentForUpdate.AppointmentId, inputValues))
            {
                Console.WriteLine("Nije moguce izmeniti pregled u izabranom terminu!");
                return;
            }

            _patientService.ReadFileForUpdateAppointment(appointmentForUpdate, inputValues);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            _patientService.AppendToActionFile("update");
            this.AntiTrolMechanism();

        }

        private void SchedulingAppointment()
        {
            string[] inputValues = _patientService.InputValuesForAppointment();

            if (!_patientScheduling.IsAppointmentFree("0", inputValues))
            {
                Console.WriteLine("Nije moguce zakazati pregled u izabranom terminu!");
                return;
            }

            Appointment newAppointment = _patientScheduling.CreateAppointment(inputValues);

            Console.WriteLine("Uspesno ste kreirali nov pregled!");

            _patientService.AppointmentService.AppendNewAppointmentInFile(newAppointment);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            _patientService.AppendToActionFile("create");
            this.AntiTrolMechanism(); 
        }

        private void AntiTrolMechanism()
        {
            int changed = 0;
            int deleted = 0;
            int created = 0;

            List<UserAction> myCurrentActions = _patientService.LoadMyCurrentActions(this._email);

            foreach (UserAction action in myCurrentActions) {
                if (action.ActionState == UserAction.State.Created)
                    created += 1;
                else if (action.ActionState == UserAction.State.Modified)
                    changed += 1;
                else if (action.ActionState == UserAction.State.Deleted)
                    deleted += 1;
            }

            if (changed > 4)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste izmenili termin.\nPristup aplikaciji Vam je sada blokiran!");
            else if (deleted > 4)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste obrisali termin.\nPristup aplikaciji Vam je sada blokiran!");
            else if (created > 8)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste kreirali termin.\nPristup aplikaciji Vam je sada blokiran!");
            else
                return;
          
            _patientService.BlockAccessApplication();
            this.LogOut(); //log out from account
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

            _patientService.AppointmentService.AppendNewAppointmentInFile(newAppointment);
            this._currentAppointments = _patientService.RefreshPatientAppointments();
            _patientService.AppendToActionFile("create");
            this.AntiTrolMechanism();
        }

        private void AnamnesisSearch()
        {
            _patientAnamnesis.MainMenuForSearch();
        }

        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
