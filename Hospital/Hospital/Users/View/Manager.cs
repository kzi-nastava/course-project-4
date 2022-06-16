using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Hospital;
using Hospital.Users.Model;
using Hospital.Rooms.Service;
using Hospital.Appointments.Service;
using Hospital.Drugs.Service;
using Hospital.Users.Service;
using Hospital.Rooms.View;
using Hospital.Drugs.View;
using Hospital.Rooms.Repository;
using Hospital.Drugs.Repository;
using Hospital.Users.Repository;

namespace Hospital.Users.View
{
    public class Manager : IMenuView
    {
        private User _currentRegisteredManager;
        private IRoomService _roomService;
        private IEquipmentService _equipmentService;
        private IEquipmentMovingService _equipmentMovingService;
        private IAppointmentService _appointmentService;
        private IRenovationService _renovationService;
        private IIngredientService _ingredientService;
        private IDrugProposalService _drugProposalService;
        private IDoctorSurveyService _doctorSurveyService;
        private HospitalSurveyService _hospitalSurveyService;

        private RoomView _roomView;
        private EquipmentView _equipmentView;
        private RenovationView _renovationView;
        private IngredientView _ingredientView;
        private DrugView _drugView;
        private SurveyView _surveyView;

        public Manager(User currentRegisteredManager)
        {
            this._currentRegisteredManager = currentRegisteredManager;
            this._roomService = Globals.container.Resolve<IRoomService>();

            EquipmentRepository equipmentRepository = new EquipmentRepository();

            this._equipmentService = Globals.container.Resolve<IEquipmentService>();
            this._equipmentMovingService = Globals.container.Resolve<IEquipmentMovingService>();
            this._appointmentService = Globals.container.Resolve<IAppointmentService>();
            this._renovationService = Globals.container.Resolve<IRenovationService>();

            IIngredientRepository ingredientRepository = Globals.container.Resolve<IIngredientRepository>();
            this._ingredientService = new IngredientService();
            IDrugProposalRepository drugProposalRepository = Globals.container.Resolve<IDrugProposalRepository>();
            this._drugProposalService = new DrugProposalService();

            IDoctorSurveyRepository doctorSurveyRepository = new DoctorSurveyRepository();
            this._doctorSurveyService = Globals.container.Resolve<IDoctorSurveyService>();
            IHospitalSurveyRepository hospitalSurveyRepository = new HospitalSurveyRepository();
            this._hospitalSurveyService = new HospitalSurveyService("");

            this._roomView = new RoomView();
            this._equipmentView = new EquipmentView();
            this._renovationView = new RenovationView();
            this._ingredientView = new IngredientView();
            this._drugView = new DrugView();
            this._surveyView = new SurveyView();
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
                Console.WriteLine("12. Pregledaj rezultate anketa");
                Console.WriteLine("13. Odjava");
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
                    _surveyView.ViewSurveyResults();
                else if (choice.Equals("13"))
                    this.LogOut();
            } while (true);
        }
    }
}