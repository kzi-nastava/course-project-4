using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

using Hospital.Drugs.Service;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Repository
{
    public class DrugProposalRepository: IDrugProposalRepository
    {
        private IngredientService _ingredientService; 
        private List<DrugProposal> _drugProposals;

        public DrugProposalRepository()
        {
            _ingredientService = new IngredientService();
            _drugProposals = Load();
        }

        public List<DrugProposal> DrugProposals { get { return _drugProposals; } }

        public List<DrugProposal> GetDrugProposalsByStatus(DrugProposal.Status status)
        {
            List<DrugProposal> proposals = new List<DrugProposal>();
            foreach (DrugProposal drugProposal in this._drugProposals)
            {
                if (drugProposal.ProposalStatus == status)
                {
                    proposals.Add(drugProposal);
                }
            }
            return proposals;
        }

        public void UpdateDrugProposal(DrugProposal drugProposalForChange)
        {
            foreach (DrugProposal drugProposal in this._drugProposals)
            {
                if (drugProposal.Id.Equals(drugProposalForChange.Id))
                {
                    drugProposal.ProposalStatus = drugProposalForChange.ProposalStatus;
                    drugProposal.Comment = drugProposalForChange.Comment;
                }
            }
            Save(this._drugProposals);
        }

        public DrugProposal Get(string id)
        {
            foreach (DrugProposal drugProposal in _drugProposals)
            {
                if (drugProposal.Id.Equals(id))
                    return drugProposal;
            }
            return null;
        }

        public void CreateDrugProposal(string id, string drugName, List<Ingredient> ingredients)
        {
            DrugProposal drugProposal = new DrugProposal(id, drugName, ingredients, DrugProposal.Status.Waiting, "");
            _drugProposals.Add(drugProposal);
            Save(_drugProposals);
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

        public void Save(List<DrugProposal> drugProposals)
        {
            string filePath = @"..\..\Data\drugProposals.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (DrugProposal drugProposal in drugProposals)
            {
                line = drugProposal.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
