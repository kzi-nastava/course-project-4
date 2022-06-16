using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        List<Equipment> AllEquipment { get; }

        Equipment GetEquipmentById(string id);

        void CreateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId);

        void UpdateEquipment(string id, string name, Equipment.Type type, int quantity, string roomId);

        void DeleteEquipment(string id);

        List<Equipment> Search(string query);

        List<Equipment> FilterByRoomType(Room.Type roomType);

        List<Equipment> FilterByQuantity(int lowerBound, int upperBound); 

        List<Equipment> FilterByEquipmentType(Equipment.Type equipmentType);

        void ChangeRoom(string roomBefore, string roomAfter);
    }
}
