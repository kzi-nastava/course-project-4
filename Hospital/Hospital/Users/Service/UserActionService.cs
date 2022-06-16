using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Repository;
using Hospital.Users.Model;
using Hospital.Users.View;
using Autofac;

namespace Hospital.Users.Service
{
    public class UserActionService : IUserActionService
    {
        private IUserActionRepository _actionRepository;
        private List<UserAction> _actions;

        public List<UserAction> Actions { get { return _actions; } }
        public IUserActionRepository ActionRepository { get { return _actionRepository; } }

        public UserActionService()
        {
            this._actionRepository = Globals.container.Resolve<IUserActionRepository>();
            this._actions = _actionRepository.Load();
        }

        public List<UserAction> LoadMyActions(string patientEmail)
        {
            List<UserAction> myActions = new List<UserAction>();

            foreach (UserAction action in this._actions)
            {
                if (patientEmail.Equals(action.PatientEmail) && (DateTime.Now - action.ActionDate).TotalDays <= 30)
                    myActions.Add(action);
            }
            return myActions;
        }

        public void AntiTrolMechanism(Patient patient)
        {
            int changed = 0;
            int deleted = 0;
            int created = 0;

            List<UserAction> myCurrentActions = this.LoadMyActions(patient.Email);

            foreach (UserAction action in myCurrentActions)
            {
                if (action.ActionState == UserAction.State.Created)
                    created += 1;
                else if (action.ActionState == UserAction.State.Modified)
                    changed += 1;
                else if (action.ActionState == UserAction.State.Deleted)
                    deleted += 1;
            }

            if (changed > 4)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste izmenili termin.\nPristup aplikaciji Vam je sada blokiran!");
            else if (deleted > 4)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste obrisali termin.\nPristup aplikaciji Vam je sada blokiran!");
            else if (created > 8)
                Console.WriteLine("\nU proteklih 30 dana previse puta ste kreirali termin.\nPristup aplikaciji Vam je sada blokiran!");
            else
                return;

            this._actionRepository.Save(this._actions);
            patient.LogOut(); //log out from account
        }
    }
}