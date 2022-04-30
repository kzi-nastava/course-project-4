using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class EquipmentMoving
    {
        private string id;
        private string equipmentId;
        private DateTime scheduledTime;
        private string sourceRoomId;
        private string destinationRoomId;

        public string Id { get { return id; } }
        public string EquipmentId { get { return equipmentId; } }
        public DateTime ScheduledTime { get { return scheduledTime; } }
        public string SourceRoomId { get { return sourceRoomId; } }
        public string DestinationRoomId { get { return destinationRoomId; } }

        public EquipmentMoving(string id, string equipmentId, DateTime scheduledTime,
            string sourceRoomId, string destinationRoomId)
        {
            this.id = id;
            this.equipmentId = equipmentId;
            this.scheduledTime = scheduledTime;
            this.sourceRoomId = sourceRoomId;
            this.destinationRoomId = destinationRoomId;
        }
    }
}
