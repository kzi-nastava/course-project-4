using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

using Hospital.Users.Model;

namespace Hospital.Users.Repository
{
    public class RequestForDaysOffRepository
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

        public void Save(List<RequestForDaysOff> requestsForDaysOff)
        {
            string filePath = @"..\..\Data\requestsForDaysOff.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (RequestForDaysOff request in requestsForDaysOff)
            {
                line = request.Id + ";" + request.EmailDoctor + ";" + request.StartDate.ToString("MM/dd/yyyy") +
                    ";" + request.EndDate.ToString("MM/dd/yyyy") + ";" + request.ReasonRequired + ";" + (int)request.StateRequired + ";" + request.Urgen.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
