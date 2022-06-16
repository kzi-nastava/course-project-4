using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.View;

namespace Hospital
{
    public class App
    { 
        static void Main(string[] args)
        {
            Globals.Load();
            Login login = new Login();
            login.LogIn();
        }
    }
}
