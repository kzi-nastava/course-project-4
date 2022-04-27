using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using Hospital.PatientImplementation;
using Hospital.DoctorImplementation;

namespace Hospital
{
    public class App
    { 
        static void Main(string[] args)
        {
            Console.Write("Prijava na sistem\n");
            Console.Write("------------------\n");

            UserService userService = new UserService();
            User user;

            while (true)
            {
                Console.Write("Unesite email: ");
                string email = Console.ReadLine();

                if (!userService.IsEmailValid(email))
                {
                    Console.WriteLine("Email nije validan!");
                }
                else if (userService.IsUserBlocked(email))
                {
                    Console.WriteLine("Korisnik je blokiran. Prijava nije moguca!");
                }
                else
                {
                    Console.Write("Unesite lozinku: ");
                    string password = Console.ReadLine();

                    user = userService.TryLogin(email, password);
                    if (user == null)
                    {
                        Console.WriteLine("Pogresna lozinka!");
                    }
                    else
                    {
                        break;
                    }
                }
                Console.Write("------------------\n");
            }

            Console.WriteLine("Uspesno ste se ulogovali!");
            Console.WriteLine($"\nDobrodosli {user.Email}");

            if (user.UserRole == User.Role.Patient) {
                Helper helper = new Helper(user, userService.Users);

                // patient
                Patient registeredPatient = new Patient(user.Email, helper);
                registeredPatient.patientMeni();
            }else if (user.UserRole == User.Role.Doctor) {
                Helper helper = new Helper(user, userService.Users);
                //doctor
                Doctor registeredDoctor = new Doctor(user, helper);
                registeredDoctor.doctorMenu();
            }
        }
    }
}
