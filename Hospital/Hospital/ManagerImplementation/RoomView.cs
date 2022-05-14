using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.ManagerImplementation
{
    public class RoomView
    {
        private RoomService _roomService;

        public RoomView(RoomService roomService)
        {
            this._roomService = roomService;
        }

        public void CreateRoom()
        {
            Console.WriteLine("Unesite podatke o sobi");
            Console.WriteLine("------------------");

            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (_roomService.IdExists(id))
            {
                Console.Write("Broj sobe je zauzet. Odaberite drugi broj: ");
                id = Console.ReadLine();
            }

            Console.Write("Unesite naziv sobe: ");
            string name = Console.ReadLine();
            while (name.Length == 0)
            {
                Console.Write("Naziv ne moze biti prazan! Unesite naziv sobe: ");
                name = Console.ReadLine();
            }

            Console.WriteLine("Odaberite tip sobe");
            Console.WriteLine("1. Operaciona sala");
            Console.WriteLine("2. Sala za preglede");
            Console.WriteLine("3. Soba za odmor");
            Console.WriteLine("4. Druga prostorija");

            Room.Type type = Room.Type.Other;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    type = Room.Type.OperationRoom;
                else if (choice.Equals("2"))
                    type = Room.Type.ExaminationRoom;
                else if (choice.Equals("3"))
                    type = Room.Type.RestRoom;
                else if (choice.Equals("4"))
                    type = Room.Type.Other;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            _roomService.CreateRoom(id, name, type);
        }

        public void ListRooms()
        {
            List<Room> allRooms = _roomService.AllRooms;
            foreach (Room room in allRooms)
            {
                Console.WriteLine("Broj sobe: " + room.Id + ", naziv sobe: " + room.Name + ", tip sobe: " + room.TypeDescription + ", obrisana: " + room.IsDeleted);
            }
        }

        public void UpdateRoom()
        {
            Console.WriteLine("Unesite podatke o sobi");
            Console.WriteLine("------------------");

            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (!_roomService.IdExists(id))
            {
                Console.Write("Broj ne postoji. Unesite broj sobe: ");
                id = Console.ReadLine();
            }

            Console.Write("Unesite naziv sobe: ");
            string name = Console.ReadLine();
            while (name.Length == 0)
            {
                Console.Write("Naziv ne moze biti prazan! Unesite naziv sobe: ");
                name = Console.ReadLine();
            }

            Console.WriteLine("Odaberite tip sobe");
            Console.WriteLine("1. Operaciona sala");
            Console.WriteLine("2. Sala za preglede");
            Console.WriteLine("3. Soba za odmor");
            Console.WriteLine("4. Druga prostorija");

            Room.Type type = Room.Type.Other;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    type = Room.Type.OperationRoom;
                else if (choice.Equals("2"))
                    type = Room.Type.ExaminationRoom;
                else if (choice.Equals("3"))
                    type = Room.Type.RestRoom;
                else if (choice.Equals("4"))
                    type = Room.Type.Other;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            _roomService.UpdateRoom(id, name, type);
        }

        public void DeleteRoom()
        {
            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (!_roomService.IdExists(id))
            {
                Console.Write("Broj ne postoji. Unesite broj sobe: ");
                id = Console.ReadLine();
            }

            _roomService.DeleteRoom(id);
        }
    }
}
