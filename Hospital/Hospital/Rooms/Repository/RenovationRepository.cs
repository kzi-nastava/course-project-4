using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public class RenovationRepository
    {
        private static string s_filePath = @"..\..\Data\renovations.csv";
        private List<Renovation> _allRenovations;

        public RenovationRepository()
        {
            _allRenovations = Load();
        }

        public List<Renovation> AllRenovations { get { return _allRenovations; } }

        public bool IdExists(string id)
        {
            foreach (Renovation renovation in _allRenovations)
            {
                if (renovation.Id.Equals(id))
                    return true;
            }
            return false;
        }

        public void CreateRenovation(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type)
        {
            Renovation renovation = new Renovation(id, startDate, endDate, roomId, true, type);
            _allRenovations.Add(renovation);
            Save(_allRenovations);
        }

        public void CreateSimpleRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            CreateRenovation(id, startDate, endDate, roomId, Renovation.Type.SimpleRenovation);
        }

        public void CreateSplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            CreateRenovation(id, startDate, endDate, roomId, Renovation.Type.SplitRenovation);
        }

        public void CreateMergeRenovation(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId)
        {
            Renovation renovation = new MergeRenovation(id, startDate, endDate, roomId, true, otherRoomId);
            _allRenovations.Add(renovation);
            Save(_allRenovations);
        }

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
                    Renovation.Type type = (Renovation.Type)int.Parse(fields[5]);

                    Renovation renovation;

                    if (type == Renovation.Type.MergeRenovation)
                    {
                        string otherRoomId = fields[6];
                        renovation = new MergeRenovation(id, startDate, endDate, roomId, active, otherRoomId);
                    }
                    else if (type == Renovation.Type.SplitRenovation)
                    {
                        renovation = new SplitRenovation(id, startDate, endDate, roomId, active);
                    }
                    else
                    {
                        renovation = new Renovation(id, startDate, endDate, roomId, active, type);
                    }
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
                    + renovation.IsActive.ToString() + "," + (int)renovation.RenovationType + ",";

                if (renovation.RenovationType == Renovation.Type.MergeRenovation)
                    lines[i] += ((MergeRenovation)renovation).OtherRoomId;
                else
                    lines[i] += "null";
            }

            File.WriteAllLines(s_filePath, lines);
        }
    }
}
