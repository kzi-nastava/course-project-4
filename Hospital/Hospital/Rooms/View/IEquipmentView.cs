using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Rooms.View
{
    public interface IEquipmentView
    {
        void SearchEquipment();

        void FilterEquipment();

        void FilterEquipmentByRoomType();

        void FilterEquipmentByQuantity();

        void FilterEquipmentByType();

        void ScheduleEquipmentMoving();

        void MoveEquipment();
    }
}
