using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hospital;
using Autofac;
using Hospital.Drugs.Repository;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Service
{
    public class DrugProposalService : IDrugProposalService
    {
        private IDrugProposalRepository _drugProposalRepository;

        public DrugProposalService()
        {
            this._drugProposalRepository = Globals.container.Resolve<IDrugProposalRepository>();
        }

        public List<DrugProposal> DrugProposals { get { return _drugProposalRepository.DrugProposals; } }

        public List<DrugProposal> GetDrugProposalsByStatus(DrugProposal.Status status)
        {
            return _drugProposalRepository.GetDrugProposalsByStatus(status);
        }

        public List<DrugProposal> WaitingStatusDrugProposals()
        {
            return GetDrugProposalsByStatus(DrugProposal.Status.Waiting);
        }

        public List<DrugProposal> GetRejectedDrugProposals()
        {
            return GetDrugProposalsByStatus(DrugProposal.Status.Rejected);
        }

        public void UpdateDrugProposal(DrugProposal drugProposalForChange)
        {
            _drugProposalRepository.UpdateDrugProposal(drugProposalForChange);
        }

        public DrugProposal Get(string id)
        {
            return _drugProposalRepository.Get(id);
        }

        public bool IdExists(string id)
        {
            return Get(id) != null;
        }

        public bool CreateDrugProposal(string id, string drugName, List<Ingredient> ingredients)
        {
            if (IdExists(id))
                return false;
            _drugProposalRepository.CreateDrugProposal(id, drugName, ingredients);
            return true;
        }

        public bool ReviewDrugProposal(string id, string drugName, List<Ingredient> ingredients)
        {
            if (!IdExists(id) || Get(id).ProposalStatus != DrugProposal.Status.Rejected)
                return false;
            foreach (DrugProposal proposal in DrugProposals)
            {
                if (proposal.Id.Equals(id))
                {
                    proposal.DrugName = drugName;
                    proposal.Ingredients = ingredients;
                    proposal.ProposalStatus = DrugProposal.Status.Waiting;
                }
            }
            _drugProposalRepository.Save(DrugProposals);
            return true;
        }
    }
}
