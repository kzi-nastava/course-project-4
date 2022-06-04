using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class WarehouseService
    {
        private WarehouseRepository _warehouseRepository;
        private List<DynamicEquipment> _warehouseEquipment;
        private DynamicEquipmentRequestRepository _dynamicEquipmentRequestRepository;
        private List<DynamicEquipmentRequest> _requests;

        public WarehouseService()
        {
            this._warehouseRepository = new WarehouseRepository();
            this._warehouseEquipment = this._warehouseRepository.Load();
            this._dynamicEquipmentRequestRepository = new DynamicEquipmentRequestRepository();
            _requests = _dynamicEquipmentRequestRepository.Load();
            UpdateWarehouse();
        }
        
        public List<DynamicEquipment> WarehouseEquipment { get { return _warehouseEquipment; } }
        public List<DynamicEquipmentRequest> Requests { get { return _requests; } }
        public DynamicEquipmentRequestRepository DynamicEquipmentRequestRepository { get { return _dynamicEquipmentRequestRepository; } }

        public void UpdateWarehouse()
		{
            foreach(DynamicEquipmentRequest request in _requests)
			{
                if (request.Updated || request.AddTime > DateTime.Now)
                    continue;
                AddRequestedAmount(request);
                request.Updated = true;
			}
            _warehouseRepository.Save(_warehouseEquipment);
            _dynamicEquipmentRequestRepository.Save(_requests);
        }

        public void AddRequestedAmount(DynamicEquipmentRequest request)
		{
            foreach(DynamicEquipment equipment in _warehouseEquipment)
			{
                if(equipment.Id == request.DynamicEquipmentId)
				{
                    equipment.Amount += request.Amount;
                    break;
				}
			}
		}

        public string GetNameEquipment(string id)
        {
            foreach(DynamicEquipment equipment in this._warehouseEquipment)
            {
                if (equipment.Id.Equals(id))
                {
                    return equipment.Name;
                }
            }
            return "";
        }
    }
}
