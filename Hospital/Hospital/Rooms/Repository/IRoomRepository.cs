using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public interface IRoomRepository : IRepository<Room>
    {
        List<Room> AllRooms { get; }

        Room GetRoomById(string id);

        void CreateRoom(string id, string name, Room.Type type);

        void UpdateRoom(string id, string name, Room.Type type);

        void DeleteRoom(string id);
    }
}
