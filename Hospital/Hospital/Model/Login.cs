using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Service;
using Hospital.PatientImplementation;
using Hospital.DoctorImplementation;
using Hospital.SecretaryImplementation;
using Hospital.ManagerImplementation;

namespace Hospital.Model
{
    class Login
    {
        UserService userService = new UserService();
        User registeredUser;

        public void logIn()
        {
            Console.WriteLine("\nPrijava na sistem");
            Console.WriteLine("------------------");

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

                    this.registeredUser = userService.TryLogin(email, password);
                    if (this.registeredUser == null)
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
            Console.WriteLine($"\nDobrodosli {this.registeredUser.Name + " " + this.registeredUser.Surname}");

            Helper helper = new Helper(this.registeredUser, userService.Users);
            if (this.registeredUser.UserRole == User.Role.Patient)
            { 
                // patient
                Patient registeredPatient = new Patient(this.registeredUser.Email, helper);
                registeredPatient.patientMenu();
            }
            else if (this.registeredUser.UserRole == User.Role.Doctor)
            {
                // doctor
                Doctor registeredDoctor = new Doctor(this.registeredUser, helper);
                registeredDoctor.doctorMenu();
            }
            else if (this.registeredUser.UserRole == User.Role.Secretary)
			{
                // secretary
                Secretary registeredSecretary = new Secretary(this.userService);
                registeredSecretary.SecretaryMenu();
			}
            else if (this.registeredUser.UserRole == User.Role.Manager)
            {
                // manager
                Manager registeredManager = new Manager(this.registeredUser);
                registeredManager.ManagerMenu();
            }
        }
    }
}
