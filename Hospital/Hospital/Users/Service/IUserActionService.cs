using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;
using Hospital.Users.Repository;

namespace Hospital.Users.Service
{
    public interface IUserActionService: IService<UserAction>
    {
        UserActionRepository ActionRepository { get; }
        List<UserAction> LoadMyActions();
        void AntiTrolMechanism();
    }
}
