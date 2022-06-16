using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autofac;
using Hospital;
using Hospital.Appointments.Repository;
using Hospital.Users.Repository;
using Hospital.Appointments.Model;
using Hospital.Users.Model;

namespace Hospital.Appointments.Service
{
    public class HealthRecordService : IHealthRecordService
    {
        private IHealthRecordRepository _healthRecordRepository;
        private IUserRepository _userRepository;
        private List<HealthRecord> _healthRecords;
        private List<User> _users;

        public HealthRecordService()
        {
            _healthRecordRepository = Globals.container.Resolve<IHealthRecordRepository>();
            _userRepository = Globals.container.Resolve<IUserRepository>();
            _healthRecords = _healthRecordRepository.Load();
            _users = _userRepository.Load();

        }

        public List<HealthRecord> HealthRecords { get { return _healthRecords; } }

        public IHealthRecordRepository HealthRecordRepository { get { return _healthRecordRepository; } }

        public void CreateHealthRecord(User patient)
        {
            List<HealthRecord> healthRecords = this.HealthRecords;
            int id = healthRecords.Count + 1;
            HealthRecord newHealthRecord = new HealthRecord(id.ToString(), patient.Email, 0, 0, "0", "0", "0");
            this.HealthRecords.Add(newHealthRecord);
            _healthRecordRepository.Save(this._healthRecords);
        }

        public void Update(HealthRecord healthRecordForUpdate)
        {
            foreach (HealthRecord healthRecord in this._healthRecords)
            {
                if (healthRecord.IdHealthRecord.Equals(healthRecordForUpdate.IdHealthRecord))
                {
                    healthRecord.EmailPatient = healthRecordForUpdate.EmailPatient;
                    healthRecord.PatientHeight = healthRecordForUpdate.PatientHeight;
                    healthRecord.PatientWeight = healthRecordForUpdate.PatientWeight;
                    healthRecord.PreviousIllnesses = healthRecordForUpdate.PreviousIllnesses;
                    healthRecord.Allergen = healthRecordForUpdate.Allergen;
                    healthRecord.BloodType = healthRecord.BloodType;
                }
            }
            this._healthRecordRepository.Save(this._healthRecords);
            Console.WriteLine("Uspesno ste izmenili zdravstveni karton!");
        }

        public void DisplayOfPatientsHealthRecord(string patientEmail)
        {
            foreach (HealthRecord healthRecord in this._healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(patientEmail))
                {
                    Console.WriteLine(healthRecord.ToString());
                }
            }
        }
    }
}
