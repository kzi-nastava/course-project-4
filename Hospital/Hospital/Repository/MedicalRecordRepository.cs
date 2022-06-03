using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    class MedicalRecordRepository
    {
        public List<MedicalRecord> Load()
        {
            List<MedicalRecord> allMedicalRecords = new List<MedicalRecord>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\medicalRecords.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string idHealthRecord = fields[0];
                    string anamnesis = fields[1];
                    string referralToDoctor = fields[2];

                    MedicalRecord newMedicalRecord = new MedicalRecord(idHealthRecord, anamnesis, referralToDoctor);
                    allMedicalRecords.Add(newMedicalRecord);

                }
            }
            return allMedicalRecords;
        }

        public void Save(List<MedicalRecord> medicalRecords)
        {
            string filePath = @"..\..\Data\medicalRecords.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (MedicalRecord medicalRecord in medicalRecords)
            {
                line = medicalRecord.IdAppointment + ";" + medicalRecord.Anamnesis + ";" + medicalRecord.ReferralToDoctor;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }

    }
}
