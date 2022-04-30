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
        private EquipmentRepository equipmentRepository;
        private RoomService roomService;
        private List<Equipment> allEquipment;

        public List<Equipment> AllEquipment { get { return allEquipment; } }

        public EquipmentService(RoomService roomService)
        {
            this.roomService = roomService;
            equipmentRepository = new EquipmentRepository();
            allEquipment = equipmentRepository.Load();
        }

        public List<Equipment> Search(string query)
        {
            List<Equipment> answer = new List<Equipment>();
            query = query.ToLower();
            foreach (Equipment equipment in allEquipment)
            {
                if (equipment.Name.ToLower().Contains(query) || equipment.Id.ToLower().Contains(query))
                    answer.Add(equipment);
            }
            return answer;
        }

        public List<Equipment> FilterByRoomType(Room.TypeOfRoom roomType)
        {
            List<Equipment> answer = new List<Equipment>();
            foreach (Equipment equipment in allEquipment)
            {
                Room room = roomService.GetRoomById(equipment.RoomId);
                if (room.Type == roomType)
                    answer.Add(equipment);
            }
            return answer;
        }

        public List<Equipment> FilterByQuantity(int lowerBound, int upperBound) // If bound is set to -1 it is ignored
        {
            List<Equipment> answer = new List<Equipment>();
            foreach (Equipment equipment in allEquipment)
            {
                if (lowerBound != -1 && lowerBound > equipment.Quantity)
                    continue;
                if (upperBound != -1 && upperBound < equipment.Quantity)
                    continue;
                answer.Add(equipment);
            }
            return answer;
        }

        public List<Equipment> FilterByEquipmentType(Equipment.TypeOfEquipment equipmentType)
        {
            List<Equipment> answer = new List<Equipment>();
            foreach (Equipment equipment in allEquipment)
            {
                if (equipment.Type == equipmentType)
                    answer.Add(equipment);
            }
            return answer;
        }
    }
}
