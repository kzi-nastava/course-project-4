using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public class DynamicEquipmentMovingService
	{
		private WarehouseService _warehouseService;
		private DynamicRoomEquipmentService _dynamicRoomEquipmentService;
		private RoomService _roomService;
		public DynamicEquipmentMovingService()
		{
			this._warehouseService = new WarehouseService();
			this._dynamicRoomEquipmentService = new DynamicRoomEquipmentService();
			this._roomService = new RoomService();
		}

		public List<KeyValuePair<string, DynamicEquipment>> GetMissingEquipmentInRooms()
		{
			List<KeyValuePair<string, DynamicEquipment>> missingEquipment =new List<KeyValuePair<string, DynamicEquipment>>();
			foreach (DynamicRoomEquipment roomEquipment in _dynamicRoomEquipmentService.DynamicEquipments)
			{
				foreach (KeyValuePair<string, int> pair in roomEquipment.AmountEquipment)
				{
					if (pair.Value < 5)
					{
						DynamicEquipment equipment = new DynamicEquipment(pair.Key, _warehouseService.GetNameEquipment(pair.Key), pair.Value);
						missingEquipment.Add(new KeyValuePair<string, DynamicEquipment>(roomEquipment.IdRoom, equipment));
					}
				}
			}
			return missingEquipment;
		}

		public List<Room> GetRoomsWithEnoughEquipment(string equipmentId, int amount, string roomId)
		{
			List<Room> rooms = new List<Room>();
			foreach(DynamicRoomEquipment roomEquipment in _dynamicRoomEquipmentService.DynamicEquipments)
			{
				if (roomId == roomEquipment.IdRoom)
					continue;
				int equipmentAmount = roomEquipment.AmountEquipment[equipmentId];
				if (equipmentAmount > amount)
					rooms.Add(_roomService.GetRoomById(roomEquipment.IdRoom));
			}
			return rooms;
		}
	}

}
