using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Service;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.PatientImplementation
{
    class PatientDoctorSearch
    {
        private Patient _currentRegisteredUser;
        private UserService _userService;

        public PatientDoctorSearch(Patient patient, UserService userService)
        {
            this._currentRegisteredUser = patient;
            this._userService = userService;
        }

        public void MenuForDoctorSearch()
        {
            string choice;
            Console.Write("\nPretraga doktora po");
            do
            {
                Console.WriteLine("\n1. Imenu");
                Console.WriteLine("2. Prezimenu");
                Console.WriteLine("3. Uzoj oblasti");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    this.FindDoctorsByName();
                else if (choice.Equals("2"))
                    this.FindDoctorsBySurname();
                else if (choice.Equals("3"))
                    this.FindDoctorsBySpeciality();
            } while (choice != "1" && choice != "2" && choice != "3");
        }

        private void HeaderDoctorTable()
        {
            Console.WriteLine();
            Console.Write(String.Format("{0,3}|{1,10}|{2,10}|{3,10}|{4,10}",
                "Br.", "Ime", "Prezime", "Email", "Specijalizacija")+"\n");
        }

        private void PrintDoctors(List<DoctorUser> doctors)
        {
            this.HeaderDoctorTable();
            for (int i = 0; i < doctors.Count; i++)
            {
                Console.WriteLine(String.Format("{0,3}|{1,10}|{2,10}|{3,10}|{4,10}",
                           i+1, doctors[i].Name, doctors[i].Surname, doctors[i].Email, doctors[i].SpecialityDoctor));
            }
        }

        private void FindDoctorByParameter(string searchParameter, int numberParameter)
        {
            List<DoctorUser> selectedDoctors = new List<DoctorUser>();
            Console.WriteLine(_userService.UsersRepository.DoctorUsers.Count);
            foreach (DoctorUser doctor in _userService.UsersRepository.DoctorUsers)
            {
                if (Utils.Capitalize(searchParameter).Equals(doctor.Name) && numberParameter == 0 ||
                    Utils.Capitalize(searchParameter).Equals(doctor.Surname) && numberParameter == 1 ||
                    Utils.Capitalize(searchParameter).Equals(doctor.SpecialityDoctor.ToString()) && numberParameter == 2)
               
                    selectedDoctors.Add(doctor);
            }

            if (selectedDoctors.Count == 0)
                Console.WriteLine("Ne postoji ni jedan doktor koji zadovoljava zadate parametre.");
            else
            {
                this.PrintDoctors(selectedDoctors);
                this.SortByParameter(selectedDoctors);
            }
        }

        private void FindDoctorsByName()
        {
            Console.Write("\nUnesite ime doktora: ");
            string inputName = Console.ReadLine();
            this.FindDoctorByParameter(inputName, 0);
        }

        private void FindDoctorsBySurname()
        {
            Console.Write("\nUnesite prezime doktora: ");
            string inputSurname = Console.ReadLine();
            this.FindDoctorByParameter(inputSurname, 1);
        }

        private void FindDoctorsBySpeciality()
        {
            Console.Write("\nUnesite uzu oblast doktora: ");
            string inputSpeciality = Console.ReadLine();
            this.FindDoctorByParameter(inputSpeciality, 2);
        }

        private void SortByParameter(List<DoctorUser> selectedDoctors)
        {
            string choice;
            Console.Write("\n\nSortiraj po");
            do
            {
                Console.WriteLine("\n1. Imenu");
                Console.WriteLine("2. Prezimenu");
                Console.WriteLine("3. Specijalizaciji");
                Console.WriteLine("4. Prosecnoj oceni");
                Console.Write(">> ");
                choice = Console.ReadLine();

                if (choice.Equals("1"))
                    this.SortByName(selectedDoctors);
                else if (choice.Equals("2"))
                    this.SortBySurname(selectedDoctors);
                else if (choice.Equals("3"))
                    this.SortBySpeciality(selectedDoctors);
                else if (choice.Equals("4"))
                    this.SortByGrades(selectedDoctors);
            } while (choice != "1" && choice != "2" && choice != "3" && choice != "4");
        }

        private void SortByName(List<DoctorUser> selectedDoctors)
        {
            selectedDoctors.Sort((x, y) => x.Name.CompareTo(y.Name));
            this.PrintDoctors(selectedDoctors);
            this.PickDoctorForScheduling(selectedDoctors);
        }

        private void SortBySurname(List<DoctorUser> selectedDoctors)
        {
            selectedDoctors.Sort((x, y) => x.Surname.CompareTo(y.Surname));
            this.PrintDoctors(selectedDoctors);
            this.PickDoctorForScheduling(selectedDoctors);
        }

        private void SortBySpeciality(List<DoctorUser> selectedDoctors)
        {
            selectedDoctors.Sort((x, y) => x.SpecialityDoctor.ToString().CompareTo(y.SpecialityDoctor.ToString()));
            this.PrintDoctors(selectedDoctors);
            this.PickDoctorForScheduling(selectedDoctors);
        }

        private void SortByGrades(List<DoctorUser> selectedDoctors)
        {
            IDictionary<string, double> averageGrades = _currentRegisteredUser.PatientDoctorSurvey.CalculateAverageDoctorGrade();
            averageGrades.OrderBy(key => key.Value);
            List<DoctorUser> sortedSelectedDoctors = new List<DoctorUser>();
            foreach (KeyValuePair<string, double> evalutedDoctor in averageGrades)
            {
                foreach (DoctorUser doctor in selectedDoctors)
                {
                    if (evalutedDoctor.Key.Equals(doctor.Email))
                    {
                        sortedSelectedDoctors.Add(doctor);
                        Console.Write(sortedSelectedDoctors.Count.ToString() + "| ");
                        Console.WriteLine("Doktor: {0}, Ocena: {1}", evalutedDoctor.Key, evalutedDoctor.Value);
                    }
                }
            }
            this.PickDoctorForScheduling(sortedSelectedDoctors);
        }

        private void PickDoctorForScheduling(List<DoctorUser> doctors)
        {
            string choice;
            int numDoctor;
            do
            {
                Console.WriteLine("\nUnesite broj doktora kod kog zelite da zakazete pregled");
                Console.Write(">> ");
                choice = Console.ReadLine();
            } while (!int.TryParse(choice, out numDoctor) || numDoctor < 1 || numDoctor > doctors.Count);

            _currentRegisteredUser.SchedulingAppointment(doctors[numDoctor-1].Email);
        }
    }
}
