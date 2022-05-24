using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using System.Globalization;

namespace Hospital.Repository
{
    class RequestForDaysOffRepository
    {
        public List<RequestForDaysOff> Load()
        {
            List<RequestForDaysOff> requirements = new List<RequestForDaysOff>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\requestsForDaysOff.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    string emailDoctor = fields[1];
                    DateTime startDate = DateTime.ParseExact(fields[2], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(fields[3], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string reasonRequired = fields[4];
                    RequestForDaysOff.State state = (RequestForDaysOff.State)int.Parse(fields[5]);
                    bool urgent = bool.Parse(fields[6]);

                    RequestForDaysOff requestForDays = new RequestForDaysOff(id,emailDoctor, startDate, endDate, reasonRequired, state, urgent);
                    requirements.Add(requestForDays);
                }
            }

            return requirements;
        }
    }
}
