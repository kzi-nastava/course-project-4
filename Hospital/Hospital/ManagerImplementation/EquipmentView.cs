﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.ManagerImplementation
{
    public class EquipmentView
    {
        private RoomService _roomService;
        private EquipmentService _equipmentService;
        private EquipmentMovingService _equipmentMovingService;

        public EquipmentView(RoomService roomService, EquipmentService equipmentService, EquipmentMovingService equipmentMovingService) 
        {
            this._roomService = roomService;
            this._equipmentService = equipmentService;
            this._equipmentMovingService = equipmentMovingService;
        }

        private void PrintEquipment(List<Equipment> equipmentToPrint)
        {
            foreach (Equipment equipment in equipmentToPrint)
            {
                Console.WriteLine("Identifikator: " + equipment.Id + ", naziv: " + equipment.Name + ", tip opreme: "
                    + equipment.TypeDescription + ", kolicina: " + equipment.Quantity + ", broj sobe: " + equipment.RoomId);
            }
        }

        public void SearchEquipment()
        {
            Console.Write("Unesite karaktere pretrage: ");
            string query = Console.ReadLine();

            List<Equipment> foundEquipment = _equipmentService.Search(query);
            PrintEquipment(foundEquipment);
        }

        public void FilterEquipment()
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

        public void FilterEquipmentByRoomType()
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

        public void FilterEquipmentByQuantity()
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

        public void FilterEquipmentByType()
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

        public void ScheduleEquipmentMoving()
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

        public void MoveEquipment()
        {
            _equipmentMovingService.MoveEquipment();
        }
    }
}