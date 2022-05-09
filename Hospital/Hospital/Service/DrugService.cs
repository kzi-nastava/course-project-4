using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;
namespace Hospital.Service
{
    class DrugService
    {
        private DrugRepository _drugRepository;
        private List<Drug> _drugs;


        public DrugService()
        {
            this._drugRepository = new DrugRepository();
            this._drugs = _drugRepository.Load();
        }

        public DrugRepository DrugRepository { get { return _drugRepository; } set { _drugRepository = value; } }

        public List<Drug> Drugs { get { return _drugs; } set { _drugs = value; } }
    }
}
