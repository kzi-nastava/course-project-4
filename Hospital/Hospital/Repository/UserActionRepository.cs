using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    class UserActionRepository
    {
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
    }
}
