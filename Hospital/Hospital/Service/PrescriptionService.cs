using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    class PrescriptionService
    {
        private PrescriptionRepository _prescriptionRepository;
        private List<Prescription> _prescriptions;
        private DrugRepository _drugRepository;
        private List<Drug> _drugs;
        private IngredientService _ingredientService;


        public PrescriptionService()
        {
            this._prescriptionRepository = new PrescriptionRepository();
            this._prescriptions = _prescriptionRepository.Load();
            this._drugRepository = new DrugRepository();
            this._drugs = _drugRepository.Load();
            this._ingredientService = new IngredientService();
        }

        public PrescriptionRepository PrescriptionRepository { get { return _prescriptionRepository; } set { _prescriptionRepository = value; } }
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
                if ((ingredient.IngredientName.ToLower()).Equals(healthRecord.Allergen.ToLower())){
                    return true;
                }
            }
            return false;

        }

        
        public string GetIdDrug(string drugName)
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
            if(selectedTimeOfConsuming.Equals("1") || selectedTimeOfConsuming.Equals("2") || selectedTimeOfConsuming.Equals("3") || selectedTimeOfConsuming.Equals("4"))
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
        public void AddPrescription(Prescription prescription)
        {
            this._prescriptions.Add(prescription);
            this._prescriptionRepository.Save(this._prescriptions);
        }
        
    }
}
