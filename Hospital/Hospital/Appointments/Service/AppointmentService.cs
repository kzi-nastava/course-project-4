using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Appointments.Repository;
using Hospital.Users.Service;
using Hospital.Appointments.Model;
using Hospital.Users.Model;
using Hospital.Rooms.Repository;
using Hospital.Rooms.Model;
using Hospital.Users.View;
using Hospital.Rooms.Service;

namespace Hospital.Appointments.Service
{
    public class AppointmentService : IAppointmentService
    {
        private AppointmentRepository _appointmentRepository;
        private UserService _userService;
        private NotificationService _notificationService;
        private List<Appointment> _appointments;
        private List<User> _users;
        private RoomRepository _roomRepository;
        private List<Room> _rooms;

        public AppointmentRepository AppointmentRepository { get { return _appointmentRepository; } }
        public List<Appointment> Appointments { get { return _appointments; } set { _appointments = value; } }
        public AppointmentService()
        {
            _appointmentRepository = new AppointmentRepository();
            _userService = new UserService();
            _roomRepository = new RoomRepository();
            _notificationService = new NotificationService();
            _appointments = _appointmentRepository.Load();
            _users = _userService.Users;
            _rooms = _roomRepository.Load();
        }

        public int GetNewAppointmentId()
        {
            return this._appointments.Count + 1;
        }

        public Appointment CreateNewAppointment(User patient, User doctor, DateTime startingTime, int appointmentType)
        {
            Room freeRoom = FindFreeRoom(startingTime, startingTime);
            DateTime endTime;
            if (appointmentType == 1)
                endTime = startingTime.AddMinutes(15);
            else
                endTime = startingTime.AddMinutes(60);
            return new Appointment(GetNewAppointmentId().ToString(), patient.Email, doctor.Email,
                startingTime, startingTime, endTime, Appointment.State.Created,
                Int32.Parse(freeRoom.Id), (Appointment.Type)appointmentType, false, true);
        }

        public bool IsDoctorFree(User doctor, DateTime startTime)
		{
            foreach(Appointment appointment in _appointments)
			{
                DateTime endTime = startTime.AddMinutes(60);
                if (appointment.DoctorEmail == doctor.Email && appointment.DateAppointment.Date == DateTime.Now.Date
                    && CheckOverlapTime(appointment, startTime, endTime))
                    return false;
			}
            return true;
		}

        public bool CheckOverlapTime(Appointment appointment, DateTime startTime, DateTime endTime)
		{
            if ((startTime <= appointment.StartTime) && (appointment.EndTime <= endTime))
                return true;
            else if ((appointment.StartTime <= endTime) && (endTime <= appointment.EndTime))
                return true;
            else if ((appointment.StartTime <= startTime) && (startTime <= appointment.EndTime))
                return true;
            else if ((appointment.StartTime <= startTime) && (endTime <= appointment.EndTime))
                return true;
            return false;
		}

        public List<Appointment> GetFilteredSortedAppointments()
		{
            var orderedAppointments = _appointments.OrderBy(a => a.Urgent).ThenByDescending(a => a.DateAppointment).ToList();
            List<Appointment> validAppointments = new List<Appointment>();
            foreach (Appointment appointment in orderedAppointments)
            {
                if (appointment.AppointmentPerformed)
                    continue;
                if (appointment.DateAppointment.Date < DateTime.Now.Date)
                    continue;
                if (appointment.DateAppointment.Date == DateTime.Now.Date && appointment.StartTime.TimeOfDay < DateTime.Now.TimeOfDay)
                    continue;
                validAppointments.Add(appointment);
            }
            return validAppointments;
        }

		public Appointment FindLeastUrgentAppointment()
        {
            List<Appointment> validAppointments = GetFilteredSortedAppointments();
            
            int i = 1;
            foreach (Appointment appointment in validAppointments)
			{
                string urgent = appointment.Urgent ? "Hitno" : "";
                Console.WriteLine("{0}. Pacijent: {1} | Doktor: {2} | {3} | {4} | {5}", i, _userService.GetUserFullName(appointment.PatientEmail),
                    _userService.GetUserFullName(appointment.DoctorEmail), appointment.DateAppointment.ToString("dd/MM/yyyy"),
                    appointment.StartTime.ToString("HH:mm"), urgent);
                i++;
			}
            string indexInput;
            int index;
			do
			{
                Console.Write(">>");
                indexInput = Console.ReadLine();
			} while (!int.TryParse(indexInput, out index) || index < 1 || index > validAppointments.Count());
            return validAppointments[index - 1];

        }

        public User FindFreeDoctor(DoctorUser.Speciality speciality, Appointment newAppointment)
		{
            List<User> allDoctors = _userService.FilterDoctors(speciality);
            foreach(User doctor in allDoctors)
			{
                newAppointment.DoctorEmail = doctor.Email;
				if (IsAppointmentFreeForDoctor(newAppointment))
				{
                    return doctor;
				}
			}
            newAppointment.DoctorEmail = "null";
            Console.WriteLine("\nNema slobodnih doktora u ovom terminu. \n");
            return null;
            
		}

        public List<Appointment> GetDoctorAppointment(User user)
        {
            //returns all appointments for a particular doctor
            List<Appointment> doctorAppointment = new List<Appointment>();
            foreach (Appointment appointment in this._appointments)
            {
                if (appointment.DoctorEmail.Equals(user.Email) && appointment.AppointmentState != Appointment.State.Deleted)
                    doctorAppointment.Add(appointment);
            }
            return doctorAppointment;
        }

        public bool IsAppointmentFreeForDoctor(Appointment newAppointment)
        {
            foreach (Appointment appointment in _appointments)
            {
                if (appointment.DoctorEmail.Equals(newAppointment.DoctorEmail) && appointment.DateAppointment == newAppointment.DateAppointment
                    && CheckOverlapTime(appointment,newAppointment.StartTime, newAppointment.EndTime))
                {
                    Console.WriteLine("Termin je vec zauzet!");
                    return false;
                }
                if ((appointment.PatientEmail.Equals(newAppointment.PatientEmail)) && (appointment.DateAppointment == newAppointment.DateAppointment) && CheckOverlapTime(appointment, newAppointment.StartTime, newAppointment.EndTime))
                {
                    Console.WriteLine("Pacijent vec ima zakazan drugi pregled u ovom terminu!");
                    return false;
                }
                if((appointment.DateAppointment == newAppointment.DateAppointment) && (CheckOverlapTime(appointment, newAppointment.StartTime, newAppointment.EndTime))&& (appointment.RoomNumber.Equals(newAppointment.RoomNumber)))
                {
                    Console.WriteLine("Soba u ovom terminu je zauzeta!");
                    return false;
                }
            }
            return true;
        }

        public void Update(Appointment appointmentChange)
        {
            this._appointments = _appointmentRepository.Load();
            foreach (Appointment updateAppointment in this._appointments)
            {
                if (updateAppointment.AppointmentId.Equals(appointmentChange.AppointmentId))
                {
                    updateAppointment.PatientEmail = appointmentChange.PatientEmail;
                    updateAppointment.DoctorEmail = appointmentChange.DoctorEmail;
                    updateAppointment.DateAppointment = appointmentChange.DateAppointment;
                    updateAppointment.StartTime = appointmentChange.StartTime;
                    updateAppointment.EndTime = appointmentChange.EndTime;
                    updateAppointment.AppointmentState = appointmentChange.AppointmentState;
                    updateAppointment.RoomNumber = appointmentChange.RoomNumber;
                    updateAppointment.AppointmentPerformed = appointmentChange.AppointmentPerformed;
                }
               
            }
            Save();
            
        }

        public void PerformAppointment(Appointment performAppointment)
        {
            foreach (Appointment appointment in this._appointments)
            {
                if (appointment.AppointmentId.Equals(performAppointment.AppointmentId))
                {
                    appointment.AppointmentPerformed = true;
                }
            }

        }

        public User IsDoctorExist(string doctorEmail)
        {
            foreach (User user in _users)
            {
                if (user.Email.Equals(doctorEmail) && user.UserRole == User.Role.Doctor)
                    return user;
            }
            Console.WriteLine("Uneli ste nepostojeceg doktora");
            return null;
        }

        public void AppendNewAppointmentInFile(Appointment newAppointment)
        {
            string filePath = @"..\..\Data\appointments.csv";
            File.AppendAllText(filePath, newAppointment.ToString() + "\n");
        }

        public void Add(Appointment appointment)
        {
            this._appointments.Add(appointment);
            this._appointmentRepository.Save(this._appointments);
        }

        public Room FindFreeRoom(DateTime newDate, DateTime newStartTime)
        {
            RoomService roomService = new RoomService();
            List<Room> freeRooms = roomService.AllRooms;  // at the beginning all the rooms are free

            foreach (Appointment appointment in this._appointments)
            {
                if (appointment.DateAppointment == newDate && newStartTime >= appointment.StartTime
                    && newStartTime < appointment.EndTime && appointment.AppointmentState != Appointment.State.Deleted)
                {
                    Room occupiedRoom = roomService.GetRoomById(appointment.RoomNumber.ToString());
                    freeRooms.Remove(occupiedRoom);
                }
            }

            if (freeRooms.Count == 0)
            {
                Console.WriteLine("\nNe postoji nijedna slobodna soba za unesen termin.");
                return null;
            }
            else
                return freeRooms[0];
        }

        public bool IntervalsOverlap(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
        {
            return firstStart <= secondEnd && secondStart <= firstEnd;
        }

        public bool OverlapingAppointmentExists(DateTime start, DateTime end, string roomId)
        {
            foreach (Appointment appointment in _appointments)
            {
                if (appointment.RoomNumber.Equals(roomId) && IntervalsOverlap(start, end, appointment.StartTime, appointment.EndTime))
                    return true;
            }
            return false;
        }

        public void Save()
        {
            _appointmentRepository.Save(this._appointments);
        }
    }
}
