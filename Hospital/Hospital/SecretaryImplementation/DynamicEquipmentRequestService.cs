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
		private DynamicEquipmentRequestRepository _repository;
		private WarehouseService _warehouseService;
		private List<DynamicEquipmentRequest> _requests;
		private List<DynamicEquipment> _warehouseEquipment;

		public DynamicEquipmentRequestService()
		{
			this._repository = new DynamicEquipmentRequestRepository();
			this._warehouseService = new WarehouseService();
			this._warehouseEquipment = _warehouseService.WarehouseEquipment;
			this._requests = _repository.Load();
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

		public void ShowEquipment(List<DynamicEquipment> dynamicEquipment)
		{
			int i = 1;
			foreach(DynamicEquipment equipment in dynamicEquipment)
			{
				Console.WriteLine("{0}. {1}", i, equipment.Name);
			}
		}

		public DynamicEquipment SelectEquipment(List<DynamicEquipment> dynamicEquipment)
		{
			if(dynamicEquipment.Count == 0)
			{
				Console.WriteLine("Trenutno nema robe koja nedostaje");
				return null;
			}
			ShowEquipment(dynamicEquipment);
			Console.WriteLine("x. Odustani");
			Console.WriteLine("------------------------------");
			string indexInput;
			int index;
			do
			{
				Console.WriteLine("Unesite redni broj opreme: ");
				Console.Write(">>");
				indexInput = Console.ReadLine();
				if (indexInput == "x")
				{
					return null;
				}
			} while (!int.TryParse(indexInput, out index) || index < 1 || index > dynamicEquipment.Count);
			return dynamicEquipment[index - 1];
		}

		public int InputAmount()
		{
			string userInput;
			int amount;
			do
			{
				Console.Write("Unesite kolicinu opreme: ");
				userInput = Console.ReadLine();
			} while (!int.TryParse(userInput, out amount));

			return amount;

		}

		public void SendRequestForProcurment()
		{
			List<DynamicEquipment> missingEquipment = GetMissingEquipment();
			DynamicEquipment chosenEquipment = SelectEquipment(missingEquipment);
			int amount = InputAmount();


		}
	}
}
