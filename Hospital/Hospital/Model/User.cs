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
        private string name;
        private string surname;
        private State state;

        public Role UserRole { get { return role; } }
        public string Email { get { return email; } }
        public string Password { get { return password; }  set { this.password = value; } }
        public string Name { get { return name; } set { this.name = value; } }
        public string Surname { get { return surname; } set { this.surname = value; } }
        public State UserState { get { return state; } set{ this.state = value; } }

        public User(Role role, string email, string password, string name, string surname, State state)
        {
            this.role = role;
            this.email = email;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.state = state;
        }

       
    }
}
