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
    public class Manager : IMenuView
    {
        private User _currentRegisteredManager;
        private RoomService _roomService;
        private EquipmentService _equipmentService;
        private EquipmentMovingService _equipmentMovingService;
        private AppointmentService _appointmentService;
        private RenovationService _renovationService;
        private IngredientService _ingredientService;
        private DrugProposalService _drugProposalService;

        private RoomView _roomView;
        private EquipmentView _equipmentView;
        private RenovationView _renovationView;
        private IngredientView _ingredientView;
        private DrugView _drugView;

        public Manager(User currentRegisteredManager)
        {
            this._currentRegisteredManager = currentRegisteredManager;
            this._roomService = new RoomService();
            this._equipmentService = new EquipmentService(_roomService);
            this._equipmentMovingService = new EquipmentMovingService(_equipmentService, _roomService);
            this._appointmentService = new AppointmentService();
            this._renovationService = new RenovationService(_roomService, _appointmentService, _equipmentService);
            this._ingredientService = new IngredientService();
            this._drugProposalService = new DrugProposalService();

            this._roomView = new RoomView(_roomService);
            this._equipmentView = new EquipmentView(_roomService, _equipmentService, _equipmentMovingService);
            this._renovationView = new RenovationView(_roomService, _appointmentService, _renovationService);
            this._ingredientView = new IngredientView(_ingredientService);
            this._drugView = new DrugView(_ingredientService, _drugProposalService);
        }

        public void ManagerMenu() 
        {
            string choice;
            Console.WriteLine("\n\tMENI");
            Console.Write("------------------");
            do
            {
                Console.WriteLine("\n1. Upravljanje sobama");
                Console.WriteLine("2. Pretraga opreme");
                Console.WriteLine("3. Filtriranje opreme");
                Console.WriteLine("4. Zakazi premestanje opreme");
                Console.WriteLine("5. Pokreni premestanje opreme");
                Console.WriteLine("6. Zakazivanje renoviranja");
                Console.WriteLine("7. Izvrsi renoviranja");
                Console.WriteLine("8. Upravljanje sastojcima");
                Console.WriteLine("9. Predlozi lek");
                Console.WriteLine("10. Pregledaj odbijene lekove");
                Console.WriteLine("11. Izmeni podatke o odbijenom leku");
                Console.WriteLine("12. Odjava");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    _roomView.ManageRooms();
                else if (choice.Equals("2"))
                    _equipmentView.SearchEquipment();
                else if (choice.Equals("3"))
                    _equipmentView.FilterEquipment();
                else if (choice.Equals("4"))
                    _equipmentView.ScheduleEquipmentMoving();
                else if (choice.Equals("5"))
                    _equipmentView.MoveEquipment();
                else if (choice.Equals("6"))
                    _renovationView.ScheduleRenovation();
                else if (choice.Equals("7"))
                    _renovationView.Renovate();
                else if (choice.Equals("8"))
                    _ingredientView.ManageIngredients();
                else if (choice.Equals("9"))
                    _drugView.ProposeDrug();
                else if (choice.Equals("10"))
                    _drugView.ListRejectedDrugProposals();
                else if (choice.Equals("11"))
                    _drugView.ReviewRejectedDrugProposal();
                else if (choice.Equals("12"))
                    this.LogOut();
            } while (true);
        } 
    }
}
