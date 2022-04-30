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
        private UserActionRepository _actionRepository;
        private List<UserAction> _actions;

        public UserActionRepository ActionRepository { get { return _actionRepository; } }
        public List<UserAction> Actions { get { return _actions; } }

        public UserActionService()
        {
            _actionRepository = new UserActionRepository();
            _actions = _actionRepository.Load();
        }
    }
}
