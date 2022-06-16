using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Drugs.Model;

namespace Hospital.Drugs.Service
{
    public interface IDrugProposalService: IService<DrugProposal>
    {
        bool ReviewDrugProposal(string id, string drugName, List<Ingredient> ingredients);
        bool CreateDrugProposal(string id, string drugName, List<Ingredient> ingredients);
        bool IdExists(string id);
        DrugProposal Get(string id);
        void UpdateDrugProposal(DrugProposal drugProposalForChange);
        List<DrugProposal> GetRejectedDrugProposals();

        List<DrugProposal> WaitingStatusDrugProposals();
        List<DrugProposal> GetDrugProposalsByStatus(DrugProposal.Status status);

    }
}
