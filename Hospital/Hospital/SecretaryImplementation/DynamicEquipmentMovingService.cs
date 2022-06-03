using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class DynamicEquipmentMovingService
	{
		private WarehouseService _warehouseService;
		private DynamicRoomEquipmentService _dynamicRoomEquipmentService;
		public DynamicEquipmentMovingService()
		{
			this._warehouseService = new WarehouseService();
			this._dynamicRoomEquipmentService = new DynamicRoomEquipmentService();
		}

		public Dictionary<string, DynamicEquipment> GetMissingEquipmentInRooms()
		{
			Dictionary<string, DynamicEquipment> missingEquipment = new Dictionary<string, DynamicEquipment>();
			foreach (DynamicRoomEquipment roomEquipment in _dynamicRoomEquipmentService.DynamicEquipments)
			{
				foreach (KeyValuePair<string, int> pair in roomEquipment.AmountEquipment)
				{
					if (pair.Value < 5)
					{
						DynamicEquipment equipment = new DynamicEquipment(pair.Key, _warehouseService.GetNameEquipment(pair.Key), pair.Value);
						missingEquipment.Add(roomEquipment.IdRoom, equipment);
					}
				}
			}
			return missingEquipment;
		}

		public void ShowMissingEquipment(Dictionary<string, DynamicEquipment> missingEquipment)
		{
			int i = 1;
			foreach(KeyValuePair<string, DynamicEquipment> pair in missingEquipment.OrderBy(key => key.Value.Amount))
			{
				Console.WriteLine("{0}. Soba: {1} | Naziv opreme: {2} | Kolicina: {3}", i, pair.Key, pair.Value.Name, pair.Value.Amount);
				i++;
			}

		}

		public KeyValuePair<string, DynamicEquipment> SelectPair(Dictionary<string, DynamicEquipment> missingEquipment)
		{
			ShowMissingEquipment(missingEquipment);
			Console.WriteLine("-----------------------------------------------------------------");
			string indexInput;
			int index;
			do
			{
				Console.WriteLine("Unesite redni broj sobe u kojoj zelite da dopunite odredjenu opremu");
				Console.Write(">>");
				indexInput = Console.ReadLine();

			} while (!int.TryParse(indexInput, out index) || index < 1 || index > missingEquipment.Count);
			int i = 1;
			foreach (KeyValuePair<string, DynamicEquipment> pair in missingEquipment.OrderBy(key => key.Value.Amount))
			{
				if(i == index)
				{
					return pair;
				}
				i++;
			}
			return new KeyValuePair<string, DynamicEquipment>("0", new DynamicEquipment("0", "", 0));
		}

		public int InputAmount()
		{
			string amountInput;
			int amount;
			do
			{
				Console.Write("Unesite kolicinu opreme koju zelite da premestite u izabranu sobu: ");
				amountInput = Console.ReadLine();
			} while (!int.TryParse(amountInput, out amount) || amount < 1);
			return amount;
		}

		public void MoveEquipment()
		{
			Dictionary<string, DynamicEquipment> missingEquipment = GetMissingEquipmentInRooms();
			if(missingEquipment.Count == 0)
			{
				Console.WriteLine("\nTrenutno ni u jednoj sobi ne postoji potreba za premestanjem dinamicke robe.\n");
				return;
			}
			KeyValuePair<string, DynamicEquipment> selectedPair = SelectPair(missingEquipment);
			var amount = InputAmount();
		}
	}

}
