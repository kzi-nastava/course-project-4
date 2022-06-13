using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Hospital.Users.Model;
using System.IO;
namespace Hospital.Users.Repository
{
    class HospitalSurveyRepository
    {
        public List<HospitalSurvey> Load()
        {
            List<HospitalSurvey> surveyResults = new List<HospitalSurvey>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\hospitalSurvey.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string patientEmail = fields[0];
                    int quality = Int32.Parse(fields[1]);
                    int cleanliness = Int32.Parse(fields[2]);
                    int satisfied = Int32.Parse(fields[3]);
                    int recommendation = Int32.Parse(fields[4]);
                    string comment = fields[5];

                    HospitalSurvey result = new HospitalSurvey(patientEmail, quality, cleanliness, quality, recommendation, comment);
                    surveyResults.Add(result);

                }
            }
            return surveyResults;
        }

        public HospitalSurvey InputValuesForServey(string patientEmail)
        {
            Console.WriteLine("\nBolnicu ocenjujete ocenama od 1 do 5");
            Console.Write("\nKvalitet usluga bolnice: ");
            int quality = Int32.Parse(Console.ReadLine());
            Console.Write("Koliko je cista bolnica? : ");
            int cleanliness = Int32.Parse(Console.ReadLine());
            Console.Write("Da li ste zadovoljni? : ");
            int satisfied = Int32.Parse(Console.ReadLine());
            Console.Write("Da li biste predlozili bolnicu prijateljima? : ");
            int recommendation = Int32.Parse(Console.ReadLine());
            Console.Write("Komentar: ");
            string comment = Console.ReadLine();

            HospitalSurvey hospitalSurvey =
                new HospitalSurvey(patientEmail, quality, cleanliness, satisfied, recommendation, comment);

            return hospitalSurvey;
        }

        public void Save(List<HospitalSurvey> hospitalSurveys)
        {
            string filePath = @"..\..\Data\hospitalSurvey.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (HospitalSurvey hospitalSurvey in hospitalSurveys)
            {
                line = hospitalSurvey.PatientEmail + ";" + hospitalSurvey.Quality + ";" + hospitalSurvey.Cleanliness + ";" +
                    hospitalSurvey.Satisfied + ";" + hospitalSurvey.Recommendation + ";" + hospitalSurvey.Comment;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
