using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    class IngredientRepository
    {
        private static string s_filePath = @"..\..\Data\ingredients.csv";

        public List<Ingredient> Load()
        {
            List<Ingredient> allIngredients = new List<Ingredient>();
            using (TextFieldParser parser = new TextFieldParser(s_filePath))
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

        public void Save(List<Ingredient> allIngredients)
        {
            string[] lines = new string[allIngredients.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                Ingredient ingredient = allIngredients[i];
                lines[i] = ingredient.ToString();
            }
            
            File.WriteAllLines(s_filePath, lines);
        }
    }
}
