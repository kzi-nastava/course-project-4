using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Hospital;
using Hospital.Appointments.Repository;
using Hospital.Appointments.Model;
using Hospital.Drugs.Repository;
using Hospital.Drugs.Model;
using Hospital.Drugs.Service;

namespace Hospital.Appointments.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private IPrescriptionRepository _prescriptionRepository;
        private List<Prescription> _prescriptions;
        private IDrugRepository _drugRepository;
        private List<Drug> _drugs;
        private IIngredientService _ingredientService;

        public PrescriptionService()
        {
            this._prescriptionRepository = Globals.container.Resolve<IPrescriptionRepository>();
            this._prescriptions = _prescriptionRepository.Load();
            IIngredientRepository ingredientRepository = Globals.container.Resolve<IIngredientRepository>();
            this._ingredientService = Globals.container.Resolve<IIngredientService>();
            this._drugRepository = Globals.container.Resolve<IDrugRepository>();
            this._drugs = _drugRepository.Load();
        }

        public IPrescriptionRepository PrescriptionRepository { get { return _prescriptionRepository; } set { _prescriptionRepository = value; } }
        public List<Prescription> Prescriptions { get { return _prescriptions; } set { _prescriptions = value; } }

        public bool CheckAllergicToDrug(HealthRecord healthRecord, string drugCheck)
        {

            foreach (Drug drug in this._drugs)
            {
                if ((drug.DrugName.ToLower()).Equals(drugCheck.ToLower()))
                {
                    if (ContainsIngredient(drug, healthRecord))
                    {
                        Console.WriteLine("Pacijent je alergičan na neki od sastojaka iz " + drug.DrugName + " leka.");
                        return false;
                    }
                }
            }
            return true;

        }

        private bool ContainsIngredient(Drug drug, HealthRecord healthRecord)
        {
            foreach (Ingredient ingredient in drug.Ingredients)
            {
                if ((ingredient.IngredientName.ToLower()).Equals(healthRecord.Allergen.ToLower()))
                {
                    return true;
                }
            }
            return false;

        }


        public string GetId(string drugName)
        {
            foreach (Drug drug in this._drugs)
            {
                if (drug.DrugName.ToLower().Equals(drugName.ToLower()))
                {
                    return drug.IdDrug;
                }

            }
            return "";

        }

        public bool IsTimeOfConsumingValid(string selectedTimeOfConsuming)
        {
            if (selectedTimeOfConsuming.Equals("1") || selectedTimeOfConsuming.Equals("2") || selectedTimeOfConsuming.Equals("3") || selectedTimeOfConsuming.Equals("4"))
            {
                return true;
            }
            return false;
        }

        public bool IsDrugValid(string drugCheck)
        {
            foreach (Drug drug in this._drugs)
            {
                if (drug.DrugName.ToLower().Equals(drugCheck.ToLower()))
                {
                    return true;
                }
            }
            Console.WriteLine("Lek sa ovim nazivom ne postoji!");
            return false;
        }
        public void Add(Prescription prescription)
        {
            this._prescriptions.Add(prescription);
            this._prescriptionRepository.Save(this._prescriptions);
        }

    }
}
