using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Hospital.Users.Model;
using Hospital.Appointments.Model;

namespace Hospital.Users.Repository
{
    public class DoctorSurveyRepository: IDoctorSurveyRepository
    {
        public List<DoctorSurvey> Load()
        {
            List<DoctorSurvey> evaluatedDoctors = new List<DoctorSurvey>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\doctorSurvey.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string idAppointment = fields[0];
                    string patientEmail = fields[1];
                    string doctorEmail = fields[2];
                    int quality = Int32.Parse(fields[3]);
                    int recommendation = Int32.Parse(fields[4]);
                    string comment = fields[5];

                    DoctorSurvey evaluatedDoctor = new DoctorSurvey(idAppointment, patientEmail, doctorEmail, quality, recommendation, comment);
                    evaluatedDoctors.Add(evaluatedDoctor);

                }
            }
            return evaluatedDoctors;
        }

        public void Save(List<DoctorSurvey> doctorSurveys)
        {
            string filePath = @"..\..\Data\doctorSurvey.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (DoctorSurvey doctorSurvey in doctorSurveys)
            {
                line = doctorSurvey.IdAppointment + ";" + doctorSurvey.PatientEmail + ";" + doctorSurvey.DoctorEmail + ";" +
                    doctorSurvey.Quality + ";" + doctorSurvey.Recommendation + ";" + doctorSurvey.Comment;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
