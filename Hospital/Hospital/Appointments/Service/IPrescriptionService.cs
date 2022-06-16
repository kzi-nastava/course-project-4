using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Appointments.Model;
using Hospital.Drugs.Model;

namespace Hospital.Appointments.Service
{
    public interface IPrescriptionService: IService<Prescription>
    {
        void Add(Prescription prescription);
        bool IsDrugValid(string drugCheck);
        bool IsTimeOfConsumingValid(string selectedTimeOfConsuming);
        string GetId(string drugName);
        bool ContainsIngredient(Drug drug, HealthRecord healthRecord);
        bool CheckAllergicToDrug(HealthRecord healthRecord, string drugCheck);
    }
}
