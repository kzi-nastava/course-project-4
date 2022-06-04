using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;

namespace Hospital.SecretaryImplementation
{
	class DynamicEquipmentRequestView
	{
		public static void ShowEquipment(List<DynamicEquipment> dynamicEquipment)
		{
			int i = 1;
			foreach (DynamicEquipment equipment in dynamicEquipment)
			{
				Console.WriteLine("{0}. {1}", i, equipment.Name);
				i++;
			}
		}

		public static int EnterEquipmentIndex()
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

		public static int InputAmount()
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

		public static DynamicEquipment SelectEquipment(List<DynamicEquipment> dynamicEquipment)
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
	}
}
