using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public class RoomRepository
    {
        private static string s_filePath = @"..\..\Data\rooms.csv";
        private List<Room> _allRooms;

        public RoomRepository()
        {
            _allRooms = Load();
        }

        public List<Room> AllRooms { get { return _allRooms; } }

        public Room GetRoomById(string id)
        {
            foreach (Room room in _allRooms)
            {
                if (room.Id == id)
                    return room;
            }
            return null;
        }

        public void CreateRoom(string id, string name, Room.Type type)
        {
            Room room = new Room(id, name, type, false);
            _allRooms.Add(room);
            Save(_allRooms);
        }

        public void UpdateRoom(string id, string name, Room.Type type)
        {
            DeleteRoom(id);
            CreateRoom(id, name, type);
        }

        public void DeleteRoom(string id)
        {
            foreach (Room room in _allRooms)
            {
                if (room.Id.Equals(id))
                    room.IsDeleted = true;
            }
            Save(_allRooms);
        }

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
                    bool deleted = bool.Parse(fields[3]);
                    
                    Room room = new Room(id, name, type, deleted);
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
                lines[i] = room.Id + "," + room.Name + "," + ((int)room.RoomType).ToString() + "," + room.IsDeleted;
            }
            
            File.WriteAllLines(s_filePath, lines);
        }
    }
}
