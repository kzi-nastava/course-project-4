using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public interface IRenovationRepository : IRepository<Renovation>
    {
        List<Renovation> AllRenovations { get; }

        bool IdExists(string id);

        void CreateRenovation(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type);

        void CreateSimpleRenovation(string id, DateTime startDate, DateTime endDate, string roomId);

        void CreateSplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId);

        void CreateMergeRenovation(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId);
    }
}
