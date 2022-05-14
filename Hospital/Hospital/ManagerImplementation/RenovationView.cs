using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.ManagerImplementation
{
    public class RenovationView
    {
        private RoomService _roomService;
        private AppointmentService _appointmentService;
        private RenovationService _renovationService;

        public RenovationView(RoomService roomService, AppointmentService appointmentService, RenovationService renovationService)
        {
            this._roomService = roomService;
            this._appointmentService = appointmentService;
            this._renovationService = renovationService;
        }

        public void ScheduleRenovation(Renovation.Type type)
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

        public void ScheduleRenovation()
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

        public void Renovate()
        {
            _renovationService.Renovate();
        }
    }
}
