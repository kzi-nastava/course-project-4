using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class UserActionService
    {
        private UserActionRepository actionRepository;
        private List<UserAction> actions;

        public UserActionRepository ActionRepository { get { return actionRepository; } }
        public List<UserAction> Actions { get { return actions; } }

        public UserActionService()
        {
            actionRepository = new UserActionRepository();
            actions = actionRepository.Load();
        }
    }
}
