using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Appointments.Model;
using Hospital.Users.Model;

namespace Hospital.Appointments.Service
{
    public interface IHealthRecordService: IService<HealthRecord>
    {
        void DisplayOfPatientsHealthRecord(string patientEmail);
        void Update(HealthRecord healthRecordForUpdate);
        void CreateHealthRecord(User patient);
    }
}
