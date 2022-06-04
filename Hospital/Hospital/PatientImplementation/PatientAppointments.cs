using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using System.Globalization;
using System.IO;
using Hospital.DoctorImplementation;

namespace Hospital.PatientImplementation
{
    class PatientAppointments
    {
        private PatientRequestService _requestService;
        private AppointmentService _appointmentService;
        private List<Appointment> _allAppointments;
        private Patient _currentRegisteredUser;

        public AppointmentService AppointmentService { get { return _appointmentService; } }

        public PatientAppointments(Patient patient, AppointmentService appointmentService)
        {
            this._requestService = new PatientRequestService();
            this._appointmentService = appointmentService;
            this._allAppointments = _appointmentService.AppointmentRepository.Load();
            this._currentRegisteredUser = patient;
        }

        public List<Appointment> RefreshPatientAppointments() 
        {
             this._allAppointments = _appointmentService.AppointmentRepository.Load();

            // finding all appointments for the registered patient that have not been deleted and has not yet passed
            List<Appointment> patientCurrentAppointment = new List<Appointment>();
            foreach (Appointment appointment in _allAppointments)
            {
                if (appointment.PatientEmail.Equals(_currentRegisteredUser.Email) &&
                    appointment.AppointmentState != Appointment.State.Deleted &&
                    appointment.DateAppointment >= DateTime.Now.Date)
                {
                    // check if today's appointment has passed
                    if (appointment.DateAppointment == DateTime.Now.Date && appointment.StartTime.Hour <= DateTime.Now.Hour)
                        continue;
                    patientCurrentAppointment.Add(appointment);
                }
            }
            return patientCurrentAppointment;
        }

        public List<Appointment> FindAppointmentsForDeleteAndUpdate()
        {
            this._currentRegisteredUser.TableHeaderForPatient();
            Console.WriteLine();

            List<Appointment> appointmentsForChange = new List<Appointment>();
            foreach (Appointment appointment in _currentRegisteredUser.PatientAppointments) 
            {
                if (appointment.AppointmentState != Appointment.State.UpdateRequest &&
                    appointment.AppointmentState != Appointment.State.DeleteRequest && 
                    appointment.TypeOfTerm != Appointment.Type.Operation)
                {
                    appointmentsForChange.Add(appointment);
                    Console.WriteLine(appointmentsForChange.Count + ". " + appointment.DisplayOfPatientAppointment());
                }
            }
            Console.WriteLine();

            return appointmentsForChange;
        }

        public bool HasPatientAppointment(List<Appointment> patientAppointments)
        {
            if (!(patientAppointments.Count == 0))
                return true;
            Console.WriteLine("\nNemate ni jedan pregled za prikaz!");
            return false;
        }

        public string[] InputValuesForAppointment(string doctorEmail)
        {
            string[] inputValues = new string[3];

            string newDate;
            string newStartTime;

            do
            {
                if (doctorEmail.Equals(""))
                {
                    Console.Write("\nUnesite email doktora: ");
                    doctorEmail = Console.ReadLine();
                }
                Console.Write("Unesite datum (MM/dd/yyyy): ");
                newDate = Console.ReadLine();
                Console.Write("Unesite vreme pocetka pregleda (HH:mm): ");
                newStartTime = Console.ReadLine();
            } while (!this.IsValidAppointmentInput(doctorEmail, newDate, newStartTime));

            inputValues[0] = doctorEmail;
            inputValues[1] = newDate;
            inputValues[2] = newStartTime;

            return inputValues;
        }

        public bool IsValidAppointmentInput(string doctorEmail, string newDateAppointment, string newStartTime)
        {
            return (Utils.IsDateFormValid(newDateAppointment) &&
                Utils.IsTimeFormValid(newStartTime) && _appointmentService.IsDoctorExist(doctorEmail) != null);
        }

        public Appointment PickAppointmentForDeleteOrUpdate()
        {
            List<Appointment> appointmentsForChange = this.FindAppointmentsForDeleteAndUpdate();

            if (appointmentsForChange.Count == 0)
                return null;

            string inputNumberAppointment;
            int numberAppointment;
            do
            {
                Console.WriteLine("Unesite broj pregleda za izabranu operaciju");
                Console.Write(">> ");
                inputNumberAppointment = Console.ReadLine();
            } while (!int.TryParse(inputNumberAppointment, out numberAppointment) || numberAppointment < 1
            || numberAppointment > appointmentsForChange.Count);

            return appointmentsForChange[numberAppointment - 1];
        }

        public void DeleteAppointment(Appointment appointmentForDelete)
        {
            foreach (Appointment appointment in _currentRegisteredUser.PatientAppointments)
            {
                if (appointment.AppointmentId.Equals(appointmentForDelete.AppointmentId))
                {
                    if ((appointmentForDelete.DateAppointment - DateTime.Now).TotalDays <= 2)
                    {
                        appointmentForDelete.AppointmentState = Appointment.State.DeleteRequest;
                        this._requestService.Requests.Add(appointmentForDelete);
                        this._requestService.UpdateFile();
                        Console.WriteLine("Zahtev za brisanje je poslat sekretaru!");
                    }
                    else
                    {
                        appointmentForDelete.AppointmentState = Appointment.State.Updated;
                        Console.WriteLine("Uspesno ste izvrsili brisanje pregleda!");
                    }
                }
            }
            _appointmentService.UpdateAppointment(appointmentForDelete);
            this._currentRegisteredUser.PatientAppointments = this.RefreshPatientAppointments();
        }

        private Appointment ModifyExistingAppointment(Appointment appointmentForModify, string[] inputValues)
        {
            DateTime appointmentDate = DateTime.ParseExact(inputValues[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
            DateTime appointmentEndTime = startTime.AddMinutes(15);
            return new Appointment(appointmentForModify.AppointmentId, this._currentRegisteredUser.Email, inputValues[0], 
                appointmentDate, startTime, appointmentEndTime, appointmentForModify.AppointmentState, 
                appointmentForModify.RoomNumber, Appointment.Type.Examination, false, false);
        }

        public void UpdateAppointment(Appointment appointmentForUpdate, string[] inputValues)
        {
            Appointment updatedAppointment = this.ModifyExistingAppointment(appointmentForUpdate, inputValues);
            foreach (Appointment appointment in _currentRegisteredUser.PatientAppointments)
            {
                if (appointment.AppointmentId.Equals(appointmentForUpdate.AppointmentId))
                {
                    if ((appointmentForUpdate.DateAppointment - DateTime.Now).TotalDays <= 2)
                    {
                        updatedAppointment.AppointmentState = Appointment.State.UpdateRequest;
                        this._requestService.Requests.Add(updatedAppointment);
                        this._requestService.UpdateFile();
                        Console.WriteLine("Zahtev za izmenu je poslat sekretaru!");
                    }
                    else
                    {
                        updatedAppointment.AppointmentState = Appointment.State.Updated;
                        Console.WriteLine("Uspesno ste izvrsili izmenu pregleda!");
                    }
                }
            }
            this._appointmentService.UpdateAppointment(updatedAppointment);
            this._currentRegisteredUser.PatientAppointments = this.RefreshPatientAppointments();
        }

        public string[] InputValueForRecommendationAppointments()
        {
            string[] inputValues = new string[4];

            string doctorEmail;
            string latestDate;
            string startTime;
            string endTime;

            do
            {
                Console.Write("\nUnesite email doktora: ");
                doctorEmail = Console.ReadLine();
                Console.Write("Unesite datum do kog mora da bude pregled (MM/dd/yyyy): ");
                latestDate = Console.ReadLine();
                Console.Write("Unesite vreme najranije moguceg pregleda (HH:mm): ");
                startTime = Console.ReadLine();
                Console.Write("Unesite vreme najkasnijeg moguceg pregleda (HH:mm): ");
                endTime = Console.ReadLine();
            } while (!(this.IsValidAppointmentInput(doctorEmail, latestDate, startTime) && Utils.IsTimeFormValid(endTime)
            && Utils.CompareTwoTimes(startTime, endTime)));

            inputValues[0] = doctorEmail;
            inputValues[1] = latestDate;
            inputValues[2] = startTime;
            inputValues[3] = endTime;

            return inputValues;
        }
    }
}
