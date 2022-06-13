using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Service;
using Hospital.Rooms.Model;
using Hospital.Rooms.Repository;

namespace Hospital.Rooms.View
{
    public class DynamicEquipmentRequestView
	{
		private WarehouseService _warehouseService;

		public DynamicEquipmentRequestView()
		{
			this._warehouseService = new WarehouseService();
		}

		public static void ShowEquipment(List<DynamicEquipment> dynamicEquipment)
		{
			int i = 1;
			foreach (DynamicEquipment equipment in dynamicEquipment)
			{
				Console.WriteLine("{0}. {1}", i, equipment.Name);
				i++;
			}
		}

		public int EnterEquipmentIndex()
		{
			string indexInput;
			int index;
			do
			{
				Console.WriteLine("Unesite redni broj opreme: ");
				Console.Write(">>");
				indexInput = Console.ReadLine();
				if (indexInput == "x")
				{
					return 0;
				}
			} while (!int.TryParse(indexInput, out index) || index < 1);
			return index;
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

		public DynamicEquipment SelectEquipment(List<DynamicEquipment> dynamicEquipment)
		{
			if (dynamicEquipment.Count == 0)
			{
				Console.WriteLine("Trenutno nema dinamicke opreme koja nedostaje");
				return null;
			}
			ShowEquipment(dynamicEquipment);
			Console.WriteLine("x. Odustani");
			Console.WriteLine("------------------------------");
			int index;
			do
			{
				index = EnterEquipmentIndex();
			} while (index > dynamicEquipment.Count);
			if (index == 0)
				return null;
			return dynamicEquipment[index - 1];
		}

		public void SendRequestForProcurment()
		{
			List<DynamicEquipment> missingEquipment = _warehouseService.GetMissingEquipment();
			DynamicEquipment chosenEquipment = SelectEquipment(missingEquipment);
			if (chosenEquipment is null)
				return;
			int amount = InputAmount();
			DynamicEquipmentRequest request = new DynamicEquipmentRequest(chosenEquipment.Id, amount, DateTime.Now.AddHours(24), false);
			_warehouseService.Requests.Add(request);
			_warehouseService.DynamicEquipmentRequestRepository.Save(_warehouseService.Requests);
			Console.WriteLine("Zahtev za nabavku opreme je uspesno poslat.\nZeljena kolicina bice dodata nakon 24h.");
		}
	}
}
