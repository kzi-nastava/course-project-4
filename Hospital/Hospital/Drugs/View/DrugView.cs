using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Drugs.Service;
using Hospital.Drugs.Model;
using Hospital;
using Autofac;
namespace Hospital.Drugs.View
{
    public class DrugView
    {
        private IIngredientService _ingredientService;
        private IDrugProposalService _drugProposalService;

        public DrugView()
        {
            this._ingredientService = Globals.container.Resolve<IIngredientService>();
            this._drugProposalService = Globals.container.Resolve<IDrugProposalService>();
        }

        private string EnterId(bool existing)
        {
            Console.Write("Unesite id: ");
            string id = Console.ReadLine();
            while (_drugProposalService.IdExists(id) != existing)
            {
                if (existing)
                    Console.Write("Id ne postoji. Unesite postojeci id: ");
                else
                    Console.Write("Id je zauzet. Odaberite drugi id: ");
                id = Console.ReadLine();
            }
            return id;
        }

        private string EnterNewId()
        {
            return EnterId(false);
        }

        private string EnterExistingId()
        {
            return EnterId(true);
        }

        private string EnterDrugName()
        {
            Console.Write("Unesite naziv leka: ");
            string name = Console.ReadLine();
            while (name.Length == 0)
            {
                Console.Write("Naziv ne moze biti prazan! Unesite naziv leka: ");
                name = Console.ReadLine();
            }
            return name;
        }

        private Ingredient EnterIngredient(bool isFirst)
        {
            if (!isFirst)
            {
                Console.Write("Da li zelite da unosite jos sastojaka (DA/NE)? ");
                string choice = Console.ReadLine();
                if (!choice.ToLower().Equals("da"))
                    return null;
            }

            Console.Write("Unesite id sastojka: ");
            string id = Console.ReadLine();
            while (!_ingredientService.IdExists(id))
            {
                Console.Write("Id sastojka ne postoji. Unesite drugi id: ");
                id = Console.ReadLine();
            }
            return _ingredientService.Get(id);
        }

        private List<Ingredient> EnterIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            Ingredient ingredient = null;
            do
            {
                ingredient = EnterIngredient(ingredients.Count == 0);
                if (ingredient != null)
                    ingredients.Add(ingredient);
            } while (ingredient != null);
            return ingredients;
        }

        public void ProposeDrug()
        {
            Console.WriteLine("Unesite podatke o predlozenom leku");
            Console.WriteLine("------------------");

            string id = EnterNewId();
            string drugName = EnterDrugName();
            List<Ingredient> ingredients = EnterIngredients();

            _drugProposalService.CreateDrugProposal(id, drugName, ingredients);
        }

        private void PrintDrugProposal(DrugProposal drugProposal)
        {
            Console.WriteLine($"\nOdbijen predlog (id: {drugProposal.Id}, naziv leka: {drugProposal.DrugName})");
            Console.WriteLine("Sastojci:");
            foreach (Ingredient ingredient in drugProposal.Ingredients)
            {
                Console.WriteLine($"Id: {ingredient.Id}, naziv sastojka: {ingredient.IngredientName}");
            }
            Console.WriteLine("Komentar: " + drugProposal.Comment);
        }

        public void ListRejectedDrugProposals()
        {
            List<DrugProposal> proposals = _drugProposalService.GetRejectedDrugProposals();
            foreach (DrugProposal proposal in proposals)
                PrintDrugProposal(proposal);
        }

        public void ReviewRejectedDrugProposal()
        {
            Console.WriteLine("Unesite izmenjene podatke o odbijenom leku");
            Console.WriteLine("------------------");

            string id = EnterExistingId();
            string drugName = EnterDrugName();
            List<Ingredient> ingredients = EnterIngredients();

            _drugProposalService.ReviewDrugProposal(id, drugName, ingredients);
        }
    }
}
