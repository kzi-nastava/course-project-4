using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

using Hospital.Appointments.Service;
using Hospital.Users.Service;
using Hospital.Appointments.Model;
using Hospital.Users.Model;
using Hospital.Rooms.Service;
using Hospital.Appointments.View;
using Hospital.Drugs.View;
using Hospital;
using Autofac;
namespace Hospital.Users.View
{
    public class Doctor : IMenuView
    {
        private IAppointmentService _appointmentService;
        private IUserService _userService;
        private IHealthRecordService _healthRecordService;
        private List<HealthRecord> _healthRecords;
        private List<Appointment> _allMyAppointments;
        private User _currentRegisteredDoctor;
        private IRoomService _roomService;

        public Doctor(User currentRegisteredDoctor)
        {
            this._appointmentService = Globals.container.Resolve<IAppointmentService>();
            this._healthRecordService = Globals.container.Resolve<IHealthRecordService>();
            this._userService = Globals.container.Resolve<IUserService>();
            this._roomService = Globals.container.Resolve<IRoomService>();
            this._currentRegisteredDoctor = currentRegisteredDoctor;
            this._healthRecords = _healthRecordService.HealthRecords;
            this._allMyAppointments = _appointmentService.GetDoctorAppointment(currentRegisteredDoctor);

        }
        public void DoctorMenu()
        {
            string choice;
            do
            {
                DoctorEnter.PrintDoctorMenu();
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    this.ReadOwnAppointment();
                }
                else if (choice.Equals("2"))
                {
                    this.CreateOwnAppointment();
                }
                else if (choice.Equals("3"))
                {
                    this.UpdateOwnAppointment();
                }
                else if (choice.Equals("4"))
                {
                    this.DeleteOwnAppointment();
                }
                else if (choice.Equals("5"))
                {
                    this.ExaminingOwnSchedule();
                }
                else if (choice.Equals("6"))
                {
                    DoctorPerformingAppointment doctorPerforming = new DoctorPerformingAppointment(_healthRecords, _currentRegisteredDoctor, _allMyAppointments);
                    doctorPerforming.PerformingAppointment();
                }
                else if (choice.Equals("7"))
                {
                    DrugVerification drugVerification = new DrugVerification();
                    drugVerification.DisplayDrugsForVerification();
                }
                else if (choice.Equals("8"))
                {
                    DoctorDaysOff doctorDaysOff = new DoctorDaysOff(_currentRegisteredDoctor);
                    doctorDaysOff.MenuForDaysOff();
                }
                else if (choice.Equals("9"))
                {
                    this.LogOut();
                }
                else { Console.WriteLine("Unesite validnu radnju!"); }
            } while (true);
        }
        private void ExaminingOwnSchedule()
        {
            string dateAppointment;
            do
            {
                Console.WriteLine("Unesite željeni datum: ");
                dateAppointment = Console.ReadLine();
            } while (!Utils.IsDateFormValid(dateAppointment));
            DoctorSchedule doctorSchedule = new DoctorSchedule(_healthRecords, _allMyAppointments, _currentRegisteredDoctor);
            doctorSchedule.ReadOwnAppointmentSpecificDate(DateTime.ParseExact(dateAppointment, "MM/dd/yyyy", CultureInfo.InvariantCulture));
        }
        private void ReadOwnAppointment()
        {
            if (this._allMyAppointments.Count == 0)
            {
                Console.WriteLine("\nJos uvek nemate zakazan termin!");
                return;
            }
            Console.WriteLine("\n\tPREGLEDI I OPERACIJE\n");
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.", "Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
            int serialNumberAppointment = 1;
            foreach (Appointment appointment in this._allMyAppointments)
            {
                if (CheckOwnAppointment(appointment))
                {
                    Console.WriteLine(appointment.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;
                }
            }
        }
        private bool CheckOwnAppointment(Appointment appointment)
        {
            if ((appointment.AppointmentState == Appointment.State.Created ||
                    appointment.AppointmentState == Appointment.State.Updated) &&
                    appointment.DateAppointment > DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public void CreateOwnAppointment()
        {
            string typeOfTerm = DoctorEnter.EnterTypeOfTerm();
            Appointment newAppointment;
            if (typeOfTerm.Equals("1"))
            {
                do
                {
                    newAppointment = PrintItemsToEnterExamination(typeOfTerm);
                } while (!_appointmentService.IsAppointmentFreeForDoctor(newAppointment));
            }
            else if (typeOfTerm.Equals("2"))
            {
                do
                {
                    newAppointment = PrintItemsToEnterOperation(typeOfTerm);
                } while (!_appointmentService.IsAppointmentFreeForDoctor(newAppointment));
            }
            else
            {
                Console.WriteLine("Unesite validnu radnju!");
                return;
            }
            _appointmentService.Add(newAppointment);
            this._allMyAppointments = _appointmentService.GetDoctorAppointment(this._currentRegisteredDoctor);
            Console.WriteLine("Uspešno ste zakazali termin.");
        }
        private Appointment PrintItemsToEnterExamination(string typeOfTerm)
        {
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
            string patientEmail = DoctorEnter.EnterPatientEmail(_userService);
            string newDate = DoctorEnter.EnterDate();
            string newStartTime = DoctorEnter.EnterStartTime();
            string newRoomNumber = DoctorEnter.EnterRoomNumber(_roomService);
            dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
            newEndTime = startTime.AddMinutes(15);
            roomNumber = Int32.Parse(newRoomNumber);
            int id = _appointmentService.GetNewAppointmentId();
            return new Appointment(id.ToString(), patientEmail, _currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm), false, false);
        }
        private Appointment PrintItemsToEnterOperation(string typeOfTerm)
        {
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
            string patientEmail = DoctorEnter.EnterPatientEmail(_userService);
            string newDate = DoctorEnter.EnterDate();
            string newStartTime = DoctorEnter.EnterStartTime();
            string newDurationOperation = DoctorEnter.EnterDurationOperation();
            string newRoomNumber = DoctorEnter.EnterRoomNumber(_roomService);
            dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
            newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
            roomNumber = Int32.Parse(newRoomNumber);
            int id = _appointmentService.GetNewAppointmentId();
            return new Appointment(id.ToString(), patientEmail, _currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Created, roomNumber, (Appointment.Type)int.Parse(typeOfTerm), false, false);
        }
        private Appointment PrintItemsToChangeOperation(Appointment appontment)
        {
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
            string patientEmail = DoctorEnter.EnterPatientEmail(_userService);
            string newDate = DoctorEnter.EnterDate();
            string newStartTime = DoctorEnter.EnterStartTime();
            string newDurationOperation = DoctorEnter.EnterDurationOperation();
            string newRoomNumber = DoctorEnter.EnterRoomNumber(_roomService);
            dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
            newEndTime = startTime.AddMinutes(Int32.Parse(newDurationOperation));
            roomNumber = Int32.Parse(newRoomNumber);
            return new Appointment(appontment.AppointmentId, patientEmail, _currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Updated, roomNumber, appontment.TypeOfTerm, false, appontment.Urgent);
        }
        private Appointment PrintItemsToChangeExamination(Appointment appointment)
        {
            DateTime dateOfAppointment, startTime, newEndTime;
            int roomNumber;
            string patientEmail = DoctorEnter.EnterPatientEmail(this._userService);
            string newDate = DoctorEnter.EnterDate();
            string newStartTime = DoctorEnter.EnterStartTime();
            string newRoomNumber = DoctorEnter.EnterRoomNumber(this._roomService);
            dateOfAppointment = DateTime.ParseExact(newDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            startTime = DateTime.ParseExact(newStartTime, "HH:mm", CultureInfo.InvariantCulture);
            newEndTime = startTime.AddMinutes(15);
            roomNumber = Int32.Parse(newRoomNumber);
            return new Appointment(appointment.AppointmentId, patientEmail, _currentRegisteredDoctor.Email, dateOfAppointment, startTime, newEndTime, Appointment.State.Updated, roomNumber, appointment.TypeOfTerm, false, appointment.Urgent);
        }
        private void DeleteOwnAppointment()
        {
            if (this._allMyAppointments.Count != 0)
            {
                this.ReadOwnAppointment();
                string numberAppointment = DoctorEnter.EnterNumberAppointment(this._allMyAppointments);
                Appointment appointmentForDelete = this._allMyAppointments[Int32.Parse(numberAppointment) - 1];
                appointmentForDelete.AppointmentState = Appointment.State.Deleted;
                _appointmentService.Update(appointmentForDelete);
                this._allMyAppointments = _appointmentService.GetDoctorAppointment(this._currentRegisteredDoctor);
                Console.WriteLine("Uspesno ste obrisali termin!");
            }
            else { Console.WriteLine("\nJos uvek nemate zakazan termin!"); }
        }
        private void UpdateOwnAppointment()
        {
            if (this._allMyAppointments.Count != 0)
            {
                this.ReadOwnAppointment();
                string numberAppointment = DoctorEnter.EnterUpdateNumberAppointment(this._allMyAppointments);
                Appointment appointmentForUpdate = this._allMyAppointments[Int32.Parse(numberAppointment) - 1];
                Appointment newAppointment;
                if (appointmentForUpdate.TypeOfTerm == Appointment.Type.Examination) { newAppointment = this.PrintItemsToChangeExamination(appointmentForUpdate); }
                else { newAppointment = this.PrintItemsToChangeOperation(appointmentForUpdate); }
                _appointmentService.Update(newAppointment);
                this._allMyAppointments = _appointmentService.GetDoctorAppointment(this._currentRegisteredDoctor);
                Console.WriteLine("Uspesno ste izmenili termin!");
            }
            else { Console.WriteLine("\nJos uvek nemate zakazan termin!"); }
        }
    }
}
