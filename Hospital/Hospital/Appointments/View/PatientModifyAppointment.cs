using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using Hospital.Users.View;
using Hospital.Users.Service;
using Hospital.Appointments.Service;
using Hospital.Appointments.Model;

namespace Hospital.Appointments.View
{
    public class PatientModifyAppointment
    {
        private Patient _currentPatient;
        private PatientAppointmentsService _patientAppointment;
        private UserActionService _userActionService;
        private PatientRequestService _requestService;
       private AppointmentService _appointmentService;

        public PatientModifyAppointment(Patient patient, PatientAppointmentsService patientAppointmentsService, UserActionService userActionService, AppointmentService appointmentService)
        {

            this._currentPatient = patient;
            this._patientAppointment = patientAppointmentsService;
            this._userActionService = userActionService;
            this._requestService = new PatientRequestService();
            this._appointmentService = appointmentService;
        }

        public void DeleteOwnAppointment()
        {
            Appointment appointmentForDelete = this.PickAppointmentForDeleteOrUpdate();

            if (appointmentForDelete == null)
                return;

            this.DeleteAppointment(appointmentForDelete);
            this._currentPatient.PatientAppointments = _patientAppointment.RefreshPatientAppointments();
            this._userActionService.ActionRepository.AppendToActionFile("delete");
            this._userActionService.AntiTrolMechanism();
        }

        public void UpdateOwnAppointment()
        {
            Appointment appointmentForUpdate = this.PickAppointmentForDeleteOrUpdate();

            if (appointmentForUpdate == null)
                return;

            string[] inputValues = _patientAppointment.InputValuesForAppointment("");

            if (!_patientAppointment.IsAppointmentFree(appointmentForUpdate.AppointmentId, inputValues))
            {
                Console.WriteLine("Nije moguce izmeniti pregled u izabranom terminu!");
                return;
            }

            this.UpdateAppointment(appointmentForUpdate, inputValues);
            this._userActionService.ActionRepository.AppendToActionFile("update");
            this._userActionService.AntiTrolMechanism();
        }

        public List<Appointment> FindAppointmentsForDeleteAndUpdate()
        {
            this._currentPatient.TableHeaderForPatient();
            Console.WriteLine();

            List<Appointment> appointmentsForChange = new List<Appointment>();
            foreach (Appointment appointment in _currentPatient.PatientAppointments)
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
            foreach (Appointment appointment in _currentPatient.PatientAppointments)
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
            _appointmentService.Update(appointmentForDelete);
            this._currentPatient.PatientAppointments = _patientAppointment.RefreshPatientAppointments();
        }

        private Appointment ModifyExistingAppointment(Appointment appointmentForModify, string[] inputValues)
        {
            DateTime appointmentDate = DateTime.ParseExact(inputValues[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
            DateTime appointmentEndTime = startTime.AddMinutes(15);
            return new Appointment(appointmentForModify.AppointmentId, this._currentPatient.Email, inputValues[0],
                appointmentDate, startTime, appointmentEndTime, appointmentForModify.AppointmentState,
                appointmentForModify.RoomNumber, Appointment.Type.Examination, false, false);
        }

        public void UpdateAppointment(Appointment appointmentForUpdate, string[] inputValues)
        {
            Appointment updatedAppointment = this.ModifyExistingAppointment(appointmentForUpdate, inputValues);
            foreach (Appointment appointment in _currentPatient.PatientAppointments)
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
            this._appointmentService.Update(updatedAppointment);
            this._currentPatient.PatientAppointments = _patientAppointment.RefreshPatientAppointments();
        }
    }
}
