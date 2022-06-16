using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Drugs.Model;

namespace Hospital.Drugs.Repository
{
    public class IngredientRepository: IIngredientRepository
    {
        private static string s_filePath = @"..\..\Data\ingredients.csv";
        
        private List<Ingredient> _ingredients;

        public List<Ingredient> Ingredients { get { return _ingredients; } set { _ingredients = value; } }

        public IngredientRepository()
        {
            _ingredients = Load();
        }

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

        public void CreateIngredient(string id, string name)
        {
            Ingredient ingredient = new Ingredient(id, name);
            _ingredients.Add(ingredient);
            Save(_ingredients);
        }

        public void UpdateIngredient(string id, string name)
        {
            DeleteIngredient(id);
            CreateIngredient(id, name);
        }

        public void DeleteIngredient(string id)
        {
            _ingredients.Remove(Get(id));
            Save(_ingredients);
        }

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
