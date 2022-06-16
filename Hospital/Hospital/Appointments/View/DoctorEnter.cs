using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Appointments.Model;
using Hospital.Rooms.Service;
using Hospital.Users.Service;

namespace Hospital.Appointments.View
{
    public class DoctorEnter
    {
        public static void PrintDoctorMenu()
        {
            Console.WriteLine("\n\tMENI");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Pregledaj sopstvene preglede/operacije");
            Console.WriteLine("2. Kreiraj sopstveni pregled/operaciju");
            Console.WriteLine("3. Izmeni sopstveni pregled/operaciju");
            Console.WriteLine("4. Obrisi sopstveni pregled/operaciju");
            Console.WriteLine("5. Ispitivanje sopstvenog rasporeda");
            Console.WriteLine("6. Izvodjenje pregleda");
            Console.WriteLine("7. Upravljanje lekovima");
            Console.WriteLine("8. Zahtevi za slobodne dane");
            Console.WriteLine("9. Odjava");
            Console.Write(">> ");

        }

        public static string EnterTypeOfTerm()
        {
            Console.WriteLine("Šta želite da kreirate: ");
            Console.WriteLine("1. Pregled");
            Console.WriteLine("2. Operaciju");
            Console.WriteLine(">> ");
            return Console.ReadLine();

        }

        public static string EnterUpdateNumberAppointment(List<Appointment> allMyAppointments)
        {
            string numberAppointment;
            int tryIntConvert;
            do
            {
                do
                {
                    Console.WriteLine("Unesite redni broj koji želite da promenite: ");
                    numberAppointment = Console.ReadLine();
                } while (!int.TryParse(numberAppointment, out tryIntConvert));
            } while (Int32.Parse(numberAppointment) > allMyAppointments.Count);
            return numberAppointment;
        }

        public static string EnterNumberAppointment(List<Appointment> allMyAppointments)
        {
            string numberAppointment;
            int tryIntConvert;
            do
            {
                do
                {
                    Console.WriteLine("Unesite redni broj koji želite da obrišete: ");
                    numberAppointment = Console.ReadLine();
                } while (!int.TryParse(numberAppointment, out tryIntConvert));
            } while (Int32.Parse(numberAppointment) > allMyAppointments.Count);
            return numberAppointment;

        }
        public static string EnterDurationOperation()
        {
            int tryIntConvert;
            string newDurationOperation;
            do
            {
                Console.WriteLine("Koliko će trajati operacija (u minutima): ");
                newDurationOperation = Console.ReadLine();

            } while (!int.TryParse(newDurationOperation, out tryIntConvert));
            return newDurationOperation;
        }

        public static string EnterRoomNumber(IRoomService roomService)
        {
            string newRoomNumber;
            do
            {
                Console.WriteLine("Unesite broj sobe: ");
                newRoomNumber = Console.ReadLine();
            } while (!roomService.IsRoomNumberValid(newRoomNumber));
            return newRoomNumber;
        }
        public static string EnterStartTime()
        {
            string newStartTime;
            do
            {
                Console.WriteLine("Unesite vreme pocetka pregleda/operacije (HH:mm): ");
                newStartTime = Console.ReadLine();
            } while (!Utils.IsTimeFormValid(newStartTime));
            return newStartTime;

        }
        public static string EnterDate()
        {
            string newDate;
            do
            {
                Console.WriteLine("Unesite datum (MM/dd/yyyy): ");
                newDate = Console.ReadLine();
            } while (!Utils.IsDateFormValid(newDate));
            return newDate;
        }

        public static string EnterPatientEmail(UserService userService)
        {
            string patientEmail;
            do
            {
                Console.WriteLine("Unesite email pacijenta: ");
                patientEmail = Console.ReadLine();
            } while (!userService.IsPatientEmailValid(patientEmail));
            return patientEmail;
        }

    }
}
