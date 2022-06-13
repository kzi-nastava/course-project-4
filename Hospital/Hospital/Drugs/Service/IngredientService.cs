using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Drugs.Repository;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Service
{
    public class IngredientService
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

        public bool IsIngredientNameValid(string name)
        {
            foreach (Ingredient ingredient in _ingredients)
            {
                if (ingredient.IngredientName.ToLower().Equals(name.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IdExists(string id)
        {
            return Get(id) != null;
        }

        public bool CreateIngredient(string id, string name)
        {
            if (IdExists(id))
                return false;
            Ingredient ingredient = new Ingredient(id, name);
            _ingredients.Add(ingredient);
            _ingredientRepository.Save(_ingredients);
            return true;
        }

        public bool UpdateIngredient(string id, string name)
        {
            if (!IdExists(id))
                return false;
            DeleteIngredient(id);
            CreateIngredient(id, name);
            return true;
        }

        public bool DeleteIngredient(string id)
        {
            if (!IdExists(id))
                return false;
            _ingredients.Remove(Get(id));
            _ingredientRepository.Save(_ingredients);
            return true;
        }
    }
}
