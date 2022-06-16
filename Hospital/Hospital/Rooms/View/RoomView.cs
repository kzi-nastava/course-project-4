using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Service;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.View
{
    public class RoomView : IRoomView
    {
        private IRoomService _roomService;

        public RoomView(IRoomService roomService)
        {
            this._roomService = roomService;
        }

        public void ManageRooms()
        {
            Console.WriteLine("1. Kreiraj sobu");
            Console.WriteLine("2. Pregledaj sobe");
            Console.WriteLine("3. Izmeni sobu");
            Console.WriteLine("4. Obrisi sobu");
            Console.Write(">> ");
            string choice = Console.ReadLine();

            if (choice.Equals("1"))
                CreateRoom();
            else if (choice.Equals("2"))
                ListRooms();
            else if (choice.Equals("3"))
                UpdateRoom();
            else if (choice.Equals("4"))
                DeleteRoom();
        }

        private string EnterRoomId(bool existing)
        {
            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (_roomService.IdExists(id) != existing)
            {
                if (existing)
                    Console.Write("Broj ne postoji. Unesite broj sobe: ");
                else
                    Console.Write("Broj sobe je zauzet. Odaberite drugi broj: ");
                id = Console.ReadLine();
            }
            return id;
        }

        private string EnterNewRoomId()
        {
            return EnterRoomId(false);
        }

        private string EnterExistingRoomId()
        {
            return EnterRoomId(true);
        }

        private string EnterRoomName()
        {
            Console.Write("Unesite naziv sobe: ");
            string name = Console.ReadLine();
            while (name.Length == 0)
            {
                Console.Write("Naziv ne moze biti prazan! Unesite naziv sobe: ");
                name = Console.ReadLine();
            }
            return name;
        }

        private Room.Type EnterRoomType()
        {
            Console.WriteLine("Odaberite tip sobe");
            Console.WriteLine("1. Operaciona sala");
            Console.WriteLine("2. Sala za preglede");
            Console.WriteLine("3. Soba za odmor");
            Console.WriteLine("4. Druga prostorija");

            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                if (choice.Equals("1"))
                    return Room.Type.OperationRoom;
                else if (choice.Equals("2"))
                    return Room.Type.ExaminationRoom;
                else if (choice.Equals("3"))
                    return Room.Type.RestRoom;
                else if (choice.Equals("4"))
                    return Room.Type.Other;
            }
        }

        public void CreateRoom()
        {
            Console.WriteLine("Unesite podatke o sobi");
            Console.WriteLine("------------------");

            string id = EnterNewRoomId();
            string name = EnterRoomName();
            Room.Type type = EnterRoomType();

            _roomService.CreateRoom(id, name, type);
        }

        public void ListRooms()
        {
            List<Room> allRooms = _roomService.AllRooms;
            foreach (Room room in allRooms)
            {
                Console.WriteLine("Broj sobe: " + room.Id + ", naziv sobe: " + room.Name + ", tip sobe: " 
                    + room.TypeDescription + ", obrisana: " + room.IsDeleted);
            }
        }

        public void UpdateRoom()
        {
            Console.WriteLine("Unesite podatke o sobi");
            Console.WriteLine("------------------");

            string id = EnterExistingRoomId();
            string name = EnterRoomName();
            Room.Type type = EnterRoomType();

            _roomService.UpdateRoom(id, name, type);
        }

        public void DeleteRoom()
        {
            string id = EnterExistingRoomId();
            _roomService.DeleteRoom(id);
        }
    }
}
