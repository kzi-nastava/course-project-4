using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    public class UserService
    {
        private UserRepository _userRepository;
        private List<User> _users;

        public List<User> Users { get { return this._users; } }

        public bool IsEmailValid(string email)
        {
            foreach (User user in _users)
            {
                if (user.Email == email)
                    return true;
            }
            return false;
        }


        public void AddUser(User user)
        {
            this._users.Add(user);
            this._userRepository.Save(this._users);
        }
        public List<DoctorUser> GetDoctors()
        {
            List<DoctorUser> doctors = new List<DoctorUser>();
            foreach (User user in _users)
            {
                if (user.UserRole.Equals(User.Role.Doctor))
                {
                    doctors.Add((DoctorUser)user);
                }
            }
            return doctors;
        }
        public bool IsUserBlocked(string email)
        {
            foreach (User user in _users)
            {
                if (user.Email == email && user.UserState != User.State.Active)
                    return true;
            }
            return false;
        }

        public string GetUserFullName(string email)
		{
            string fullName = "";
            foreach(User user in _users)
			{
                if(user.Email == email)
				{
                    fullName = user.Name + " " + user.Surname;
				}
			}
            return fullName;
		}

        public User TryLogin(string email, string password)
        {
            foreach (User user in _users)
            {
                if ((user.Email == email) && (user.Password == password))
                    return user;
            }
            return null;
        }

        public void BlockOrUnblockUser(User forUpdate, bool blocking)
		{
            foreach(User user in _users)
			{
                if(user.Email == forUpdate.Email)
				{
					if (blocking)
					{
                        user.UserState = User.State.BlockedBySecretary;
                        break;
					}
					else
					{
                        user.UserState = User.State.Active;
                        break;
					}
                    
				}
			}

            UpdateFile();
		}

        public void UpdateUserInfo(User forUpdate)
		{
            for(int i = 0; i < _users.Count; i++)
			{
                User user = Users[i];
                if (user.Email == forUpdate.Email){
                    _users[i] = forUpdate;
                    break;
				}
			}
            UpdateFile();
		}

        public List<User> FilterDoctors(DoctorUser.Speciality speciality)
		{
            List<User> allDoctors = new List<User>();
            foreach(User user in _users)
			{
                if(user.UserRole == User.Role.Doctor)
				{
                    DoctorUser doctor = (DoctorUser)user;
                    if(doctor.SpecialityDoctor == speciality)
					{
                        allDoctors.Add(user);
					}
                    
				}
			}
            return allDoctors;
		}

        public UserService()
        {
            _userRepository = new UserRepository();
            _users = _userRepository.Load();
        }

        public void DisplayOfPatientData(string patientEmail)
        {
            foreach (User user in this._users)
            {
                if (user.Email.Equals(patientEmail))
                {
                    Console.WriteLine("Ime: " + user.Name + "\n" + "Prezime: " + user.Surname + "\n");

                }
            }

        }

        private void UpdateFile()
        {
            this._userRepository.Save(this._users);
        }

        public bool IsPatientEmailValid(string patientEmail)
        {
            foreach (User user in _users)
            {
                if ((user.Email == patientEmail) && (user.UserRole == User.Role.Patient) && user.UserState == User.State.Active)
                {
                    return true;
                }
            }
            Console.WriteLine("Pacijent ne postoji!");
            return false;
        }
    }
}
