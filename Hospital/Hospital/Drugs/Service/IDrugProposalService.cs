using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Service
{
    public interface IDrugProposalService : IService<DrugProposal>
    {
        List<DrugProposal> DrugProposals { get; }

        List<DrugProposal> GetDrugProposalsByStatus(DrugProposal.Status status);

        List<DrugProposal> WaitingStatusDrugProposals();

        List<DrugProposal> GetRejectedDrugProposals();

        void UpdateDrugProposal(DrugProposal drugProposalForChange);

        DrugProposal Get(string id);

        bool IdExists(string id);

        bool CreateDrugProposal(string id, string drugName, List<Ingredient> ingredients);

        bool ReviewDrugProposal(string id, string drugName, List<Ingredient> ingredients);
    }
}
