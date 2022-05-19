using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    class Drug
    {
       
        private string _idDrug;
        private string _drugName;
        private List<Ingredient> _ingredients;
      


        public string IdDrug { get { return _idDrug; } set { _idDrug = value; } }

        public string DrugName { get { return _drugName; } set { _drugName = value; } }

        public List<Ingredient> Ingredients { get { return _ingredients; } set { _ingredients = value; } }

      
        public Drug(string idDrug, string drugName, List<Ingredient> ingredients)
        {
            this._idDrug = idDrug;
            this._drugName = drugName;
            this._ingredients = ingredients;
        }

        public override string ToString()
        {
            string ingredients = "";
            foreach (Ingredient ingredient in this._ingredients)
            {
                ingredients += ingredient.Id + ";";
            }
            return this._idDrug + "," + this._drugName + "," + ingredients.Remove(ingredients.Length - 1);
        }


    }

}
