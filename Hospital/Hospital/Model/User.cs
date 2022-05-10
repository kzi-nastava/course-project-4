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

        protected Role _role;
        protected string _email;
        protected string _password;
        protected string _name;
        protected string _surname;
        protected State _state;

        public Role UserRole { get { return _role; } }
        public string Email { get { return _email; } }
        public string Password { get { return _password; }  set { this._password = value; } }
        public string Name { get { return _name; } set { this._name = value; } }
        public string Surname { get { return _surname; } set { this._surname = value; } }
        public State UserState { get { return _state; } set{ this._state = value; } }

        public User(Role role, string email, string password, string name, string surname, State state)
        {
            this._role = role;
            this._email = email;
            this._password = password;
            this._name = name;
            this._surname = surname;
            this._state = state;
        }

       
    }
}
