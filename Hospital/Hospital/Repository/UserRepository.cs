using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    public class UserRepository
    {
        public List<User> Load()
        {
            List<User> users = new List<User>();

            using (TextFieldParser parser = new TextFieldParser(@"Data\users.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    
                    User.Role role = (User.Role)int.Parse(fields[0]);
                    string username = fields[1];
                    string password = fields[2];
                    User.State state = (User.State)int.Parse(fields[3]);
                    
                    User user = new User(role, username, password, state);
                    users.Add(user);
                }
            }

            return users;
        }
    }
}
