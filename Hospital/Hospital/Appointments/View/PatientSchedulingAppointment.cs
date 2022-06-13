using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

using Hospital.Appointments.Service;
using Hospital.Users.Service;
using Hospital.Users.View;
using Hospital.Appointments.Model;
using Hospital;
using Hospital.Rooms.Model;

namespace Hospital.Appointments.View
{
    public class PatientSchedulingAppointment
    {
        private AppointmentService _appointmentService;
        private UserService _userService;
        private Patient _currentRegisteredUser;
        private PatientAppointmentsService _patientAppointmentsService;
        private UserActionService _userActionService;

        public AppointmentService AppointmentService { get { return _appointmentService; } }

        public PatientSchedulingAppointment(Patient patient, AppointmentService appointmentService, UserService userService, PatientAppointmentsService patientAppointmentsService, UserActionService userActionService)
        {
            this._userService = userService;
            this._appointmentService = appointmentService;
            this._currentRegisteredUser = patient;
            this._patientAppointmentsService = patientAppointmentsService;
            this._userActionService = userActionService;
        }

        public string AcceptAppointment(Appointment newAppointment)
        {
            Console.WriteLine("\nPRONADJEN TERMIN PREGLEDA");
            this._currentRegisteredUser.TableHeaderForPatient();
            Console.WriteLine();
            Console.WriteLine("1. " + newAppointment.DisplayOfPatientAppointment());

            string choice;
            do
            {
                Console.WriteLine("\nIzaberite opciju");
                Console.WriteLine("1. Prihvatam");
                Console.WriteLine("2. Odbijam");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    return "1";
                else if (choice.Equals("2"))
                    return "2";
            } while (true);
        }

        public List<Appointment> FindRandomAppointmentForScheduling(string[] inputValues)
        {
            List<Appointment> appointmentsForChoosing = new List<Appointment>();

            DateTime appointmentDate = DateTime.Now.AddDays(1);
            DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
            string[] dataForAppointment;

            do
            {
                if (Utils.IsTimeBetweenTwoTimes(startTime))
                {
                    startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
                    appointmentDate = appointmentDate.AddDays(1);
                }

                dataForAppointment = new string[] { inputValues[0], appointmentDate.ToString("MM/dd/yyyy"), startTime.ToString("HH:mm") };
              
                if (this._patientAppointmentsService.IsAppointmentFree("0", dataForAppointment))
                    appointmentsForChoosing.Add(this.CreateAppointment(dataForAppointment));
                
                startTime = startTime.AddMinutes(15);
            } while (appointmentsForChoosing.Count != 3);

            return appointmentsForChoosing;
        }

        public Appointment CreateAppointment(string[] dataForAppointment)
        {
            DateTime appointmentDate = DateTime.ParseExact(dataForAppointment[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime startTime = DateTime.ParseExact(dataForAppointment[2], "HH:mm", CultureInfo.InvariantCulture);

            string id = this.AppointmentService.GetNewAppointmentId().ToString();
            Room freeRoom = this.AppointmentService.FindFreeRoom(appointmentDate, startTime);
            int roomId = Int32.Parse(freeRoom.Id);

            Appointment newAppointment = new Appointment(id, this._currentRegisteredUser.Email, dataForAppointment[0],
                appointmentDate, startTime, startTime.AddMinutes(15), Appointment.State.Created, roomId,
                Appointment.Type.Examination, false, false);

            return newAppointment;
        }

        public Appointment PickAppointmentForScheduling(List<Appointment> appointments)
        {
            this._patientAppointmentsService.PrintRecommendedAppointments(appointments);
            int numAppointment;
            string choice;
            do
            {
                Console.WriteLine("\nUnesite broj pregleda koji zelite da zakazete");
                Console.Write(">> ");
                choice = Console.ReadLine();
            } while (!int.TryParse(choice, out numAppointment) || numAppointment < 1 || numAppointment > appointments.Count);

            return appointments[numAppointment - 1];
        }

        public void SchedulingAppointment(string doctorEmail)
        {
            string[] inputValues = _patientAppointmentsService.InputValuesForAppointment(doctorEmail);

            if (!_patientAppointmentsService.IsAppointmentFree("0", inputValues))
            {
                Console.WriteLine("Nije moguce zakazati pregled u izabranom terminu!");
                return;
            }

            Appointment newAppointment = this.CreateAppointment(inputValues);
            this._appointmentService.AddAppointment(newAppointment);
            this._currentRegisteredUser.PatientAppointments = _patientAppointmentsService.RefreshPatientAppointments();
            this._userActionService.ActionRepository.AppendToActionFile("create");
            this._userActionService.AntiTrolMechanism();

            Console.WriteLine("Uspesno ste kreirali nov pregled!");
        }
    }
}
