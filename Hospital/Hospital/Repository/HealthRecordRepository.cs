using Hospital.Model;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repository
{
    class HealthRecordRepository
    {
        public List<HealthRecord> Load()
        {
            List<HealthRecord> allMedicalRecords = new List<HealthRecord>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\healthRecords.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("*");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    string emailPatient = fields[1];
                    int patientHeight = Int32.Parse(fields[2]);
                    double patientWeight = Double.Parse(fields[3]);
                    string previousIllnesses = fields[4];
                    string allergen = fields[5];
                    string bloodType = fields[6];
       

                    HealthRecord newMedicalRecord = new HealthRecord(id, emailPatient, patientHeight, patientWeight, previousIllnesses, allergen, bloodType);
                    allMedicalRecords.Add(newMedicalRecord);

                }
            }
            return allMedicalRecords;
        }
        public void Save(List<HealthRecord> healthRecords)
        {
            string filePath = @"..\..\Data\healthRecords.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (HealthRecord healthRecord in healthRecords)
            {
                line = healthRecord.ToStringForFile();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
