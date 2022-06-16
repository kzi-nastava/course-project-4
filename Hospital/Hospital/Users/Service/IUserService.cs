using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;
using Hospital.Users.Repository;

namespace Hospital.Users.Service
{
    public interface IUserService
    {
        IUserRepository UserRepository { get; }
        List<User> Users { get; }
        List<User> AllDoctors();
        bool IsPatientEmailValid(string patientEmail);
        void UpdateFile();
        void DisplayOfPatientData(string patientEmail);
        List<User> FilterDoctors(DoctorUser.Speciality speciality);
        void UpdateUserInfo(User forUpdate);
        void BlockOrUnblockUser(User forUpdate, bool blocking);
        User TryLogin(string email, string password);
        string GetUserFullName(string email);
        bool IsUserBlocked(string email);
        List<DoctorUser> GetDoctors();
        void Add(User user);
        bool IsEmailValid(string email);

    }
}
