using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    public class RoomRepository
    {
        public List<Room> Load()
        {
            List<Room> allRooms = new List<Room>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\rooms.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string id = fields[0];
                    string name = fields[1];
                    Room.TypeOfRoom type = (Room.TypeOfRoom)int.Parse(fields[2]);
                    
                    Room room = new Room(id, name, type);
                    allRooms.Add(room);
                }
            }

            return allRooms;
        }
    }
}
