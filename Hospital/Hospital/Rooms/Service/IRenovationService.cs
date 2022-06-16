using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public interface IRenovationService
    {
        List<Renovation> AllRenovations { get; }

        bool IdExists(string id);

        bool ActiveRenovationExists(string roomId);

        bool IsRenovationValid(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type);

        bool CreateRenovation(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type);

        void CreateSimpleRenovation(string id, DateTime startDate, DateTime endDate, string roomId);

        void CreateSplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId); 

        bool IsMergeRenovationValid(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId);

        bool CreateMergeRenovation(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId);

        void Renovate();
    }
}
