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
        private UserRepository userRepository;
        private List<User> users;

        public List<User> Users { get { return this.users; } }

        public bool IsEmailValid(string email)
        {
            foreach (User user in users)
            {
                if (user.Email == email)
                    return true;
            }
            return false;
        }

        public bool IsUserBlocked(string email)
        {
            foreach (User user in users)
            {
                if (user.Email == email && user.UserState != User.State.Active)
                    return true;
            }
            return false;
        }

        public User TryLogin(string email, string password)
        {
            foreach (User user in users)
            {
                if ((user.Email == email) && (user.Password == password))
                    return user;
            }
            return null;
        }

        public void UpdateUserFile()
		{
            string filePath = @"..\..\Data\users.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (User user in users)
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
            foreach(User user in users)
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

            UpdateUserFile();

		}

        public void UpdateUserInfo(User forUpdate)
		{
            for(int i = 0; i < users.Count; i++)
			{
                User user = Users[i];
                if (user.Email == forUpdate.Email){
                    users[i] = forUpdate;
                    break;
				}
			}
            UpdateUserFile();
		}

        public UserService()
        {
            userRepository = new UserRepository();
            users = userRepository.Load();
        }

        
    }
}
