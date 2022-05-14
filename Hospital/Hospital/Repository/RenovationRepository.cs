using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    public class RenovationRepository
    {
        private static string s_filePath = @"..\..\Data\renovations.csv";

        public List<Renovation> Load()
        {
            List<Renovation> allRenovations = new List<Renovation>();

            using (TextFieldParser parser = new TextFieldParser(s_filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string id = fields[0];
                    DateTime startDate = DateTime.ParseExact(fields[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(fields[2], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string roomId = fields[3];
                    bool active = bool.Parse(fields[4]);

                    Renovation renovation = new Renovation(id, startDate, endDate, roomId, active);
                    allRenovations.Add(renovation);
                }
            }

            return allRenovations;
        }

        public void Save(List<Renovation> allRenovations)
        {
            string[] lines = new string[allRenovations.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                Renovation renovation = allRenovations[i];
                lines[i] = renovation.Id + "," + renovation.StartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ","
                    + renovation.EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "," + renovation.RoomId + ","
                    + renovation.IsActive.ToString();
            }

            File.WriteAllLines(s_filePath, lines);
        }
    }
}
