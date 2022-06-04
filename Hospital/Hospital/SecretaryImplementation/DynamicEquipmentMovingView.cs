using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;

namespace Hospital.SecretaryImplementation
{
	class DynamicEquipmentMovingView
	{
		public void ShowMissingEquipment(List<KeyValuePair<string, DynamicEquipment>> missingEquipment)
		{
			int i = 1;
			foreach (KeyValuePair<string, DynamicEquipment> pair in missingEquipment.OrderBy(key => key.Value.Amount))
			{
				Console.WriteLine("{0}. Soba: {1} | Naziv opreme: {2} | Kolicina: {3}", i, pair.Key, pair.Value.Name, pair.Value.Amount);
				i++;
			}

		}

		public int InputMissingEquipmentIndex()
		{
			string indexInput;
			int index;
			do
			{
				Console.WriteLine("\nUnesite redni broj sobe u kojoj zelite da dopunite odredjenu opremu");
				Console.Write(">>");
				indexInput = Console.ReadLine();

			} while (!int.TryParse(indexInput, out index) || index < 1);
			return index;
		}

		public KeyValuePair<string, DynamicEquipment> SelectPair(List<KeyValuePair<string, DynamicEquipment>> missingEquipment)
		{
			ShowMissingEquipment(missingEquipment);
			Console.WriteLine("-----------------------------------------------------------------");
			int index;
			do
			{
				index = InputMissingEquipmentIndex();
			} while (index > missingEquipment.Count);
			int i = 1;
			foreach (KeyValuePair<string, DynamicEquipment> pair in missingEquipment.OrderBy(key => key.Value.Amount))
			{
				if (i == index)
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
				Console.Write("\nUnesite kolicinu opreme koju zelite da premestite u izabranu sobu: ");
				amountInput = Console.ReadLine();
			} while (!int.TryParse(amountInput, out amount) || amount < 1);
			return amount;
		}

		public void ShowRoomsWithEquipment(List<Room> rooms)
		{
			int i = 1;
			foreach(Room room in rooms)
			{
				Console.WriteLine("{0}. Soba: {1}", i, room.Id);
				i++;
			}
		}

		public int InputRoomIndex()
		{
			string indexInput;
			int index;
			do
			{
				Console.WriteLine("\nUnesite redni broj sobe iz koje zelite da uzmete odredjenu opremu");
				Console.Write(">>");
				indexInput = Console.ReadLine();

			} while (!int.TryParse(indexInput, out index) || index < 1);
			return index;
		}

		public Room SelectRoom(List<Room> rooms)
		{
			ShowRoomsWithEquipment(rooms);
			if (rooms.Count == 0)
				return null;
			int index;
			do
			{
				index = InputRoomIndex();
			} while (index > rooms.Count);
			return rooms[index - 1];
		}
	}
}
