using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Service;

namespace Hospital.Rooms.Model
{
    public class MergeRenovation : Renovation
    {
        private string _otherRoomId;

        public string OtherRoomId { get { return _otherRoomId; } }

        public MergeRenovation(string id, DateTime startDate, DateTime endDate, string roomId, bool active, string otherRoomId)
            : base(id, startDate, endDate, roomId, active, Renovation.Type.MergeRenovation)
        {
            this._otherRoomId = otherRoomId;
        }

        public override void Renovate(RoomService roomService, EquipmentService equipmentService)
        {
            base.Renovate(roomService, equipmentService);

            Room room1 = roomService.GetRoomById(_roomId);
            Room room2 = roomService.GetRoomById(_otherRoomId);

            string newId = room1.Id + "+" + room2.Id;
            string newName = room1.Name + " + " + room2.Name;

            equipmentService.ChangeRoom(room1.Id, newId);
            equipmentService.ChangeRoom(room2.Id, newId);

            roomService.DeleteRoom(room1.Id);
            roomService.DeleteRoom(room2.Id);
            roomService.CreateRoom(newId, newName, room1.RoomType);
        }
    }
}
