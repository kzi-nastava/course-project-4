using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

using Hospital.Users.Model;

namespace Hospital.Users.Repository
{
    public class UserActionRepository
    {
        private string _userEmail;

        public UserActionRepository(string userEmail)
        {
            this._userEmail = userEmail;
        }

        public List<UserAction> Load()
        {
            List<UserAction> allActions = new List<UserAction>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\actions.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string patientEmail = fields[0];
                    UserAction.State state = (UserAction.State)int.Parse(fields[1]);
                    DateTime actionDate = DateTime.ParseExact(fields[2], "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    UserAction action = new UserAction(patientEmail, actionDate, state);
                    allActions.Add(action);
                }
            }
            return allActions;
        }

        public void BlockAccessApplication()
        {
            // read from file
            string filePath = @"..\..\Data\users.csv";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string userEmailFromFile = fields[1];
                if (_userEmail.Equals(userEmailFromFile))
                    lines[i] = fields[0] + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4]
                        + "," + (int)User.State.BlockedBySystem + "," + "null";
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }

        public void AppendToActionFile(string typeAction)
        {
            string filePath = @"..\..\Data\actions.csv";

            UserAction newAction;
            if (typeAction.Equals("create"))
                newAction = new UserAction(_userEmail, DateTime.Now, UserAction.State.Created);
            else if (typeAction.Equals("update"))
                newAction = new UserAction(_userEmail, DateTime.Now, UserAction.State.Modified);
            else
                newAction = new UserAction(_userEmail, DateTime.Now, UserAction.State.Deleted);

            File.AppendAllText(filePath, newAction.ToString());
        }
    }
}
