using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    public class EquipmentService
    {
        private EquipmentRepository _equipmentRepository;
        private RoomService _roomService;
        private List<Equipment> _allEquipment;

        public List<Equipment> AllEquipment { get { return _allEquipment; } }

        public EquipmentService(RoomService roomService)
        {
            this._roomService = roomService;
            _equipmentRepository = new EquipmentRepository();
            _allEquipment = _equipmentRepository.Load();
        }

        public Equipment GetEquipmentById(string id)
        {
            foreach (Equipment equipment in _allEquipment)
            {
                if (equipment.Id.Equals(id))
                    return equipment;
            }
            return null;
        }

        public bool IdExist(string id)
        {
            return GetEquipmentById(id) != null;
        }

        public bool CreateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId)
        {
            if (IdExist(id))
                return false;
            Equipment equipment = new Equipment(id, name, type, quantity, roomId);
            _allEquipment.Add(equipment);
            _equipmentRepository.Save(_allEquipment);
            return true;
        }

        public bool UpdateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId)
        {
            if (!IdExist(id))
                return false;
            DeleteEquipment(id);
            CreateEquipment(id, name, type, quantity, roomId);
            return true;
        }

        public bool DeleteEquipment(string id)
        {
            if (!IdExist(id))
                return false;
            Equipment equipment = GetEquipmentById(id);
            _allEquipment.Remove(equipment);
            _equipmentRepository.Save(_allEquipment);
            return true;
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
            _equipmentRepository.Save(_allEquipment);
        }
    }
}
