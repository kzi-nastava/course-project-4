using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using Hospital.Users.View;
using Hospital.Users.Service;
using Hospital.Appointments.Service;
using Hospital;
using Hospital.Appointments.Model;
using Hospital.Users.Model;

namespace Hospital.Appointments.View
{
    public class RecommendedAppointment
    {
        private PatientAppointmentsService _patientAppointmentsService;
        private Patient _currentPatient;
        private UserActionService _userActionService;
        private UserService _userService;
        private PatientSchedulingAppointment _patientScheduling;
        private AppointmentService _appointmentService;

        public RecommendedAppointment(Patient patient, UserService userService, PatientAppointmentsService patientAppointmentsService, UserActionService userActionService, PatientSchedulingAppointment patientScheduling, AppointmentService appointmentService)
        {
            
            this._patientAppointmentsService = patientAppointmentsService;
            this._currentPatient = patient;
            this._userActionService = userActionService;
            this._userService = userService;
            this._patientScheduling = patientScheduling;
            this._appointmentService = appointmentService;
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
            } while (!(this._patientAppointmentsService.IsValidAppointmentInput(doctorEmail, latestDate, startTime) &&
            Utils.IsTimeFormValid(endTime) && Utils.CompareTwoTimes(startTime, endTime)));

            inputValues[0] = doctorEmail;
            inputValues[1] = latestDate;
            inputValues[2] = startTime;
            inputValues[3] = endTime;

            return inputValues;
        }

        public void RecommendationFreeAppointments()
        {
            string[] inputValues = this.InputValueForRecommendationAppointments();
            string priority = this.CheckPriority();
            Appointment newAppointment;

            if (priority.Equals("1"))
                newAppointment = this.FindAppointmentAtChosenDoctor(inputValues);
            else
                newAppointment = this.FindAppointmentInTheSelectedRange(inputValues);

            if (newAppointment == null)
            {
                Console.WriteLine("Zakazivanje je odbijeno!");
                return;
            }
            if (_patientScheduling.AcceptAppointment(newAppointment).Equals("2"))
                return;

            this._appointmentService.AddAppointment(newAppointment);
            this._currentPatient.PatientAppointments = _patientAppointmentsService.RefreshPatientAppointments();
            this._userActionService.ActionRepository.AppendToActionFile("create");
            this._userActionService.AntiTrolMechanism();
        }

        public Appointment FindAppointmentsClosestPatientWishes(string[] inputValues)
        {
            foreach (User doctor in this._userService.AllDoctors())
            {
                if (doctor.Email.Equals(inputValues[0]))
                    continue;
                inputValues[0] = doctor.Email;
                break;
            }
            List<Appointment> appointmentsForChoosing = this._patientScheduling.FindRandomAppointmentForScheduling(inputValues);
            return this._patientScheduling.PickAppointmentForScheduling(appointmentsForChoosing);
        }

        public string CheckPriority()
        {
            string priority;
            do
            {
                Console.WriteLine("\nIzaberite prioritet pri zakazivanju");
                Console.WriteLine("1. Doktor");
                Console.WriteLine("2. Vremenski opseg");
                Console.Write(">> ");
                priority = Console.ReadLine();

                if (priority.Equals("1"))
                    return "1";
                else if (priority.Equals("2"))
                    return "2";
            } while (true);
        }

        public Appointment FindAppointmentAtChosenDoctor(string[] inputValues)
        {
            string doctorEmail = inputValues[0];
            DateTime latestDate = DateTime.ParseExact(inputValues[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);

            DateTime earliestDate = DateTime.Now.AddDays(1);
            string[] dataForAppointment;
            do
            {
                if (Utils.IsTimeBetweenTwoTimes(startTime))
                {
                    startTime = DateTime.ParseExact("06:00", "HH:mm", CultureInfo.InvariantCulture);
                    earliestDate = earliestDate.AddDays(1);
                }

                if (earliestDate.Date > latestDate.Date)
                    return this.FindAppointmentsClosestPatientWishes(inputValues);

                dataForAppointment = new string[] { doctorEmail, earliestDate.ToString("MM/dd/yyyy"), startTime.ToString("HH:mm") };
                startTime = startTime.AddMinutes(15);
            } while (!this._patientAppointmentsService.IsAppointmentFree("0", dataForAppointment));

            return this._patientScheduling.CreateAppointment(dataForAppointment);
        }

        public Appointment FindAppointmentInTheSelectedRange(string[] inputValues)
        {
            string[] dataForAppointment = new string[3];

            foreach (User doctor in this._userService.AllDoctors())
            {
                inputValues[0] = doctor.Email;
                dataForAppointment = this._patientAppointmentsService.IsDoctorAvailable(inputValues);
                if (dataForAppointment != null)
                    break;
            }
            if (dataForAppointment == null)
                return this.FindAppointmentsClosestPatientWishes(inputValues);

            return this._patientScheduling.CreateAppointment(dataForAppointment);
        }
    }
}
