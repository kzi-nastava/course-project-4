using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Drugs.Service;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Repository
{
    public class DrugRepository
    {
        private IngredientService _ingredientService;
        private List<Drug> _drugs;

        public DrugRepository()
        {
            _ingredientService = new IngredientService();
            this._drugs = Load();
        }

        public List<Drug> Drugs { get { return _drugs; } }

        public int GetNewDrugId()
        {
            return _drugs.Count + 1;
        }

        public void AddDrug(Drug drug)
        {
            this._drugs.Add(drug);
            Save(this._drugs);
        }

        public List<Drug> Load()
        {
            List<Drug> allDrugs = new List<Drug>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\drugs.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string idDrug = fields[0];
                    string idName = fields[1];
                    List<string> ingredientIds = fields[2].Split(';').ToList();
                    List<Ingredient> ingredients = new List<Ingredient>();
                    foreach (string id in ingredientIds)
                    {
                        ingredients.Add(_ingredientService.Get(id));
                    }
                    
                    Drug newDrug = new Drug(idDrug, idName, ingredients);
                    allDrugs.Add(newDrug);
                }
            }
            return allDrugs;
        }

        public void Save(List<Drug> drugs)
        {
            string filePath = @"..\..\Data\drugs.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (Drug drug in drugs)
            {
                line = drug.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}

