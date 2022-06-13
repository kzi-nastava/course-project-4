using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Drugs.Service;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.View
{
    public class DrugVerification
    {
        IngredientService ingredientService;
        DrugProposalService drugProposalService;
        DrugService drugService;

        public DrugVerification()
        {
            ingredientService = new IngredientService();
            drugProposalService = new DrugProposalService();
            drugService = new DrugService();
        }

        private void DrugVerificationManagement()
        {
            string selectedDrugProposal;
            do
            {
                Console.WriteLine("Unesite redni broj verifikacije leka: ");
                selectedDrugProposal = Console.ReadLine();

            } while (Int32.Parse(selectedDrugProposal) > drugProposalService.WaitingStatusDrugProposals().Count);
            DrugProposal drugProposalSelected = drugProposalService.WaitingStatusDrugProposals()[Int32.Parse(selectedDrugProposal) - 1];
            string selectionOfUpdates = this.EnterSelectionOfUpdates();
            this.VerifyDrugProposal(selectionOfUpdates, drugProposalSelected);

        }

        private string EnterSelectionOfUpdates()
        {
            string selectionOfUpdates;
            do
            {
                Console.WriteLine("Da li zelite da prihvatite lek? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                selectionOfUpdates = Console.ReadLine();

            } while (!selectionOfUpdates.Equals("1") && !selectionOfUpdates.Equals("2"));
            return selectionOfUpdates;
        }

        private void VerifyDrugProposal(string choice, DrugProposal drugProposalSelected)
        {
            if (choice.Equals("1"))
            {
                drugProposalSelected.ProposalStatus = DrugProposal.Status.Accepted;
                drugProposalService.UpdateDrugProposal(drugProposalSelected);
                this.AddDrug(drugProposalSelected);
                Console.WriteLine("Uspesno ste prihvatili lek!");
            }
            else
            {
                
                Console.WriteLine("Unesite komentar odbijanja leka: ");
                string comment = Console.ReadLine();
                drugProposalSelected.Comment = comment;
                drugProposalSelected.ProposalStatus = DrugProposal.Status.Rejected;
                drugProposalService.UpdateDrugProposal(drugProposalSelected);
                Console.WriteLine("Uspesno ste odbili zahtev!");
            }
        }

        private void AddDrug(DrugProposal drugProposal)
        {
            Drug newDrug = new Drug(drugService.GetNewDrugId().ToString(), drugProposal.DrugName, drugProposal.Ingredients);
            drugService.AddDrug(newDrug);
        }
        public void DisplayDrugsForVerification()
        {
            List<DrugProposal> drugProposals = drugProposalService.WaitingStatusDrugProposals();
            if(drugProposals.Count != 0)
            {
                int serialNumber = 1;
                foreach (DrugProposal drugProposal in drugProposals)
                {
                    Console.WriteLine("Redni broj: " + serialNumber.ToString());
                    Console.WriteLine("Naziv leka: " + drugProposal.DrugName);
                    Console.WriteLine("Sastojci: ");
                    foreach (Ingredient ingredient in drugProposal.Ingredients)
                    {
                        Console.WriteLine("- " + ingredient.IngredientName);

                    }
                    Console.WriteLine("-----------------------------");
                    serialNumber++;

                }
                this.DrugVerificationManagement();
            }
            else
            {
                Console.WriteLine("Nema lekova koji cekaju na verifikaciju!");
            }
        }
    }
}
