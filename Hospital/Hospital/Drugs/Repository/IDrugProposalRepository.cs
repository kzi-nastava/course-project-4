using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Repository
{
    public interface IDrugProposalRepository: IRepository<DrugProposal>
    {
        List<DrugProposal> DrugProposals { get; }

        List<DrugProposal> GetDrugProposalsByStatus(DrugProposal.Status status);
        
        void UpdateDrugProposal(DrugProposal drugProposalForChange);

        DrugProposal Get(string id);

        void CreateDrugProposal(string id, string drugName, List<Ingredient> ingredients);
    }
}
