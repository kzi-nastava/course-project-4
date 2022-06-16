using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Service;
using Hospital.Appointments.Service;
using Hospital.Rooms.Model;
using Hospital;
using Autofac;
namespace Hospital.Rooms.View
{
    public class RenovationView
    {
        private IRoomService _roomService;
        private IAppointmentService _appointmentService;
        private IRenovationService _renovationService;

        public RenovationView()
        {
            this._roomService = Globals.container.Resolve<IRoomService>();
            this._appointmentService = Globals.container.Resolve<IAppointmentService>();
            this._renovationService = Globals.container.Resolve<IRenovationService>();
        }

        private string EnterRenovationId()
        {
            Console.Write("Unesite identifikator: ");
            string id = Console.ReadLine();
            while (_renovationService.IdExists(id))
            {
                Console.Write("Identifikator vec postoji. Ponovite unos: ");
                id = Console.ReadLine();
            }
            return id;
        }

        private string EnterRoomId(bool isOther)
        {
            if (isOther)
                Console.Write("Unesite broj druge sobe: ");
            else
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
            return roomId;
        }

        private DateTime EnterDate(bool isStart)
        {
            if (isStart)
                Console.Write("Unesite datum pocetka renoviranja (u formatu MM/dd/yyyy): ");
            else
                Console.Write("Unesite datum kraja renoviranja (u formatu MM/dd/yyyy): ");

            bool isDateValid = false;
            DateTime date = DateTime.Now;
            do
            {
                string startDateStr = Console.ReadLine();
                isDateValid = DateTime.TryParseExact(startDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date);
                if (!isDateValid)
                    Console.Write("Datum nije ispravan. Ponovite unos: ");
            } while (!isDateValid);
            return date;
        }

        public void ScheduleRenovation(Renovation.Type type)
        {
            Console.WriteLine("Unesite podatke o renoviranju");

            string id = EnterRenovationId();
            string roomId = EnterRoomId(false);

            string otherRoomId = "";
            if (type == Renovation.Type.MergeRenovation)
            {
                otherRoomId = EnterRoomId(true);
                while (roomId.Equals(otherRoomId))
                {
                    Console.WriteLine("Sobe koje se spajaju moraju biti razlicite!");
                    otherRoomId = EnterRoomId(true);
                }
            }

            DateTime startDate = EnterDate(true);
            DateTime endDate = EnterDate(false);

            while (endDate < startDate)
            {
                Console.WriteLine("Datum kraja ne moze biti pre datuma pocetka!");
                startDate = EnterDate(true);
                endDate = EnterDate(false);
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

        private Renovation.Type EnterRenovationType()
        {
            Console.WriteLine("Izaberite tip renoviranja");
            Console.WriteLine("1. Renoviranje sobe");
            Console.WriteLine("2. Razdvajanje sobe na dve");
            Console.WriteLine("3. Spajanje dve sobe");

            while (true)
            {
                Console.Write(">> ");
                string choice = Console.ReadLine();

                if (choice.Equals("1"))
                    return Renovation.Type.SimpleRenovation;
                else if (choice.Equals("2"))
                    return Renovation.Type.SplitRenovation;
                else if (choice.Equals("3"))
                    return Renovation.Type.MergeRenovation;
            }
        }

        public void ScheduleRenovation()
        {
            Renovation.Type type = EnterRenovationType();
            ScheduleRenovation(type);
        }

        public void Renovate()
        {
            _renovationService.Renovate();
        }
    }
}
