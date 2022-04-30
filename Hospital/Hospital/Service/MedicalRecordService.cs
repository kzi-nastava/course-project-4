using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;
namespace Hospital.Service
{
    class MedicalRecordService
    {
        private MedicalRecordRepository medicalRecordRepository;
        private List<MedicalRecord> medicalRecords;

        public MedicalRecordService()
        {
            this.medicalRecordRepository = new MedicalRecordRepository();
            this.medicalRecords = medicalRecordRepository.Load();

        }

        public List<MedicalRecord> GetMedicalRecords { get { return medicalRecords; } }
    }
}
