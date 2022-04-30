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
        private HealthRecordRepository _healthRecordRepository;
        private UserRepository _userRepository;
        private List<HealthRecord> _healthRecords;
        private List<User> _users;


        public HealthRecordService()
        {
            _healthRecordRepository = new HealthRecordRepository();
            _userRepository = new UserRepository();
            _healthRecords = _healthRecordRepository.Load();
            _users = _userRepository.Load();

        }

        public List<HealthRecord> GetHealthRecords { get{ return _healthRecords; } }

        public void UpdateHealthRecord(HealthRecord healthRecord)
        {
            
            string filePath = @"..\..\Data\healthRecords.csv";
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ',' });
                string id = fields[0];

                if (id.Equals(healthRecord.IdHealthRecord))
                {

                    lines[i] = id + "," + healthRecord.EmailPatient + "," + healthRecord.PatientHeight.ToString() + "," + healthRecord.PatientWeight.ToString() + "," + healthRecord.PreviousIllnesses + "," + healthRecord.Allergen
                        + "," + healthRecord.BloodType;
                    Console.WriteLine("Uspesno ste izmenili zdravstveni karton");


                }
            }
            // saving changes
            File.WriteAllLines(filePath, lines);
        }
        
    }
}
