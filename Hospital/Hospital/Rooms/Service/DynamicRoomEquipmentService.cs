using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Hospital.Rooms.Repository;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public class DynamicRoomEquipmentService
    {

        private DynamicRoomEquipmentRepository _equipmentRepository;
        private List<DynamicRoomEquipment> _dynamicEquipments;

        public DynamicRoomEquipmentService()
        {
            this._equipmentRepository = new DynamicRoomEquipmentRepository();
            this._dynamicEquipments = this._equipmentRepository.Load();
        }

        public List<DynamicRoomEquipment> DynamicEquipments { get { return _dynamicEquipments; } }

        public void ChangeEquipmentAmount(string roomId, string equipmentId, int amount, bool add)
		{
            foreach(DynamicRoomEquipment dynamicRoomEquipment in _dynamicEquipments)
			{
                if(dynamicRoomEquipment.IdRoom == roomId)
				{
                    if (add)
                        dynamicRoomEquipment.AmountEquipment[equipmentId] += amount;
                    else
                        dynamicRoomEquipment.AmountEquipment[equipmentId] -= amount;
                    break;
                }
			}
            UpdateFile();
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
            UpdateFile();
            
        }

        public void UpdateFile()
        {
            this._equipmentRepository.Save(this._dynamicEquipments);
        }

    }
}
