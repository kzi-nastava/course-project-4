using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Drugs.View
{
    public interface IDrugView
    {
        void ProposeDrug();

        void ListRejectedDrugProposals();

        void ReviewRejectedDrugProposal();
    }
}
