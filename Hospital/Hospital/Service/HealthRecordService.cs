using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;
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

        public List<HealthRecord> GetHealthRecords { get{ return healthRecords; } }
        
    }
}
