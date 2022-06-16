using Hospital.Drugs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Drugs.Repository
{
    public interface IIngredientRepository: IRepository<Ingredient>
    {
        List<Ingredient> Ingredients { get; set; }

        Ingredient Get(string id);

        bool IsIngredientNameValid(string name);

        void CreateIngredient(string id, string name);

        void UpdateIngredient(string id, string name);

        void DeleteIngredient(string id);
    }
}
