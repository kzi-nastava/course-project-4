using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;
using Hospital.Service;

namespace Hospital.Repository
{
    class DrugProposalRepository
    {
        private IngredientService _ingredientService;

        public DrugProposalRepository()
        {
            _ingredientService = new IngredientService();
        }


        public List<DrugProposal> Load()
        {
            List<DrugProposal> allDrugProposals = new List<DrugProposal>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\drugProposals.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("*");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string idProposal = fields[0];
                    string drugName = fields[1];
                    List<string> ingredientIds = fields[2].Split(';').ToList();
                    List<Ingredient> ingredients = new List<Ingredient>();
                    foreach (string id in ingredientIds)
                    {
                        ingredients.Add(_ingredientService.Get(id));
                    }
                    DrugProposal.Status status = (DrugProposal.Status)int.Parse(fields[3]);
                    string comment = fields[4];
                    DrugProposal newDrugProposal = new DrugProposal(idProposal, drugName, ingredients, status, comment);
                    allDrugProposals.Add(newDrugProposal);
                }
            }
            return allDrugProposals;
        }
    }
}
