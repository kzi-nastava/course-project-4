using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    }
}
