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
        private EquipmentService equipmentService;

        public Manager(User currentRegisteredManager)
        {
            this.currentRegisteredManager = currentRegisteredManager;
            roomService = new RoomService();
            equipmentService = new EquipmentService(roomService);
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
                Console.WriteLine("5. Pretraga opreme");
                Console.WriteLine("6. Filtriranje opreme");
                Console.WriteLine("7. Odjava");
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
                    this.searchEquipment();
                else if (choice.Equals("6"))
                    this.filterEquipment();
                else if (choice.Equals("7"))
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

        private void printEquipment(List<Equipment> equipmentList)
        {
            foreach (Equipment equipment in equipmentList)
            {
                Console.WriteLine("Identifikator: " + equipment.Id + ", naziv: " + equipment.Name + ", tip opreme: "
                    + equipment.TypeStr + ", kolicina: " + equipment.Quantity + ", broj sobe: " + equipment.RoomId);
            }
        }

        private void searchEquipment()
        {
            Console.Write("Unesite karaktere pretrage: ");
            string query = Console.ReadLine();

            List<Equipment> foundEquipment = equipmentService.Search(query);
            printEquipment(foundEquipment);
        }

        private void filterEquipment()
        {
            Console.WriteLine("Odaberite kriterijum filtriranja");
            Console.WriteLine("1. Tip sobe");
            Console.WriteLine("2. Kolicina");
            Console.WriteLine("3. Tip opreme");

            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    filterEquipmentByRoomType();
                else if (choice.Equals("2"))
                    filterEquipmentByQuantity();
                else if (choice.Equals("3"))
                    filterEquipmentByType();
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }
        }

        private void filterEquipmentByRoomType()
        {
            Console.WriteLine("Odaberite tip sobe");
            Console.WriteLine("1. Operaciona sala");
            Console.WriteLine("2. Sala za preglede");
            Console.WriteLine("3. Soba za odmor");
            Console.WriteLine("4. Druga soba");
            Console.WriteLine("5. Magacin");

            Room.TypeOfRoom roomType = Room.TypeOfRoom.Other;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    roomType = Room.TypeOfRoom.OperationRoom;
                else if (choice.Equals("2"))
                    roomType = Room.TypeOfRoom.ExaminationRoom;
                else if (choice.Equals("3"))
                    roomType = Room.TypeOfRoom.RestRoom;
                else if (choice.Equals("4"))
                    roomType = Room.TypeOfRoom.Other;
                else if (choice.Equals("5"))
                    roomType = Room.TypeOfRoom.Warehouse;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            List<Equipment> foundEquipment = equipmentService.FilterByRoomType(roomType);
            printEquipment(foundEquipment);
        }

        private void filterEquipmentByQuantity()
        {
            Console.WriteLine("Odaberite opciju");
            Console.WriteLine("1. Nema na stanju");
            Console.WriteLine("2. 0-10");
            Console.WriteLine("3. 10+");

            int lowerBound = -1;
            int upperBound = -1;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                {
                    lowerBound = 0;
                    upperBound = 0;
                }
                else if (choice.Equals("2"))
                {
                    lowerBound = 0;
                    upperBound = 10;
                }
                else if (choice.Equals("3"))
                {
                    lowerBound = 10;
                    upperBound = -1;
                }
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            List<Equipment> foundEquipment = equipmentService.FilterByQuantity(lowerBound, upperBound);
            printEquipment(foundEquipment);
        }

        private void filterEquipmentByType()
        {
            Console.WriteLine("Odaberite tip opreme");
            Console.WriteLine("1. Oprema za preglede");
            Console.WriteLine("2. Oprema za operacije");
            Console.WriteLine("3. Sobni namestaj");
            Console.WriteLine("4. Oprema za hodnike");
            
            Equipment.TypeOfEquipment equipmentType = Equipment.TypeOfEquipment.ExaminationEquipment;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    equipmentType = Equipment.TypeOfEquipment.ExaminationEquipment;
                else if (choice.Equals("2"))
                    equipmentType = Equipment.TypeOfEquipment.OperationEquipment;
                else if (choice.Equals("3"))
                    equipmentType = Equipment.TypeOfEquipment.Furniture;
                else if (choice.Equals("4"))
                    equipmentType = Equipment.TypeOfEquipment.HallwayEquipment;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            List<Equipment> foundEquipment = equipmentService.FilterByEquipmentType(equipmentType);
            printEquipment(foundEquipment);
        }

        private void logOut()
        {
            Login loging = new Login();
            loging.logIn();
        }
    }
}
