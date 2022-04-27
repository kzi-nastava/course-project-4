using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class ActionService
    {
        private ActionRepository actionRepository;
        private List<UserAction> actions;

        public ActionRepository ActionRepository { get { return actionRepository; } }
        public List<UserAction> Actions { get { return actions; } }

        public ActionService()
        {
            actionRepository = new ActionRepository();
            actions = actionRepository.Load();
        }
    }
}
