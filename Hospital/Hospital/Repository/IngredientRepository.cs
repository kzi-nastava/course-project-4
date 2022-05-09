using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    class IngredientRepository
    {
        public List<Ingredient> Load()
        {
            List<Ingredient> allIngredients = new List<Ingredient>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\ingredients.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    string ingredientName = fields[1];

                    Ingredient newIngredient = new Ingredient(id, ingredientName);
                    allIngredients.Add(newIngredient);
                }
            }
            return allIngredients;
        }
    }
}
