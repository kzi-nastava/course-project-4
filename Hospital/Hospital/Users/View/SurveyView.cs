using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Service;
using Hospital.Users.Model;

namespace Hospital.Users.View
{
    public class SurveyView : ISurveyView
    {
        private IDoctorSurveyService _doctorSurveyService;
        private IHospitalSurveyService _hospitalSurveyService;

        public SurveyView(IDoctorSurveyService doctorSurveyService, IHospitalSurveyService hospitalSurveyService)
        {
            _doctorSurveyService = doctorSurveyService;
            _hospitalSurveyService = hospitalSurveyService;
        }

        public void ViewSurveyResults()
        {
            Console.WriteLine("1. Rezultati anketa o bolnici");
            Console.WriteLine("2. Rezultati anketa o doktorima");
            Console.Write(">> ");
            string choice = Console.ReadLine();

            if (choice.Equals("1"))
                ViewHospitalSurveyResults();
            else if (choice.Equals("2"))
                ViewDoctorSurveyResults();
        }

        public void ViewHospitalSurveyResults()
        {
            HospitalSurveyResult result = _hospitalSurveyService.GetResult();
            Console.WriteLine("Rezultat ankete o bolnici");
            Console.WriteLine("-------------------------");
            Console.WriteLine("Broj anketa: " + result.SurveyCount);
            
            Console.Write("Prosecna ocena kvaliteta: " + result.AverageQuality + " ->");
            for (int i = 1; i <= 5; i++)
                Console.Write(" " + i + "(" + result.QualityCount[i] + ")");
            Console.WriteLine();

            Console.Write("Prosecna ocena cistoce: " + result.AverageCleanliness + " ->");
            for (int i = 1; i <= 5; i++)
                Console.Write(" " + i + "(" + result.CleanlinessCount[i] + ")");
            Console.WriteLine();

            Console.Write("Prosecna ocena zadovoljstva: " + result.AverageSatisfied + " ->");
            for (int i = 1; i <= 5; i++)
                Console.Write(" " + i + "(" + result.SatisfiedCount[i] + ")");
            Console.WriteLine();

            Console.Write("Prosecna ocena preporuke: " + result.AverageRecommendation + " ->");
            for (int i = 1; i <= 5; i++)
                Console.Write(" " + i + "(" + result.RecommendationCount[i] + ")");
            Console.WriteLine();

            Console.WriteLine("Komentari");
            Console.WriteLine("-----------------------");
            foreach (string comment in result.Comments)
            {
                Console.WriteLine(comment);
            }
            Console.WriteLine();
        }

        private void PrintDoctorSurveyResult(DoctorSurveyResult result)
        {
            Console.WriteLine("Ime: " + result.Doctor.Name);
            Console.WriteLine("Prezime: " + result.Doctor.Surname);
            Console.WriteLine("E-mail: " + result.Doctor.Email);
            Console.WriteLine("Broj anketa: " + result.SurveyCount);
            
            Console.Write("Prosecna ocena kvaliteta: " + result.AverageQuality + " ->");
            for (int i = 1; i <= 5; i++)
                Console.Write(" " + i + "(" + result.QualityCount[i] + ")");
            Console.WriteLine();
            
            Console.Write("Prosecna ocena preporuke: " + result.AverageRecommendation + " ->");
            for (int i = 1; i <= 5; i++)
                Console.Write(" " + i + "(" + result.RecommendationCount[i] + ")");
            Console.WriteLine();

            Console.WriteLine("Komentari");
            Console.WriteLine("-----------------------");
            foreach (string comment in result.Comments)
            {
                Console.WriteLine(comment);
            }
            Console.WriteLine();
        }

        private void PrintDoctors(List<DoctorSurveyResult> doctors)
        {
            foreach (DoctorSurveyResult doctor in doctors)
            {
                Console.WriteLine("Ime: " + doctor.Doctor.Name);
                Console.WriteLine("Prezime: " + doctor.Doctor.Surname);
                Console.WriteLine("E-mail: " + doctor.Doctor.Email);
                Console.WriteLine("Prosecna ocena kvaliteta: " + doctor.AverageQuality);
                Console.WriteLine();
            }
        }

        private void PrintBestDoctors()
        {
            Console.WriteLine("Najbolje ocenjeni doktori");
            Console.WriteLine("-------------------------");
            List<DoctorSurveyResult> bestDoctors = _doctorSurveyService.GetBestDoctors();
            PrintDoctors(bestDoctors);
        }

        private void PrintWorstDoctors()
        {
            Console.WriteLine("Najgore ocenjeni doktori");
            Console.WriteLine("-------------------------");
            List<DoctorSurveyResult> worstDoctors = _doctorSurveyService.GetWorstDoctors();
            PrintDoctors(worstDoctors);
        }

        public void ViewDoctorSurveyResults()
        {
            List<DoctorSurveyResult> results = _doctorSurveyService.GetResults();
            Console.WriteLine("Rezultati anketa o svim doktorima");
            Console.WriteLine("---------------------------------");
            
            foreach (DoctorSurveyResult result in results)
            {
                PrintDoctorSurveyResult(result);
            }

            PrintBestDoctors();
            PrintWorstDoctors();
        }
    }
}
