using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;
using Hospital.Users.Repository;
using Hospital.Users.View;

namespace Hospital.Users.Service
{
    public interface IUserActionService
    {
        IUserActionRepository ActionRepository { get; }
        List<UserAction> LoadMyActions(string patientEmail);
        void AntiTrolMechanism(Patient patient);
    }
}
