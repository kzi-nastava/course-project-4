using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public interface IWarehouseService
    {
        List<DynamicEquipment> GetMissingEquipment();
        string GetNameEquipment(string id);
        void AddRequestedAmount(DynamicEquipmentRequest request);
        void UpdateWarehouse();

    }
}
