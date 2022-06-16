using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Drugs.Model;
namespace Hospital.Drugs.Service
{
     public interface IIngredientService
    {
        List<Ingredient> Ingredients { get; }
        bool DeleteIngredient(string id);
        bool UpdateIngredient(string id, string name);
        bool CreateIngredient(string id, string name);
        bool IdExists(string id);
        bool IsIngredientNameValid(string name);
        Ingredient Get(string id);
    }
}
