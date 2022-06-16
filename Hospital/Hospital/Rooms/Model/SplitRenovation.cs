using Hospital.Rooms.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Rooms.Model
{
    public class SplitRenovation : Renovation
    {
        public SplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId, bool active)
            : base(id, startDate, endDate, roomId, active, Renovation.Type.SplitRenovation)
        {
        }

        public override void Renovate(IRoomService roomService, IEquipmentService equipmentService)
        {
            base.Renovate(roomService, equipmentService);

            Room roomBefore = roomService.GetRoomById(_roomId);
            equipmentService.ChangeRoom(roomBefore.Id, roomBefore.Id + "_1");

            roomService.DeleteRoom(roomBefore.Id);
            roomService.CreateRoom(roomBefore.Id + "_1", roomBefore.Name + " 1", roomBefore.RoomType);
            roomService.CreateRoom(roomBefore.Id + "_2", roomBefore.Name + " 2", roomBefore.RoomType);
        }
    }
}
