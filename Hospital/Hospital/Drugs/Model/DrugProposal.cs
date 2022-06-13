using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Drugs.Model
{
    public class DrugProposal
    {
        public enum Status
        {
            Waiting = 1,
            Accepted = 2,
            Rejected = 3

        }
        private string _id;
        private string _drugName;
        private List<Ingredient> _ingredients;
        private Status _status;
        private string _comment;

        public DrugProposal(string id,string drugName, List<Ingredient> ingredients, Status status, string comment)
        {
            this._id = id;
            this._drugName = drugName;
            this._ingredients = ingredients;
            this._status = status;
            this._comment = comment;
        }

        public string Id { get { return _id; } set { _id = value; } }
        public string DrugName { get { return _drugName; } set { _drugName = value; } }

        public List<Ingredient> Ingredients { get { return _ingredients; } set { _ingredients = value; } }

        public Status ProposalStatus { get { return _status; } set { _status = value; } }

        public string Comment { get { return _comment; } set { _comment = value; } }

        public override string ToString()
        {
            string ingredients = "";
            foreach(Ingredient ingredient in this._ingredients)
            {
                ingredients += ingredient.Id + ";";
            }         
            return this._id + "*" + this._drugName + "*" + ingredients.Remove(ingredients.Length - 1) + "*" + (int)this.ProposalStatus + "*" + this._comment;
        }

    }
}
