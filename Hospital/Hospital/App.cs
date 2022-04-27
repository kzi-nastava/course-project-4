using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital
{
    public class App
    { 
        static void Main(string[] args)
        {
            Login login = new Login();
            login.LogIn();
        }
    }
}
