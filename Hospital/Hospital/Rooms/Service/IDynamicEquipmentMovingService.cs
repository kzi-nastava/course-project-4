using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
	public interface IDynamicEquipmentMovingService
	{
		List<KeyValuePair<string, DynamicEquipment>> GetMissingEquipmentInRooms();
		List<Room> GetRoomsWithEnoughEquipment(string equipmentId, int amount, string roomId);
	}
}
