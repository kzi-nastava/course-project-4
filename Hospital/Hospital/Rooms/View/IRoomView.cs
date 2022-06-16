using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Rooms.View
{
    public interface IRoomView
    {
        void ManageRooms();

        void CreateRoom();

        void ListRooms();

        void UpdateRoom();

        void DeleteRoom();
    }
}
