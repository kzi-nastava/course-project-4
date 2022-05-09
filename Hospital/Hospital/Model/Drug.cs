using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    class Drug
    {
        public enum StateDrug
        {
            Accepted = 1,
            Rejected = 2,
            InProcess = 3
        }
        private string _idDrug;
        private string _drugName;
        private List<Ingredient> _ingredients;
        private StateDrug _stateDrug;


        public string IdDrug { get { return _idDrug; } set { _idDrug = value; } }

        public string DrugName { get { return _drugName; } set { _drugName = value; } }

        public List<Ingredient> Ingredients { get { return _ingredients; } set { _ingredients = value; } }

        public StateDrug DrugState { get { return _stateDrug; } set { _stateDrug = value; } }
        public Drug(string idDrug, string drugName, List<Ingredient> ingredients, StateDrug stateDrug)
        {
            this._idDrug = idDrug;
            this._drugName = drugName;
            this._ingredients = ingredients;
            this._stateDrug = stateDrug;
        }

      
       
    }

}
