using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public interface IEquipmentMovingRepository : IRepository<EquipmentMoving>
    {
        List<EquipmentMoving> AllEquipmentMovings { get; }

        bool IdExists(string id);

        bool ActiveMovingExists(string equipmentId);

        void CreateEquipmentMoving(string id, string equipmentId, DateTime scheduledTime, string sourceRoomId, string destinationRoomId);
    }
}
