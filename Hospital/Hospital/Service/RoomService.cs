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
        private RoomRepository roomRepository;
        private List<Room> rooms;

        public List<Room> Rooms { get { return rooms; } }
        
        public RoomService()
        {
            roomRepository = new RoomRepository();
            rooms = roomRepository.Load();
        }

        public Room GetRoomById(string id)
        {
            foreach (Room room in rooms)
            {
                if (room.Id == id)
                    return room;
            }
            return null;
        }

        public bool DoesIdExist(string id)
        {
            return GetRoomById(id) != null;
        }

        public bool CreateRoom(string id, string name, Room.TypeOfRoom type)
        {
            if (DoesIdExist(id))
                return false;
            Room room = new Room(id, name, type);
            rooms.Add(room);
            roomRepository.Save(rooms);
            return true;
        }

        public bool UpdateRoom(string id, string name, Room.TypeOfRoom type)
        {
            if (!DoesIdExist(id))
                return false;
            DeleteRoom(id);
            CreateRoom(id, name, type);
            return true;
        }

        public bool DeleteRoom(string id)
        {
            if (!DoesIdExist(id))
                return false;
            rooms.Remove(GetRoomById(id));
            roomRepository.Save(rooms);
            return true;
        }
    }
}
