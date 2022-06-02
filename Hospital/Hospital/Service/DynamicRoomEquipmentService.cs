using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;
using System.IO;

namespace Hospital.Service
{
    class DynamicRoomEquipmentService
    {

        private DynamicRoomEquipmentRepository _equipmentRepository;
        private List<DynamicRoomEquipment> _dynamicEquipments;


        public DynamicRoomEquipmentService()
        {
            this._equipmentRepository = new DynamicRoomEquipmentRepository();
            this._dynamicEquipments = this._equipmentRepository.Load();
        }

        public List<DynamicRoomEquipment> DynamicEquipments { get { return _dynamicEquipments; } }

        public void UpdateFile()
        {
            string filePath = @"..\..\Data\dynamicRoomEquipments.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (DynamicRoomEquipment equipment in this._dynamicEquipments)
            {
                line = equipment.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }

        public void  UpdateDictionary(Dictionary<string, int> amount, string idRoom)
        {
            for(int i = 0; i < this._dynamicEquipments.Count; i++)
            {
                if (this._dynamicEquipments[i].IdRoom.Equals(idRoom))
                {
                    this._dynamicEquipments[i].AmountEquipment = amount;

                }
            }
        }

  //      public void AddEquipmentAmount(string roomId, string equipmentId, int amount)
		//{
  //          for(int i = 0; i < this._dynamicEquipments.Count; i++)
		//	{
		//		if (this._dynamicEquipments[i].IdRoom.Equals(roomId))
		//		{
  //                  foreach(KeyValuePair<string, int> pair in this._dynamicEquipments[i].AmountEquipment)
		//			{
		//				if (pair.Key.Equals(equipmentId))
		//				{
  //                          this._dynamicEquipments[i].AmountEquipment[pair.Key] = pair.Value + amount;
  //                          UpdateFile();
  //                          return;

  //                      }
		//			}
		//		}
		//	}
		//}
    }
}
