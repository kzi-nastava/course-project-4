using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Rooms.Model;
using Hospital.Rooms.Service;

namespace Hospital.Rooms.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private static string s_filePath = @"..\..\Data\equipment.csv"; 
        private IRoomService _roomService;
        private List<Equipment> _allEquipment;

        public EquipmentRepository(IRoomService roomService)
        {
            _roomService = roomService;
            _allEquipment = Load();
        }

        public List<Equipment> AllEquipment { get { return _allEquipment; } }

        public Equipment GetEquipmentById(string id)
        {
            foreach (Equipment equipment in _allEquipment)
            {
                if (equipment.Id.Equals(id))
                    return equipment;
            }
            return null;
        }

        public void CreateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId)
        {
            Equipment equipment = new Equipment(id, name, type, quantity, roomId);
            _allEquipment.Add(equipment);
            Save(_allEquipment);
        }

        public void UpdateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId)
        {
            DeleteEquipment(id);
            CreateEquipment(id, name, type, quantity, roomId);
        }

        public void DeleteEquipment(string id)
        {
            Equipment equipment = GetEquipmentById(id);
            _allEquipment.Remove(equipment);
            Save(_allEquipment);
        }

        public List<Equipment> Search(string query)
        {
            List<Equipment> answer = new List<Equipment>();
            query = query.ToLower();
            foreach (Equipment equipment in _allEquipment)
            {
                if (equipment.Name.ToLower().Contains(query) || equipment.Id.ToLower().Contains(query)
                    || equipment.TypeDescription.ToLower().Contains(query))
                    answer.Add(equipment);
            }
            return answer;
        }

        public List<Equipment> FilterByRoomType(Room.Type roomType)
        {
            List<Equipment> answer = new List<Equipment>();
            foreach (Equipment equipment in _allEquipment)
            {
                Room room = _roomService.GetRoomById(equipment.RoomId);
                if (room.RoomType == roomType)
                    answer.Add(equipment);
            }
            return answer;
        }

        public List<Equipment> FilterByQuantity(int lowerBound, int upperBound) // If bound is set to -1 it is ignored
        {
            List<Equipment> answer = new List<Equipment>();
            foreach (Equipment equipment in _allEquipment)
            {
                if (lowerBound != -1 && lowerBound > equipment.Quantity)
                    continue;
                if (upperBound != -1 && upperBound < equipment.Quantity)
                    continue;
                answer.Add(equipment);
            }
            return answer;
        }

        public List<Equipment> FilterByEquipmentType(Equipment.Type equipmentType)
        {
            List<Equipment> answer = new List<Equipment>();
            foreach (Equipment equipment in _allEquipment)
            {
                if (equipment.EquipmentType == equipmentType)
                    answer.Add(equipment);
            }
            return answer;
        }

        public void ChangeRoom(string roomBefore, string roomAfter)
        {
            foreach (Equipment equipment in _allEquipment)
            {
                if (equipment.RoomId.Equals(roomBefore))
                    equipment.RoomId = roomAfter;
            }
            Save(_allEquipment);
        }

        public List<Equipment> Load()
        {
            List<Equipment> allEquipment = new List<Equipment>();

            using (TextFieldParser parser = new TextFieldParser(s_filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string id = fields[0];
                    string name = fields[1];
                    Equipment.Type type = (Equipment.Type)int.Parse(fields[2]);
                    int quantity = int.Parse(fields[3]);
                    string roomId = fields[4];

                    Equipment equipment = new Equipment(id, name, type, quantity, roomId);
                    allEquipment.Add(equipment);
                }
            }

            return allEquipment;
        }

        public void Save(List<Equipment> allEquipment)
        {
            string[] lines = new string[allEquipment.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                Equipment equipment = allEquipment[i];
                lines[i] = equipment.Id + "," + equipment.Name + "," + ((int)equipment.EquipmentType).ToString() + "," 
                    + equipment.Quantity.ToString() + "," + equipment.RoomId;
            }

            File.WriteAllLines(s_filePath, lines);
        }
    }
}
