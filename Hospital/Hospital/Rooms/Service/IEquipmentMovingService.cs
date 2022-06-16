using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public interface IEquipmentMovingService
    {
        List<EquipmentMoving> AllEquipmentMovings { get; }

        void MoveEquipment();

        bool IdExists(string id);

        bool ActiveMovingExists(string equipmentId);

        bool IsValidEquipmentMoving(string id, string equipmentId, DateTime scheduledTime, string sourceRoomId, string destinationRoomId);

        bool CreateEquipmentMoving(string id, string equipmentId, DateTime scheduledTime, string sourceRoomId, string destinationRoomId);
    }
}
