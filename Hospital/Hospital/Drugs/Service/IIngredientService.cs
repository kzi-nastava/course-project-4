using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Drugs.Model;
namespace Hospital.Drugs.Service
{
    public interface IIngredientService: IService<Ingredient>
    {
        List<Ingredient> Ingredients { get; }

        Ingredient Get(string id);

        bool IsIngredientNameValid(string name);

        bool IdExists(string id);

        bool CreateIngredient(string id, string name);

        bool UpdateIngredient(string id, string name);

        bool DeleteIngredient(string id);
    }
}
