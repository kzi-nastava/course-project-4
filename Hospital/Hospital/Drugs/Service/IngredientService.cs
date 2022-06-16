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
        
        public IngredientService()
        {
            this._ingredientRepository = new IngredientRepository();
        }

        public List<Ingredient> Ingredients { get { return _ingredientRepository.Ingredients; } }
        
        public Ingredient Get(string id)
        {
            return _ingredientRepository.Get(id);
        }

        public bool IsIngredientNameValid(string name)
        {
            return _ingredientRepository.IsIngredientNameValid(name);
        }
        public bool IdExists(string id)
        {
            return Get(id) != null;
        }

        public bool CreateIngredient(string id, string name)
        {
            if (IdExists(id))
                return false;
            _ingredientRepository.CreateIngredient(id, name);
            return true;
        }

        public bool UpdateIngredient(string id, string name)
        {
            if (!IdExists(id))
                return false;
            _ingredientRepository.UpdateIngredient(id, name);
            return true;
        }

        public bool DeleteIngredient(string id)
        {
            if (!IdExists(id))
                return false;
            _ingredientRepository.DeleteIngredient(id);
            return true;
        }
    }
}
