using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class User
    {
        public enum Role
        {
            Manager = 1,
            Doctor = 2,
            Patient = 3,
            Secretary = 4
        }

        public enum State
        {
            BlockedBySystem = 1,
            BlockedBySecretary = 2,
            Active = 3
        }

        private Role role;
        private string email;
        private string password;
        private State state;

        public Role UserRole { get { return role; } }
        public string Email { get { return email; } }
        public string Password { get { return password; } }
        public State UserState { get { return state; } }

        public User(Role role, string email, string password, State state)
        {
            this.role = role;
            this.email = email;
            this.password = password;
            this.state = state;
        }
    }
}
