using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Rooms.Model
{
    public class EquipmentMoving
    {
        private string _id;
        private string _equipmentId;
        private DateTime _scheduledTime;
        private string _sourceRoomId;
        private string _destinationRoomId;
        private bool _active;

        public string Id { get { return _id; } }
        public string EquipmentId { get { return _equipmentId; } }
        public DateTime ScheduledTime { get { return _scheduledTime; } }
        public string SourceRoomId { get { return _sourceRoomId; } }
        public string DestinationRoomId { get { return _destinationRoomId; } }
        public bool IsActive { get { return _active; } set { _active = value; } }

        public EquipmentMoving(string id, string equipmentId, DateTime scheduledTime,
            string sourceRoomId, string destinationRoomId, bool active)
        {
            this._id = id;
            this._equipmentId = equipmentId;
            this._scheduledTime = scheduledTime;
            this._sourceRoomId = sourceRoomId;
            this._destinationRoomId = destinationRoomId;
            this._active = active;
        }
    }
}
