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
            foreach(User user in users)
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

        public void UpdateFile()
		{
            string filePath = @"..\..\Data\_users.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (User user in _users)
            {
                int role = (int)user.UserRole;
                int state = (int)user.UserState;
                line = role.ToString() + "," + user.Email + "," + user.Password + "," + user.Name + "," + user.Surname + "," + state.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
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

        public UserService()
        {
            _userRepository = new UserRepository();
            _users = _userRepository.Load();
        }
    }
}
