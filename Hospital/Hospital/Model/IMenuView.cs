using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
     public abstract class IMenuView
    {
        public void LogOut()
        {
            Login loging = new Login();
            loging.LogIn();
        }
    }
}
