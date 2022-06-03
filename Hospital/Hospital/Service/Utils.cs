using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Service
{
    class Utils
    {
        public static bool IsDateFormValid(string date)
        {
            DateTime checkDate;
            bool validDate = DateTime.TryParseExact(date, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out checkDate);
            if (!validDate)
            {
                Console.WriteLine("Nevalidan unos datuma");
                return false;
            }
            else if (checkDate <= DateTime.Now)
            {
                Console.WriteLine("Uneli ste datum koji je prosao ili je danasnji.");
                return false;
            }
            return true;
        }

        public static bool IsTimeFormValid(string time)
        {
            TimeSpan checkTime;
            bool validTime = TimeSpan.TryParse(time, out checkTime);
            if (!validTime)
            {
                Console.WriteLine("Nevalidan unos vremena");
                return false;
            }
            return true;
        }
    }
}
