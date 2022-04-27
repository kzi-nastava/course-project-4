using System;
using System.Collections.Generic;
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

        public UserService()
        {
            userRepository = new UserRepository();
            users = userRepository.Load();
        }

        
    }
}
