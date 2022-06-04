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
    class PatientAppointmentsService
    {
        private AppointmentService _appointmentService;
        private List<Appointment> _allAppointments;
        private Patient _currentRegisteredUser;

        public AppointmentService AppointmentService { get { return _appointmentService; } }

        public PatientAppointmentsService(Patient patient, AppointmentService appointmentService)
        {
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

        public void ReadOwnAppointments()
        {
            if (!this.HasPatientAppointment(this._currentRegisteredUser.PatientAppointments))
                return;

            int serialNumber = 0;

            this._currentRegisteredUser.TableHeaderForPatient();
            Console.WriteLine();

            foreach (Appointment appointment in this._currentRegisteredUser.PatientAppointments)
            {
                serialNumber++;
                Console.WriteLine(serialNumber + ". " + appointment.DisplayOfPatientAppointment());
            }
            Console.WriteLine();
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

        public bool IsAppointmentFree(string id, string[] inputValues)
        {
            string doctorEmail = inputValues[0];
            string newDate = inputValues[1];
            string newStartTime = inputValues[2];

            DateTime dateExamination = DateTime.Parse(newDate);
            DateTime startTime = DateTime.Parse(newStartTime);

            foreach (Appointment appointment in _appointmentService.Appointments)
            {
                if (appointment.DoctorEmail.Equals(doctorEmail) && appointment.DateAppointment == dateExamination
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime)
                    return false;
                else if (appointment.PatientEmail.Equals(_currentRegisteredUser.Email) && appointment.DateAppointment == dateExamination
                    && appointment.StartTime <= startTime && appointment.EndTime > startTime && !appointment.AppointmentId.Equals(id))
                    return false;
            }
            return true;
        }

        public string[] IsDoctorAvailable(string[] inputValues)
        {
            DateTime latestDate = DateTime.ParseExact(inputValues[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(inputValues[3], "HH:mm", CultureInfo.InvariantCulture);

            DateTime earliestDate = DateTime.Now.AddDays(1);
            string[] dataForAppointment;
            do
            {
                if (startTime.TimeOfDay >= endTime.TimeOfDay)
                {
                    earliestDate = earliestDate.AddDays(1);
                    startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture); // reset time
                }

                if (earliestDate.Date > latestDate.Date)
                    return null;

                dataForAppointment = new string[] { inputValues[0], earliestDate.ToString("MM/dd/yyyy"), startTime.ToString("HH:mm") };
                startTime = startTime.AddMinutes(15);
            } while (!this.IsAppointmentFree("0", dataForAppointment));

            return dataForAppointment;
        }

        public void PrintRecommendedAppointments(List<Appointment> appointments)
        {
            int numAppointment = 1;
            Console.WriteLine("\nPREDLOZI TERMINA PREGLEDA");
            this._currentRegisteredUser.TableHeaderForPatient();
            Console.WriteLine();
            foreach (Appointment appointment in appointments)
            {
                Console.WriteLine(numAppointment + ". " + appointment.DisplayOfPatientAppointment());
                numAppointment += 1;
            }
        }
    }
}
