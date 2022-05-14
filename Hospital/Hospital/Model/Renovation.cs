using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Renovation
    {
        private string _id;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _roomId;

        public string Id { get { return _id; } }
        public DateTime StartDate { get { return _startDate; } }
        public DateTime EndDate { get { return _endDate; } }
        public string RoomId { get { return _roomId; } }

        public Renovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            this._id = id;
            this._startDate = startDate;
            this._endDate = endDate;
            this._roomId = roomId;
        }
    }
}
