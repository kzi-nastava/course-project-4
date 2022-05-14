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
        private User _currentRegisteredManager;
        private RoomService _roomService;
        private EquipmentService _equipmentService;
        private EquipmentMovingService _equipmentMovingService;
        private AppointmentService _appointmentService;
        private RenovationService _renovationService;

        public Manager(User currentRegisteredManager)
        {
            this._currentRegisteredManager = currentRegisteredManager;
            this._roomService = new RoomService();
            this._equipmentService = new EquipmentService(_roomService);
            this._equipmentMovingService = new EquipmentMovingService(_equipmentService, _roomService);
            this._appointmentService = new AppointmentService();
            this._renovationService = new RenovationService(_roomService, _appointmentService);
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
                Console.WriteLine("9. Zakazivanje renoviranja");
                Console.WriteLine("10. Odjava");
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
                    this.ScheduleRenovation();
                else if (choice.Equals("10"))
                    this.LogOut();
            } while (true);
        }

        private void CreateRoom() 
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

        private void ListRooms()
        {
            List<Room> allRooms = _roomService.AllRooms;
            foreach (Room room in allRooms)
            {
                Console.WriteLine("Broj sobe: " + room.Id + ", naziv sobe: " + room.Name + ", tip sobe: " + room.TypeDescription);
            }
        }

        private void UpdateRoom()
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

        private void DeleteRoom()
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

        private void PrintEquipment(List<Equipment> equipmentToPrint)
        {
            foreach (Equipment equipment in equipmentToPrint)
            {
                Console.WriteLine("Identifikator: " + equipment.Id + ", naziv: " + equipment.Name + ", tip opreme: "
                    + equipment.TypeDescription + ", kolicina: " + equipment.Quantity + ", broj sobe: " + equipment.RoomId);
            }
        }

        private void SearchEquipment()
        {
            Console.Write("Unesite karaktere pretrage: ");
            string query = Console.ReadLine();

            List<Equipment> foundEquipment = _equipmentService.Search(query);
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

            Room.Type roomType = Room.Type.Other;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    roomType = Room.Type.OperationRoom;
                else if (choice.Equals("2"))
                    roomType = Room.Type.ExaminationRoom;
                else if (choice.Equals("3"))
                    roomType = Room.Type.RestRoom;
                else if (choice.Equals("4"))
                    roomType = Room.Type.Other;
                else if (choice.Equals("5"))
                    roomType = Room.Type.Warehouse;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            List<Equipment> foundEquipment = _equipmentService.FilterByRoomType(roomType);
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

            List<Equipment> foundEquipment = _equipmentService.FilterByQuantity(lowerBound, upperBound);
            PrintEquipment(foundEquipment);
        }

        private void FilterEquipmentByType()
        {
            Console.WriteLine("Odaberite tip opreme");
            Console.WriteLine("1. Oprema za preglede");
            Console.WriteLine("2. Oprema za operacije");
            Console.WriteLine("3. Sobni namestaj");
            Console.WriteLine("4. Oprema za hodnike");
            
            Equipment.Type equipmentType = Equipment.Type.ExaminationEquipment;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    equipmentType = Equipment.Type.ExaminationEquipment;
                else if (choice.Equals("2"))
                    equipmentType = Equipment.Type.OperationEquipment;
                else if (choice.Equals("3"))
                    equipmentType = Equipment.Type.Furniture;
                else if (choice.Equals("4"))
                    equipmentType = Equipment.Type.HallwayEquipment;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            List<Equipment> foundEquipment = _equipmentService.FilterByEquipmentType(equipmentType);
            PrintEquipment(foundEquipment);
        }

        private void ScheduleEquipmentMoving()
        {
            Console.WriteLine("Unesite podatke o pomeranju opreme");

            Console.Write("Unesite identifikator: ");
            string id = Console.ReadLine();
            while (_equipmentMovingService.IdExists(id)) 
            {
                Console.Write("Identifikator vec postoji. Ponovite unos: ");
                id = Console.ReadLine();
            }

            Console.Write("Unesite identifikator opreme: ");
            string equipmentId = Console.ReadLine();
            while (!_equipmentService.IdExist(equipmentId) || 
                _equipmentMovingService.ActiveMovingExists(equipmentId))
            {
                if (!_equipmentService.IdExist(equipmentId))
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

            Equipment equipment = _equipmentService.GetEquipmentById(equipmentId);
            string sourceRoomId = equipment.RoomId;

            Console.Write("Unesite broj sobe u koju se oprema pomera: ");
            string destinationRoomId = Console.ReadLine();
            while (!_roomService.IdExists(destinationRoomId) || destinationRoomId.Equals(sourceRoomId))
            {
                if (destinationRoomId.Equals(sourceRoomId))
                    Console.Write("Nije moguce pomeriti opremu u istu sobu. Ponovite unos: ");
                else
                    Console.Write("Broj sobe ne postoji. Ponovite unos: ");
                destinationRoomId = Console.ReadLine();
            }

            _equipmentMovingService.CreateEquipmentMoving(id, equipmentId, scheduledTime, sourceRoomId, destinationRoomId);
        }

        private void MoveEquipment()
        {
            _equipmentMovingService.MoveEquipment();
        }

        private void ScheduleRenovation(Renovation.Type type)
        {
            Console.WriteLine("Unesite podatke o renoviranju");

            Console.Write("Unesite identifikator: ");
            string id = Console.ReadLine();
            while (_renovationService.IdExists(id))
            {
                Console.Write("Identifikator vec postoji. Ponovite unos: ");
                id = Console.ReadLine();
            }

            Console.Write("Unesite broj sobe: ");
            string roomId = Console.ReadLine();
            while (!_roomService.IdExists(roomId) || _renovationService.ActiveRenovationExists(roomId))
            {
                if (!_roomService.IdExists(roomId))
                    Console.Write("Identifikator ne postoji. Ponovite unos: ");
                else
                    Console.Write("Renoviranje za trazenu sobu je vec zakazano. Ponovite unos: ");
                roomId = Console.ReadLine();
            }

            string otherRoomId = "";
            if (type == Renovation.Type.MergeRenovation) 
            {
                Console.Write("Unesite broj druge sobe: ");
                otherRoomId = Console.ReadLine();
                while (!_roomService.IdExists(otherRoomId) || _renovationService.ActiveRenovationExists(otherRoomId))
                {
                    if (!_roomService.IdExists(otherRoomId))
                        Console.Write("Identifikator ne postoji. Ponovite unos: ");
                    else
                        Console.Write("Renoviranje za trazenu sobu je vec zakazano. Ponovite unos: ");
                    otherRoomId = Console.ReadLine();
                }
            }

            Console.Write("Unesite datum pocetka renoviranja (u formatu MM/dd/yyyy): ");
            bool isDateValid = false;
            DateTime startDate = DateTime.Now;
            do
            {
                string startDateStr = Console.ReadLine();
                isDateValid = DateTime.TryParseExact(startDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out startDate);
                if (!isDateValid)
                    Console.Write("Datum nije ispravan. Ponovite unos: ");
            } while (!isDateValid);

            Console.Write("Unesite datum kraja renoviranja (u formatu MM/dd/yyyy): ");
            isDateValid = false;
            DateTime endDate = DateTime.Now;
            do
            {
                string endDateStr = Console.ReadLine();
                isDateValid = DateTime.TryParseExact(endDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out endDate);
                if (!isDateValid)
                    Console.Write("Datum nije ispravan. Ponovite unos: ");
            } while (!isDateValid);

            if (endDate < startDate)
            {
                Console.WriteLine("Datum kraja ne moze biti pre datuma pocetka!");
                return;
            }

            if (_appointmentService.OverlapingAppointmentExists(startDate, endDate, roomId))
            {
                Console.WriteLine("Zakazani pregled ili operacija se poklapa sa vremenom renoviranja.");
                Console.WriteLine("Zakazivanje nije uspelo!");
                return;
            }

            if (type == Renovation.Type.SimpleRenovation)
                _renovationService.CreateSimpleRenovation(id, startDate, endDate, roomId);
            else if (type == Renovation.Type.SplitRenovation)
                _renovationService.CreateSplitRenovation(id, startDate, endDate, roomId);
            else
                _renovationService.CreateMergeRenovation(id, startDate, endDate, roomId, otherRoomId);
        }

        private void ScheduleRenovation()
        {
            Console.WriteLine("Izaberite tip renoviranja");
            Console.WriteLine("1. Renoviranje sobe");
            Console.WriteLine("2. Razdvajanje sobe na dve");
            Console.WriteLine("3. Spajanje dve sobe");

            Renovation.Type type = Renovation.Type.SimpleRenovation;
            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                bool shouldBreak = true;
                if (choice.Equals("1"))
                    type = Renovation.Type.SimpleRenovation;
                else if (choice.Equals("2"))
                    type = Renovation.Type.SplitRenovation;
                else if (choice.Equals("3"))
                    type = Renovation.Type.MergeRenovation;
                else
                    shouldBreak = false;

                if (shouldBreak)
                    break;
            }

            ScheduleRenovation(type);
        }

        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
