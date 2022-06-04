using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class DynamicEquipmentRequestService
	{
		private WarehouseService _warehouseService;
		private List<DynamicEquipment> _warehouseEquipment;

		public DynamicEquipmentRequestService()
		{
			this._warehouseService = new WarehouseService();
			this._warehouseEquipment = _warehouseService.WarehouseEquipment;
		}
		public List<DynamicEquipment> GetMissingEquipment()
		{
			List<DynamicEquipment> missingEquipment = new List<DynamicEquipment>();

			foreach (DynamicEquipment equipment in _warehouseEquipment)
			{
				if(equipment.Amount == 0)
				{
					missingEquipment.Add(equipment);
				}
			}
			return missingEquipment;
		}

		public void SendRequestForProcurment()
		{
			List<DynamicEquipment> missingEquipment = GetMissingEquipment();
			DynamicEquipment chosenEquipment = DynamicEquipmentRequestView.SelectEquipment(missingEquipment);
			if (chosenEquipment is null)
				return;
			int amount = DynamicEquipmentRequestView.InputAmount();
			DynamicEquipmentRequest request = new DynamicEquipmentRequest(chosenEquipment.Id, amount, DateTime.Now.AddHours(24), false);
			_warehouseService.Requests.Add(request);
			_warehouseService.DynamicEquipmentRequestRepository.Save(_warehouseService.Requests);
		}
	}
}
