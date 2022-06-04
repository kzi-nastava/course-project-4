using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class DynamicEquipmentMoving
	{
		private WarehouseService _warehouseService;
		private DynamicRoomEquipmentService _dynamicRoomEquipmentService;
		private DynamicEquipmentMovingView _dynamicEquipmentMovingView;
		private RoomService _roomService;
		public DynamicEquipmentMoving()
		{
			this._warehouseService = new WarehouseService();
			this._dynamicRoomEquipmentService = new DynamicRoomEquipmentService();
			this._dynamicEquipmentMovingView = new DynamicEquipmentMovingView();
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

		public void MoveEquipment()
		{
			List < KeyValuePair<string, DynamicEquipment> > missingEquipment = GetMissingEquipmentInRooms();
			if(missingEquipment.Count == 0)
			{
				Console.WriteLine("\nTrenutno ni u jednoj sobi ne postoji potreba za premestanjem dinamicke robe.\n");
				return;
			}
			KeyValuePair<string, DynamicEquipment> selectedPair = _dynamicEquipmentMovingView.SelectPair(missingEquipment);
			var amount = _dynamicEquipmentMovingView.InputAmount();
			List<Room> roomsWithEquipment = GetRoomsWithEnoughEquipment(selectedPair.Value.Id, amount, selectedPair.Key);
			Room selectedRoom = _dynamicEquipmentMovingView.SelectRoom(roomsWithEquipment);
			if(selectedRoom is null)
			{
				Console.WriteLine("Nema soba sa dovoljno opreme!");
				return;
			}
			_dynamicRoomEquipmentService.ChangeEquipmentAmount(selectedPair.Key, selectedPair.Value.Id, amount, true);
			_dynamicRoomEquipmentService.ChangeEquipmentAmount(selectedRoom.Id, selectedPair.Value.Id, amount, false);
		}
	}

}
