using System;
using System.Collections.Generic;
using System.Globalization;
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
        private EquipmentMovingService equipmentMovingService;

        public Manager(User currentRegisteredManager)
        {
            this.currentRegisteredManager = currentRegisteredManager;
            roomService = new RoomService();
            equipmentService = new EquipmentService(roomService);
            equipmentMovingService = new EquipmentMovingService(equipmentService, roomService);
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
                Console.WriteLine("7. Zakazi premestanje opreme");
                Console.WriteLine("8. Pokreni premestanje opreme");
                Console.WriteLine("9. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    this.CreateRoom();
                else if (choice.Equals("2"))
                    this.ListRooms();
                else if (choice.Equals("3"))
                    this.UpdateRoom();
                else if (choice.Equals("4"))
                    this.DeleteRoom();
                else if (choice.Equals("5"))
                    this.SearchEquipment();
                else if (choice.Equals("6"))
                    this.FilterEquipment();
                else if (choice.Equals("7"))
                    this.ScheduleEquipmentMoving();
                else if (choice.Equals("8"))
                    this.MoveEquipment();
                else if (choice.Equals("9"))
                    this.LogOut();
            } while (true);
        }

        private void CreateRoom() 
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

        private void ListRooms()
        {
            List<Room> allRooms = roomService.Rooms;
            foreach (Room room in allRooms)
            {
                Console.WriteLine("Broj sobe: " + room.Id + ", naziv sobe: " + room.Name + ", tip sobe: " + room.TypeStr);
            }
        }

        private void UpdateRoom()
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

        private void DeleteRoom()
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

        private void PrintEquipment(List<Equipment> equipmentList)
        {
            foreach (Equipment equipment in equipmentList)
            {
                Console.WriteLine("Identifikator: " + equipment.Id + ", naziv: " + equipment.Name + ", tip opreme: "
                    + equipment.TypeStr + ", kolicina: " + equipment.Quantity + ", broj sobe: " + equipment.RoomId);
            }
        }

        private void SearchEquipment()
        {
            Console.Write("Unesite karaktere pretrage: ");
            string query = Console.ReadLine();

            List<Equipment> foundEquipment = equipmentService.Search(query);
            PrintEquipment(foundEquipment);
        }

        private void FilterEquipment()
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
                    FilterEquipmentByRoomType();
                else if (choice.Equals("2"))
                    FilterEquipmentByQuantity();
                else if (choice.Equals("3"))
                    FilterEquipmentByType();
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }
        }

        private void FilterEquipmentByRoomType()
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
            PrintEquipment(foundEquipment);
        }

        private void FilterEquipmentByQuantity()
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
            PrintEquipment(foundEquipment);
        }

        private void FilterEquipmentByType()
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
            PrintEquipment(foundEquipment);
        }

        private void ScheduleEquipmentMoving()
        {
            Console.WriteLine("Unesite podatke o pomeranju opreme");

            Console.Write("Unesite identifikator: ");
            string id = Console.ReadLine();
            while (equipmentMovingService.DoesIdExist(id)) 
            {
                Console.Write("Identifikator vec postoji. Ponovite unos: ");
                id = Console.ReadLine();
            }

            Console.Write("Unesite identifikator opreme: ");
            string equipmentId = Console.ReadLine();
            while (!equipmentService.DoesIdExist(equipmentId) || 
                equipmentMovingService.ActiveEquipmentMovingExist(equipmentId))
            {
                if (!equipmentService.DoesIdExist(equipmentId))
                    Console.Write("Identifikator ne postoji. Ponovite unos: ");
                else
                    Console.Write("Pomeranje odabrane opreme je vec zakazano. Ponovite unos: ");
                equipmentId = Console.ReadLine();
            }

            Console.Write("Unesite vreme pomeranja (u formatu MM/dd/yyyy HH:mm): ");
            bool isTimeValid = false;
            DateTime scheduledTime = DateTime.Now;
            do
            {
                string scheduledTimeStr = Console.ReadLine();
                isTimeValid = DateTime.TryParseExact(scheduledTimeStr, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out scheduledTime);
                if (!isTimeValid)
                    Console.Write("Vreme nije ispravno. Ponovite unos: ");
            } while (!isTimeValid);

            Equipment equipment = equipmentService.GetEquipmentById(equipmentId);
            string sourceRoomId = equipment.RoomId;

            Console.WriteLine("Unesite broj sobe u koju se oprema pomera: ");
            string destinationRoomId = Console.ReadLine();
            while (!roomService.DoesIdExist(destinationRoomId) || destinationRoomId.Equals(sourceRoomId))
            {
                if (destinationRoomId.Equals(sourceRoomId))
                    Console.Write("Nije moguce pomeriti opremu u istu sobu. Ponovite unos: ");
                else
                    Console.Write("Broj sobe ne postoji. Ponovite unos: ");
                destinationRoomId = Console.ReadLine();
            }

            equipmentMovingService.CreateEquipmentMoving(id, equipmentId, scheduledTime, sourceRoomId, destinationRoomId);
        }

        private void MoveEquipment()
        {
            equipmentMovingService.MoveEquipment();
        }

        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
