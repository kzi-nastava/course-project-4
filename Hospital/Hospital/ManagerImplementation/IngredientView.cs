using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.ManagerImplementation
{
    public class IngredientView
    {
        private IngredientService _ingredientService;

        public IngredientView(IngredientService ingredientService)
        {
            this._ingredientService = ingredientService;
        }

        public void ManageIngredients()
        {
            Console.WriteLine("1. Kreiraj sastojak");
            Console.WriteLine("2. Pregledaj sastojke");
            Console.WriteLine("3. Izmeni sastojak");
            Console.WriteLine("4. Obrisi sastojak");
            Console.Write(">> ");
            string choice = Console.ReadLine();

            if (choice.Equals("1"))
                CreateIngredient();
            else if (choice.Equals("2"))
                ListIngredients();
            else if (choice.Equals("3"))
                UpdateIngredient();
            else if (choice.Equals("4"))
                DeleteIngredient();
        }

        private string EnterIngredientId(bool existing)
        {
            Console.Write("Unesite id sastojka: ");
            string id = Console.ReadLine();
            while (_ingredientService.IdExists(id) != existing)
            {
                if (existing)
                    Console.Write("Id ne postoji. Unesite postojeci id sastojka: ");
                else
                    Console.Write("Id je zauzet. Odaberite drugi id: ");
                id = Console.ReadLine();
            }
            return id;
        }

        private string EnterNewIngredientId()
        {
            return EnterIngredientId(false);
        }

        private string EnterExistingIngredientId()
        {
            return EnterIngredientId(true);
        }

        private string EnterIngredientName()
        {
            Console.Write("Unesite naziv sastojka: ");
            string name = Console.ReadLine();
            while (name.Length == 0)
            {
                Console.Write("Naziv ne moze biti prazan! Unesite naziv sastojka: ");
                name = Console.ReadLine();
            }
            return name;
        }

        public void CreateIngredient()
        {
            Console.WriteLine("Unesite podatke o sastojku");
            Console.WriteLine("------------------");

            string id = EnterNewIngredientId();
            string name = EnterIngredientName();

            _ingredientService.CreateIngredient(id, name);
        }

        public void ListIngredients()
        {
            List<Ingredient> allIngredients = _ingredientService.Ingredients;
            foreach (Ingredient ingredient in allIngredients)
            {
                Console.WriteLine("Id sastojka: " + ingredient.Id + ", naziv sastojka: " + ingredient.IngredientName);
            }
        }

        public void UpdateIngredient()
        {
            Console.WriteLine("Unesite podatke o sastojku");
            Console.WriteLine("------------------");

            string id = EnterExistingIngredientId();
            string name = EnterIngredientName();

            _ingredientService.UpdateIngredient(id, name);
        }

        public void DeleteIngredient()
        {
            string id = EnterExistingIngredientId();
            _ingredientService.DeleteIngredient(id);
        }
    }
}
