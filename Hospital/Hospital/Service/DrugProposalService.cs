using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;
using System.IO;

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

        public void UpdateDrugProposalFile()
        {
            string filePath = @"..\..\Data\drugProposals.csv";

            List<string> lines = new List<String>();

            string line;
            foreach (DrugProposal drugProposal in this._drugProposals)
            {
                line = drugProposal.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }

        public List<DrugProposal> WaitingStatusDrugProposals()
        {
            List<DrugProposal> proposals = new List<DrugProposal>();
            foreach(DrugProposal drugProposal in this._drugProposals)
            {
                if(drugProposal.ProposalStatus == DrugProposal.Status.Waiting)
                {
                    proposals.Add(drugProposal);
                }
            }
            return proposals;
        }

        public void  UpdateDrugProposal(DrugProposal drugProposalForChange)
        {
            foreach(DrugProposal drugProposal in this._drugProposals)
            {
                if (drugProposal.Id.Equals(drugProposalForChange.Id))
                {
                    drugProposal.ProposalStatus = drugProposalForChange.ProposalStatus;
                    drugProposal.Comment = drugProposalForChange.Comment;
                }
            }
        }
    }
}
