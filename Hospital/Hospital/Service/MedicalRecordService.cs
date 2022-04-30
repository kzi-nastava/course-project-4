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
        private MedicalRecordRepository _medicalRecordRepository;
        private List<MedicalRecord> _medicalRecords;

        public MedicalRecordService()
        {
            this._medicalRecordRepository = new MedicalRecordRepository();
            this._medicalRecords = _medicalRecordRepository.Load();

        }

        public List<MedicalRecord> GetMedicalRecords { get { return _medicalRecords; } }
    }
}
