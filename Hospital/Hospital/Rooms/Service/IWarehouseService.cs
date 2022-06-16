using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Rooms.Model;
using Hospital.Rooms.Repository;

namespace Hospital.Rooms.Service
{
    public interface IWarehouseService
    {
        List<DynamicEquipment> WarehouseEquipment { get; }
        List<DynamicEquipmentRequest> Requests { get; }
        DynamicEquipmentRequestRepository DynamicEquipmentRequestRepository { get; }
        List<DynamicEquipment> GetMissingEquipment();
        string GetNameEquipment(string id);
        void AddRequestedAmount(DynamicEquipmentRequest request);
        void UpdateWarehouse();

    }
}
