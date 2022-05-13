using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using System.Globalization;
using System.IO;

namespace Hospital.PatientImplementation
{
    class PatientSchedulingAppointment
    {
        AppointmentService _appointmentService = new AppointmentService();  // loading all appointments
        List<User> _allUsers;
        User _currentRegisteredUser;

        public AppointmentService AppointmentService { get { return _appointmentService; } }

        public PatientSchedulingAppointment(User user, List<User> allUsers)
        {
            this._currentRegisteredUser = user;
            this._allUsers = allUsers;
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

        public bool IsTimeBetweenTwoTimes(DateTime time)
        {
            DateTime midnight = DateTime.ParseExact("00:00", "HH:mm", CultureInfo.InvariantCulture);
            DateTime earliestTime = DateTime.ParseExact("06:00", "HH:mm", CultureInfo.InvariantCulture);
            if (time.TimeOfDay >= midnight.TimeOfDay && time.TimeOfDay < earliestTime.TimeOfDay)
                return true;
            return false;
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
                if (this.IsTimeBetweenTwoTimes(startTime))
                {
                    startTime = DateTime.ParseExact("06:00", "HH:mm", CultureInfo.InvariantCulture);
                    earliestDate = earliestDate.AddDays(1);
                }

                if (earliestDate.Date > latestDate.Date)
                    return this.FindAppointmentsClosestPatientWishes(inputValues);

                dataForAppointment = new string[] { doctorEmail, earliestDate.ToString("MM/dd/yyyy"), startTime.ToString("HH:mm") };
                startTime = startTime.AddMinutes(15);
            } while (!this.IsAppointmentFree("0", dataForAppointment));

            return this.CreateAppointment(dataForAppointment);
        }

        public string AcceptAppointment(Appointment newAppointment)
        {
            Console.WriteLine("\nPRONADJEN TERMIN PREGLEDA");
            this._appointmentService.TableHeaderForPatient();
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

        public Appointment FindAppointmentInTheSelectedRange(string[] inputValues)
        {
            string[] dataForAppointment = new string[3];

            foreach (User doctor in this.AllDoctors())
            {
                dataForAppointment = this.IsDoctorAvailable(inputValues, doctor);
                if (dataForAppointment != null)
                    break;
            }
            if (dataForAppointment == null)
                return this.FindAppointmentsClosestPatientWishes(inputValues); 

            return this.CreateAppointment(dataForAppointment);
        }

        public string[] IsDoctorAvailable(string[] inputValues, User doctor)
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
                    startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
                }

                if (earliestDate.Date > latestDate.Date)
                    return null;

                dataForAppointment = new string[] { doctor.Email, earliestDate.ToString("MM/dd/yyyy"), startTime.ToString("HH:mm") };
                startTime = startTime.AddMinutes(15);
            } while (!this.IsAppointmentFree("0", dataForAppointment));

            return dataForAppointment;
        }

        public Appointment FindAppointmentsClosestPatientWishes(string[] inputValues)
        {
            foreach (User doctor in this.AllDoctors())
            {
                if (doctor.Email.Equals(inputValues[0]))
                    continue;
                inputValues[0] = doctor.Email;
                break;
            }
            List<Appointment> appointmentsForChoosing = FindRandomAppointmentForScheduling(inputValues);
            return this.PickAppointmentForScheduling(appointmentsForChoosing);
        }

        public List<Appointment> FindRandomAppointmentForScheduling(string[] inputValues)
        {
            List<Appointment> appointmentsForChoosing = new List<Appointment>();

            DateTime appointmentDate = DateTime.Now.AddDays(1);
            DateTime startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
            string[] dataForAppointment;

            do
            {
                if (this.IsTimeBetweenTwoTimes(startTime))
                {
                    startTime = DateTime.ParseExact(inputValues[2], "HH:mm", CultureInfo.InvariantCulture);
                    appointmentDate = appointmentDate.AddDays(1);
                }

                dataForAppointment = new string[] { inputValues[0], appointmentDate.ToString("MM/dd/yyyy"), startTime.ToString("HH:mm") };

                if (this.IsAppointmentFree("0", dataForAppointment))
                    appointmentsForChoosing.Add(this.CreateAppointment(dataForAppointment));
                
                startTime = startTime.AddMinutes(15);
            } while (appointmentsForChoosing.Count != 3);

            return appointmentsForChoosing;
        }

        public List<User> AllDoctors()
        {
            List<User> allDoctors = new List<User>();
            foreach (User user in _allUsers)
                if (user.UserRole == User.Role.Doctor)
                    allDoctors.Add(user);
            return allDoctors;
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
                Appointment.Type.Examination, false);

            return newAppointment;
        }

        public void PrintAppointments(List<Appointment> appointments)
        {
            int numAppointment = 1;
            Console.WriteLine("\nPREDLOZI TERMINA PREGLEDA");
            this._appointmentService.TableHeaderForPatient();
            Console.WriteLine();
            foreach (Appointment appointment in appointments)
            {
                Console.WriteLine(numAppointment + ". " + appointment.DisplayOfPatientAppointment());
                numAppointment += 1;
            }
        }

        public Appointment PickAppointmentForScheduling(List<Appointment> appointments)
        {
            this.PrintAppointments(appointments);
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

    }
}
