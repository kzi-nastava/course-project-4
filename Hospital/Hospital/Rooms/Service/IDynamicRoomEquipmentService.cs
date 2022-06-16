using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public interface IDynamicRoomEquipmentService
    {
        List<DynamicRoomEquipment> DynamicEquipments { get; }
        void UpdateFile();
        void UpdateDictionary(Dictionary<string, int> amount, string idRoom);
        void ChangeEquipmentAmount(string roomId, string equipmentId, int amount, bool add);

    }
}
