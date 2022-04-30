using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    public class RoomRepository
    {
        private static string s_filePath = @"..\..\Data\rooms.csv";

        public List<Room> Load()
        {
            List<Room> allRooms = new List<Room>();

            using (TextFieldParser parser = new TextFieldParser(s_filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string id = fields[0];
                    string name = fields[1];
                    Room.Type type = (Room.Type)int.Parse(fields[2]);
                    
                    Room room = new Room(id, name, type);
                    allRooms.Add(room);
                }
            }

            return allRooms;
        }

        public void Save(List<Room> allRooms)
        {
            string[] lines = new string[allRooms.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                Room room = allRooms[i];
                lines[i] = room.Id + "," + room.Name + "," + ((int)room.RoomType).ToString();
            }
            
            File.WriteAllLines(s_filePath, lines);
        }
    }
}
