using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital;
using Autofac;
using Hospital.Rooms.Repository;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public class EquipmentService : IEquipmentService
    {
        private IEquipmentRepository _equipmentRepository;
        private IRoomService _roomService;

        public List<Equipment> AllEquipment { get { return _equipmentRepository.AllEquipment; } }

        public EquipmentService()
        {
            _roomService = Globals.container.Resolve<IRoomService>();
            _equipmentRepository = Globals.container.Resolve<IEquipmentRepository>();
        }

        public Equipment GetEquipmentById(string id)
        {
            return _equipmentRepository.GetEquipmentById(id);
        }

        public bool IdExist(string id)
        {
            return GetEquipmentById(id) != null;
        }

        public bool CreateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId)
        {
            if (IdExist(id))
                return false;
            _equipmentRepository.CreateEquipment(id, name, type, quantity, roomId);
            return true;
        }

        public bool UpdateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId)
        {
            if (!IdExist(id))
                return false;
            _equipmentRepository.UpdateEquipment(id, name, type, quantity, roomId);
            return true;
        }

        public bool DeleteEquipment(string id)
        {
            if (!IdExist(id))
                return false;
            _equipmentRepository.DeleteEquipment(id);
            return true;
        }

        public List<Equipment> Search(string query)
        {
            return _equipmentRepository.Search(query);
        }

        public List<Equipment> FilterByRoomType(Room.Type roomType)
        {
            return _equipmentRepository.FilterByRoomType(roomType);
        }

        public List<Equipment> FilterByQuantity(int lowerBound, int upperBound) // If bound is set to -1 it is ignored
        {
            return _equipmentRepository.FilterByQuantity(lowerBound, upperBound);
        }

        public List<Equipment> FilterByEquipmentType(Equipment.Type equipmentType)
        {
            return _equipmentRepository.FilterByEquipmentType(equipmentType);
        }

        public void ChangeRoom(string roomBefore, string roomAfter)
        {
            _equipmentRepository.ChangeRoom(roomBefore, roomAfter);
        }
    }
}
