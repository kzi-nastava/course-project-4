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
    class MedicalRecordService
    {
        private MedicalRecordRepository _medicalRecordRepository;
        private List<MedicalRecord> _medicalRecords;

        public MedicalRecordService()
        {
            this._medicalRecordRepository = new MedicalRecordRepository();
            this._medicalRecords = _medicalRecordRepository.Load();

        }

        public List<MedicalRecord> MedicalRecords { get { return _medicalRecords; } }

        public void UpdateFile()
        {
            string filePath = @"..\..\Data\medicalRecords.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (MedicalRecord medicalRecord in _medicalRecords)
            {
                line = medicalRecord.IdAppointment + ";" + medicalRecord.Anamnesis + ";" + medicalRecord.ReferralToDoctor;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
