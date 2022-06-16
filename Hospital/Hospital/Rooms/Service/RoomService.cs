using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Hospital.Rooms.Repository;
using Hospital.Rooms.Model;
using Hospital;

namespace Hospital.Rooms.Service
{
    public class RoomService : IRoomService
    {
        private IRoomRepository _roomRepository;

        public RoomService()
        {
            _roomRepository = Globals.container.Resolve<IRoomRepository>();
        }
        public List<Room> AllRooms { get { return _roomRepository.AllRooms; } }

        public Room GetRoomById(string id)
        {
            return _roomRepository.GetRoomById(id);
        }

        public bool IdExists(string id)
        {
            return GetRoomById(id) != null;
        }

        public bool CreateRoom(string id, string name, Room.Type type)
        {
            if (IdExists(id))
                return false;
            _roomRepository.CreateRoom(id, name, type);
            return true;
        }

        public bool UpdateRoom(string id, string name, Room.Type type)
        {
            if (!IdExists(id))
                return false;
            _roomRepository.UpdateRoom(id, name, type);
            return true;
        }

        public bool DeleteRoom(string id)
        {
            if (!IdExists(id))
                return false;
            _roomRepository.DeleteRoom(id);
            return true;
        }

        public bool IsRoomNumberValid(string roomNumber)
        {
            foreach (Room room in AllRooms)
            {
                if (room.Id.Equals(roomNumber))
                {
                    return true;
                }
            }
            Console.WriteLine("Broj sobe ne postoji!");
            return false;

        }
    }
}
