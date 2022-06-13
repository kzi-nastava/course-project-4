using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Users.Model;
using Hospital.Users.View;

namespace Hospital.Users.Repository
{
    public class UserRepository
    {
        private List<User> users = new List<User>();
        private List<DoctorUser> doctorUsers = new List<DoctorUser>();

        public List<DoctorUser> DoctorUsers { get { return doctorUsers; } set { doctorUsers = value; } }

        public List<User> Load()
        {
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\users.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    
                    User.Role role = (User.Role)int.Parse(fields[0]);
                    string email = fields[1];
                    string password = fields[2];
                    string name = fields[3];
                    string surname = fields[4];
                    User.State state = (User.State)int.Parse(fields[5]);

                    User user;
                    if (!fields[6].Equals("null"))
                    {
                        DoctorUser.Speciality speciality = (DoctorUser.Speciality)int.Parse(fields[6]);
                        user = new DoctorUser(role, email, password, name, surname, state, speciality);
                        DoctorUser specialist = new DoctorUser(role, email, password, name, surname, state, speciality);
                        this.doctorUsers.Add(specialist);
                    }
                    else
                    {
                        user = new User(role, email, password, name, surname, state);
                    }
                    this.users.Add(user);
                }
            }
            return this.users;
        }

        public void Save(List<User> users)
        {
            string filePath = @"..\..\Data\users.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (User user in users)
            {
                int role = (int)user.UserRole;
                int state = (int)user.UserState;
                if (user.UserRole.Equals(User.Role.Doctor))
                {
                    DoctorUser doctorUser = (DoctorUser)user;
                    line = role.ToString() + "," + user.Email + "," + user.Password + "," + user.Name + "," + user.Surname + "," + state.ToString() + "," + (int)doctorUser.SpecialityDoctor;
                    lines.Add(line);
                }
                else
                {
                    line = role.ToString() + "," + user.Email + "," + user.Password + "," + user.Name + "," + user.Surname + "," + state.ToString() + ",null";
                    lines.Add(line);
                }

            }
            File.WriteAllLines(filePath, lines.ToArray());
        }

    }
}
