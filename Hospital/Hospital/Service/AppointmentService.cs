using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    public class AppointmentService
    {
        private AppointmentRepository _appointmentRepository;
        private UserService _userService;
        private NotificationService _notificationService;
        private List<Appointment> _appointments;
        private List<User> _users;
        private RoomRepository _roomRepository;
        private List<Room> _rooms;

        public AppointmentRepository AppointmentRepository { get { return _appointmentRepository; } }
        public List<Appointment> Appointments { get { return _appointments; } }
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

        public bool IsDoctorFree(User doctor, DateTime startTime)
		{
            foreach(Appointment appointment in _appointments)
			{
                if (appointment.DoctorEmail == doctor.Email && appointment.DateAppointment.Date == DateTime.Now.Date
                    && CheckOverlapTime(appointment, startTime))
                    return false;
			}
            return true;
		}

        public bool CheckOverlapTime(Appointment appointment, DateTime startTime)
		{
            DateTime endTime = startTime.AddMinutes(60);
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

  //      public List<Appointment> FilterNonUrgentAppointments()
		//{
  //          List<Appointment> appointments = new List<Appointment>();
  //          foreach(Appointment appointment in _appointments)
		//	{
  //              Console.WriteLine(appointment.DateAppointment);
		//		if (!appointment.Urgent && appointment.DateAppointment.Date >= DateTime.Now.Date)
		//		{
  //                  if (appointment.DateAppointment.Date == DateTime.Now.Date)
  //                      if (appointment.StartTime.TimeOfDay < DateTime.Now.TimeOfDay)
  //                          continue;
  //                  appointments.Add(appointment);
  //                  Console.WriteLine(appointment);
		//		}
		//	}
  //          return appointments;
		//}

  //      public void FindLeastUrgentAppointment()
		//{
  //          List<Appointment> nonUrgentAppointments = FilterNonUrgentAppointments();
  //          nonUrgentAppointments.Sort(delegate (Appointment x, Appointment y) { return x.StartTime.CompareTo(y.StartTime); });
  //          foreach(Appointment app in nonUrgentAppointments)
		//	{
                
  //              Console.WriteLine(app.PatientEmail + " " + app.DoctorEmail + " " +app.DateAppointment + " " + app.StartTime);
		//	}
		//}

        public void ScheduleUrgently(User patient, DoctorUser.Speciality speciality, int appointmentType)
		{
            List<User> capableDoctors = _userService.FilterDoctors(speciality);
            DateTime currentTime = DateTime.Now.AddMinutes(15);
            DateTime gapTime = DateTime.Now.AddHours(2);

            while(currentTime <= gapTime)
			{
                foreach(User doctor in capableDoctors)
				{
                    if(IsDoctorFree(doctor, currentTime))
					{
                        Room freeRoom = FindFreeRoom(currentTime, currentTime);
                        DateTime endTime;
                        if (appointmentType == 1)
                            endTime = currentTime.AddMinutes(15);
                        else
                            endTime = currentTime.AddMinutes(60);
                        Appointment newAppointment = new Appointment(GetNewAppointmentId().ToString(), patient.Email, doctor.Email,
                            currentTime, currentTime, endTime, Appointment.State.Created,
                            Int32.Parse(freeRoom.Id), (Appointment.Type)appointmentType, false, true);
                        _appointments.Add(newAppointment);
                        this.UpdateFile();
                        Console.WriteLine("\nUspesno obavljeno hitno zakazivanje\nSlanje obavestenja izabranom lekaru...");
                        
                        //TODO citanje notifikacije
                        Notification notification = new Notification(_notificationService.GetNewNotificationId(), doctor.Email,
                            "Imate hitno zakazan termin u " + currentTime.ToString("HH:mm"), false);
                        _notificationService.Notifications.Add(notification);
                        _notificationService.UpdateFile();
                        return;
					}
				}
                currentTime = currentTime.AddMinutes(15);
			}
            //TODO  SVE ZAUZETO

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

            //returns all _appointments for a particular doctor
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
                    && CheckOverlapTerms(appointment,newAppointment))
                {
                    Console.WriteLine("Termin je vec zauzet!");
                    return false;
                }
                if ((appointment.PatientEmail.Equals(newAppointment.PatientEmail)) && (appointment.DateAppointment == newAppointment.DateAppointment) && CheckOverlapTerms(appointment, newAppointment))
                {
                    Console.WriteLine("Pacijent vec ima zakazan drugi pregled u ovom terminu!");
                    return false;
                }
                if((appointment.DateAppointment == newAppointment.DateAppointment) && (CheckOverlapTerms(appointment, newAppointment))&& (appointment.RoomNumber.Equals(newAppointment.RoomNumber)))
                {
                    Console.WriteLine("Soba u ovom terminu je zauzeta!");
                    return false;
                }
            }
            return true;
        }

        public bool CheckOverlapTerms(Appointment appointment, Appointment newAppointment)
        {
            if((newAppointment.StartTime<= appointment.StartTime) && (appointment.EndTime <= newAppointment.EndTime))
            {
                return true;
            }else if ((appointment.StartTime <= newAppointment.EndTime) && (newAppointment.EndTime <= appointment.EndTime))
            {
                return true;
            }else if((appointment.StartTime<=newAppointment.StartTime) && (newAppointment.StartTime <= appointment.EndTime)){
                return true;
            }
            else if((appointment.StartTime <= newAppointment.StartTime) && (newAppointment.EndTime <= appointment.EndTime))
            {
                return true;
            }
            return false;
        }
        public bool IsPatientEmailValid(string patientEmail)
        {
            foreach(User user in _users)
            {
                if((user.Email == patientEmail) && (user.UserRole == User.Role.Patient) && user.UserState == User.State.Active)
                {
                    return true;
                }
            }
            Console.WriteLine("Pacijent ne postoji!");
            return false;
        }

        public bool IsDateFormValid(string date)
        {
            DateTime checkDate;
            bool validDate = DateTime.TryParseExact(date, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out checkDate);
            if (!validDate)
            {
                Console.WriteLine("Nevalidan unos datuma");
                return false;
            }
            else if (checkDate <= DateTime.Now)
            {
                Console.WriteLine("Uneli ste datum koji je prosao ili je danasnji.");
                return false;
            }
            return true;
        }

        public bool IsTimeFormValid(string time)
        {
            TimeSpan checkTime;
            bool validTime = TimeSpan.TryParse(time, out checkTime);
            if (!validTime)
            {
                Console.WriteLine("Nevalidan unos vremena");
                return false;
            }
            return true;
        }
       
        public bool IsRoomNumberValid(string roomNumber)
        {
            foreach(Room room in _rooms)
            {
                if (room.Id.Equals(roomNumber))
                {
                    return true;
                }
            }
            Console.WriteLine("Broj sobe ne postoji!");
            return false;

        }

        public bool IsIntegerValid(string number)
        {
            bool isNumeric = true;
            foreach (char c in number)
            {
                if (!Char.IsNumber(c))
                {
                    isNumeric = false;
                    Console.WriteLine("Unesite validnu radnju!");
                    break;
                }
            }
            return isNumeric;
        }

        public bool IsDoubleValid(string number)

        {
            if (double.TryParse(number, out double doubleNumber) && !Double.IsNaN(doubleNumber) && !Double.IsInfinity(doubleNumber))
            {
                return true;

            }
            return false;
        }

        public void DeleteAppointment(Appointment appointmentForDelete)
        {
            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);
           
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(appointmentForDelete.AppointmentId))
                {

                    lines[i] = id + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," + fields[5]
                        + "," + (int)Appointment.State.Deleted + "," + fields[7] + "," + fields[8] + "," + fields[9]+","+fields[10];
                    Console.WriteLine("Uspesno ste obrisali termin!");
                    
                }
            }
            // saving changes
            File.WriteAllLines(filePath, lines);

            // update list with all appointments

            foreach(Appointment appointment in this._appointments)
            {
                if (appointment.AppointmentId.Equals(appointmentForDelete.AppointmentId))
                {
                    appointment.AppointmentState = Appointment.State.Deleted;
                }
            }
        }

        public void UpdateAppointment(Appointment appointmentChange)
        {
            foreach(Appointment updateAppointment in this._appointments)
            {
                if (updateAppointment.AppointmentId.Equals(appointmentChange.AppointmentId))
                {
                    updateAppointment.PatientEmail = appointmentChange.PatientEmail;
                    updateAppointment.DoctorEmail = appointmentChange.DoctorEmail;
                    updateAppointment.DateAppointment = appointmentChange.DateAppointment;
                    updateAppointment.StartTime = appointmentChange.StartTime;
                    updateAppointment.EndTime = appointmentChange.EndTime;
                    updateAppointment.AppointmentState = Appointment.State.Updated;
                    updateAppointment.RoomNumber = appointmentChange.RoomNumber;
                    updateAppointment.AppointmentPerformed = appointmentChange.AppointmentPerformed;
                }
               
            }
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

        public void UpdateFile()
		{
            string filePath = @"..\..\Data\appointments.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (Appointment appointment in _appointments)
            {
                line = appointment.AppointmentId + "," + appointment.PatientEmail + "," + appointment.DoctorEmail + "," + appointment.DateAppointment.ToString("MM/dd/yyyy") +
                    "," + appointment.StartTime.ToString("HH:mm") + "," + appointment.EndTime.ToString("HH:mm") + "," +
                    (int)appointment.AppointmentState + "," + appointment.RoomNumber + "," + (int)appointment.TypeOfTerm + "," +  appointment.AppointmentPerformed + "," + appointment.Urgent;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }

        public void RemakePerformedAppointment(Appointment remakeAppointment)
        {
            string filePath = @"..\..\Data\appointments.csv";
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(remakeAppointment.AppointmentId))
                {

                    lines[i] = id + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," + fields[5]
                        + "," + (int)Appointment.State.Updated + "," + fields[7] + "," + fields[8] + ",true,"+ fields[10];
                }
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
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

        public void TableHeaderForPatient()
        {
            Console.WriteLine();
            Console.Write(String.Format("{0,3}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}|{7,10}",
                "Br.", "Doktor", "Datum", "Pocetak", "Kraj", "Soba", "Tip", "Stanje"));
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
    }
}
