using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.PatientImplementation;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.ManagerImplementation
{
    public class Manager
    {
        private User currentRegisteredManager;
        private RoomService roomService;

        public Manager(User currentRegisteredManager)
        {
            this.currentRegisteredManager = currentRegisteredManager;
            roomService = new RoomService();
        }

        public void ManagerMenu() 
        {
            string choice;
            Console.WriteLine("\n\tMENI");
            Console.Write("------------------");
            do
            {
                Console.WriteLine("\n1. Kreiraj sobu");
                Console.WriteLine("2. Pregledaj sobe");
                Console.WriteLine("3. Izmeni sobu");
                Console.WriteLine("4. Obrisi sobu");
                Console.WriteLine("5. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    this.createRoom();
                else if (choice.Equals("2"))
                    this.listRooms();
                else if (choice.Equals("3"))
                    this.updateRoom();
                else if (choice.Equals("4"))
                    this.deleteRoom();
                else if (choice.Equals("5"))
                    this.logOut();
            } while (true);
        }

        private void createRoom() 
        {
            Console.WriteLine("Unesite podatke o sobi");
            Console.WriteLine("------------------");

            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (roomService.DoesIdExist(id))
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

            Room.TypeOfRoom type = Room.TypeOfRoom.Other;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    type = Room.TypeOfRoom.OperationRoom;
                else if (choice.Equals("2"))
                    type = Room.TypeOfRoom.ExaminationRoom;
                else if (choice.Equals("3"))
                    type = Room.TypeOfRoom.RestRoom;
                else if (choice.Equals("4"))
                    type = Room.TypeOfRoom.Other;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            roomService.CreateRoom(id, name, type);
        }

        private void listRooms()
        {
            List<Room> allRooms = roomService.Rooms;
            foreach (Room room in allRooms)
            {
                Console.WriteLine("Broj sobe: " + room.Id + ", naziv sobe: " + room.Name + ", tip sobe: " + room.TypeStr);
            }
        }

        private void updateRoom()
        {
            Console.WriteLine("Unesite podatke o sobi");
            Console.WriteLine("------------------");

            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (!roomService.DoesIdExist(id))
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

            Room.TypeOfRoom type = Room.TypeOfRoom.Other;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    type = Room.TypeOfRoom.OperationRoom;
                else if (choice.Equals("2"))
                    type = Room.TypeOfRoom.ExaminationRoom;
                else if (choice.Equals("3"))
                    type = Room.TypeOfRoom.RestRoom;
                else if (choice.Equals("4"))
                    type = Room.TypeOfRoom.Other;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            roomService.UpdateRoom(id, name, type);
        }

        private void deleteRoom()
        {
            Console.Write("Unesite broj sobe: ");
            string id = Console.ReadLine();
            while (!roomService.DoesIdExist(id))
            {
                Console.Write("Broj ne postoji. Unesite broj sobe: ");
                id = Console.ReadLine();
            }

            roomService.DeleteRoom(id);
        }

        private void logOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
