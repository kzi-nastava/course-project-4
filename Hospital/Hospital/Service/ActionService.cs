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
        private ActionRepository _actionRepository;
        private List<UserAction> _actions;

        public ActionRepository ActionRepository { get { return _actionRepository; } }
        public List<UserAction> Actions { get { return _actions; } }

        public ActionService()
        {
            _actionRepository = new ActionRepository();
            _actions = _actionRepository.Load();
        }
    }
}
