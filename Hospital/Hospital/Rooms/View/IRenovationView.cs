using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.View
{
    public interface IRenovationView
    {
        void ScheduleRenovation(Renovation.Type type);

        void ScheduleRenovation();

        void Renovate();
    }
}
