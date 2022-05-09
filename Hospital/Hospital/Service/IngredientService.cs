using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class IngredientService
    {
        private IngredientRepository _ingredientRepository;
        private List<Ingredient> _ingredients;


        public IngredientService()
        {
            this._ingredientRepository = new IngredientRepository();
            this._ingredients = _ingredientRepository.Load();
        }

        public IngredientRepository IngredientRepository{get{ return _ingredientRepository; }set { _ingredientRepository = value; } }
        public List<Ingredient> Ingredients { get { return _ingredients; } set { _ingredients = value; } }

        public Ingredient Get(string id)
        {
            foreach (Ingredient ingredient in _ingredients)
            {
                if (ingredient.Id.Equals(id))
                    return ingredient; 
            }
            return null;
        }

    }
}
