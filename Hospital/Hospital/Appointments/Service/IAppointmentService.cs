using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Appointments.Model;
using Hospital.Rooms.Model;
using Hospital.Users.Model;

namespace Hospital.Appointments.Service
{
    public interface IAppointmentService: IService<Appointment>
    {
        void Save();
        bool OverlapingAppointmentExists(DateTime start, DateTime end, string roomId);
        bool IntervalsOverlap(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd);
        Room FindFreeRoom(DateTime newDate, DateTime newStartTime);
        void Add(Appointment appointment);
        void AppendNewAppointmentInFile(Appointment newAppointment);

        User IsDoctorExist(string doctorEmail);

        void PerformAppointment(Appointment performAppointment);

        void Update(Appointment appointmentChange);

        bool IsAppointmentFreeForDoctor(Appointment newAppointment);
        List<Appointment> GetDoctorAppointment(User user);

        User FindFreeDoctor(DoctorUser.Speciality speciality, Appointment newAppointment);

        Appointment FindLeastUrgentAppointment();

        List<Appointment> GetFilteredSortedAppointments();

        bool CheckOverlapTime(Appointment appointment, DateTime startTime, DateTime endTime);

        bool IsDoctorFree(User doctor, DateTime startTime);

        Appointment CreateNewAppointment(User patient, User doctor, DateTime startingTime, int appointmentType);

        int GetNewAppointmentId();


    }
}
