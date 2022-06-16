using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Drugs.View
{
    public interface IIngredientView
    {
        void ManageIngredients();

        void CreateIngredient();

        void ListIngredients();

        void UpdateIngredient();

        void DeleteIngredient();
    }
}
