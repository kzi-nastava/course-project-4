using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public interface IRoomService
    {
        List<Room> AllRooms { get; }

        Room GetRoomById(string id);

        bool IdExists(string id);

        bool CreateRoom(string id, string name, Room.Type type);

        bool UpdateRoom(string id, string name, Room.Type type);

        bool DeleteRoom(string id);

        bool IsRoomNumberValid(string roomNumber);
    }
}
