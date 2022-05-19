using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class DrugProposalService
    {
        private DrugProposalRepository _drugProposalRepository;
        private List<DrugProposal> _drugProposals;


        public DrugProposalService()
        {
            this._drugProposalRepository = new DrugProposalRepository();
            this._drugProposals = _drugProposalRepository.Load();
        }

        public List<DrugProposal> DrugProposals { get { return _drugProposals; }set { _drugProposals = value; } }
    }
}
