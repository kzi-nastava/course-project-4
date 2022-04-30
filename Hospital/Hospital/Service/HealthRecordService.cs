using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;
using System.IO;

namespace Hospital.Service
{
    class HealthRecordService
    {
        private HealthRecordRepository healthRecordRepository;
        private UserRepository userRepository;
        private List<HealthRecord> healthRecords;
        private List<User> users;


        public HealthRecordService()
        {
            healthRecordRepository = new HealthRecordRepository();
            userRepository = new UserRepository();
            healthRecords = healthRecordRepository.Load();
            users = userRepository.Load();

        }

        public List<HealthRecord> HealthRecords { get{ return healthRecords; } }

        public HealthRecordRepository HealthRecordRepository { get { return healthRecordRepository; } }

        public void UpdateHealthRecordFile()
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

        public void CreateHealthRecord(User patient)
        {
            List<HealthRecord> healthRecords = this.HealthRecords;
            int id = healthRecords.Count + 1;
            HealthRecord newHealthRecord = new HealthRecord(id.ToString(), patient.Email, 0, 0, "0", "0", "0");
            this.HealthRecords.Add(newHealthRecord);
            this.UpdateHealthRecordFile();

        }

        public void UpdateHealthRecord(HealthRecord healthRecord)
        {
            string filePath = @"..\..\Data\healthRecords.csv";
            string[] lines = File.ReadAllLines(filePath);
           

            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { '*' });
                string id = fields[0];
               

                if (id.Equals(healthRecord.IdHealthRecord))
                {

                    lines[i] = id + "*" + healthRecord.EmailPatient + "*" + healthRecord.PatientHeight.ToString() + "*" + healthRecord.PatientWeight.ToString() + "*" + healthRecord.PreviousIllnesses + "*" + healthRecord.Allergen
                        + "*" + healthRecord.BloodType;
                    Console.WriteLine("Uspesno ste izmenili zdravstveni karton");


                }
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }

    }
}
