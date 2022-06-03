using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    public class RoomService
    {
        private RoomRepository _roomRepository;
        private List<Room> _allRooms;

        public List<Room> AllRooms { get { return _allRooms; } }
        
        public RoomService()
        {
            _roomRepository = new RoomRepository();
            _allRooms = _roomRepository.Load();
        }

        public Room GetRoomById(string id)
        {
            foreach (Room room in _allRooms)
            {
                if (room.Id == id)
                    return room;
            }
            return null;
        }

        public bool IdExists(string id)
        {
            return GetRoomById(id) != null;
        }

        public bool CreateRoom(string id, string name, Room.Type type)
        {
            if (IdExists(id))
                return false;
            Room room = new Room(id, name, type, false);
            _allRooms.Add(room);
            _roomRepository.Save(_allRooms);
            return true;
        }

        public bool UpdateRoom(string id, string name, Room.Type type)
        {
            if (!IdExists(id))
                return false;
            DeleteRoom(id);
            CreateRoom(id, name, type);
            return true;
        }

        public bool DeleteRoom(string id)
        {
            if (!IdExists(id))
                return false;
            foreach (Room room in _allRooms)
            {
                if (room.Id.Equals(id))
                    room.IsDeleted = true;
            }
            _roomRepository.Save(_allRooms);
            return true;
        }

        public bool IsRoomNumberValid(string roomNumber)
        {
            foreach (Room room in _allRooms)
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
