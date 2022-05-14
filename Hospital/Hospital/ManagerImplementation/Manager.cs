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

        private RoomView _roomView;
        private EquipmentView _equipmentView;
        private RenovationView _renovationView;

        public Manager(User currentRegisteredManager)
        {
            this._currentRegisteredManager = currentRegisteredManager;
            this._roomService = new RoomService();
            this._equipmentService = new EquipmentService(_roomService);
            this._equipmentMovingService = new EquipmentMovingService(_equipmentService, _roomService);
            this._appointmentService = new AppointmentService();
            this._renovationService = new RenovationService(_roomService, _appointmentService, _equipmentService);
            this._roomView = new RoomView(_roomService);
            this._equipmentView = new EquipmentView(_roomService, _equipmentService, _equipmentMovingService);
            this._renovationView = new RenovationView(_roomService, _appointmentService, _renovationService);
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
                Console.WriteLine("10. Izvrsi renoviranja");
                Console.WriteLine("11. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    _roomView.CreateRoom();
                else if (choice.Equals("2"))
                    _roomView.ListRooms();
                else if (choice.Equals("3"))
                    _roomView.UpdateRoom();
                else if (choice.Equals("4"))
                    _roomView.DeleteRoom();
                else if (choice.Equals("5"))
                    _equipmentView.SearchEquipment();
                else if (choice.Equals("6"))
                    _equipmentView.FilterEquipment();
                else if (choice.Equals("7"))
                    _equipmentView.ScheduleEquipmentMoving();
                else if (choice.Equals("8"))
                    _equipmentView.MoveEquipment();
                else if (choice.Equals("9"))
                    _renovationView.ScheduleRenovation();
                else if (choice.Equals("10"))
                    _renovationView.Renovate();
                else if (choice.Equals("11"))
                    this.LogOut();
            } while (true);
        } 

        private void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
