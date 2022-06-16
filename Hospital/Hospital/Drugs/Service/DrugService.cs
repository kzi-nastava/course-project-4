using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Hospital.Drugs.Repository;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Service
{
    public class DrugService: IDrugService
    {
        private DrugRepository _drugRepository;
        
        public DrugService()
        {
            this._drugRepository = new DrugRepository();
        }

        public List<Drug> Drugs { get { return _drugRepository.Drugs; } }

        public int GetNewDrugId()
        {
            return _drugRepository.GetNewDrugId();
        }

        public void AddDrug(Drug drug)
        {
            _drugRepository.AddDrug(drug);
        }
    }
}
