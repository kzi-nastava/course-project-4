using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital
{
    public class Utils
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

        public static bool CompareTwoTimes(string startTime, string endTime)
        {
            DateTime initialTime = DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture);
            DateTime latestTime = DateTime.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture);

            if (initialTime.AddMinutes(15) > latestTime)
                return false;

            return initialTime < latestTime;
        }

        public static string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public static bool IsTimeBetweenTwoTimes(DateTime time)
        {
            DateTime midnight = DateTime.ParseExact("00:00", "HH:mm", CultureInfo.InvariantCulture);
            DateTime earliestTime = DateTime.ParseExact("06:00", "HH:mm", CultureInfo.InvariantCulture);
            if (time.TimeOfDay >= midnight.TimeOfDay && time.TimeOfDay < earliestTime.TimeOfDay)
                return true;
            return false;
        }
    }
}
