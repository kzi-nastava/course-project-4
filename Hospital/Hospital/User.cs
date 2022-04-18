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
            BlockedBySystem,
            BlockedBySecretary,
            Active

        }

        private Role role;
        private string username;
        private string password;
        private State state;

        public User(Role role, string username, string password, State state)
        {
            this.role = role;
            this.username = username;
            this.password = password;
            this.state = state;
        }
    }
}
