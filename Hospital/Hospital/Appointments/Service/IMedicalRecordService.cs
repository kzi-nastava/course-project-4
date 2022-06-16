using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Appointments.Model;

namespace Hospital.Appointments.Service
{
    public interface IMedicalRecordService
    {
        void Add(MedicalRecord medicalRecord);

        List<MedicalRecord> MedicalRecords { get; }
    }
}
