using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Ingredient
    {
        private string _id;
        private string _ingredientName;

        public Ingredient(string id, string ingredientName)
        {
            this._id = id;
            this._ingredientName = ingredientName;
        }
        public string Id { get { return _id; } set { _id = value; } }
        public string IngredientName { get { return _ingredientName; } set { _ingredientName = value; } }

        public override string ToString()
        {
            return this._id + "," + this._ingredientName;
        }
    }
}
