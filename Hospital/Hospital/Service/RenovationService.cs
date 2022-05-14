using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    public class RenovationService
    {
        private RenovationRepository _renovationRepository;
        private RoomService _roomService;
        private List<Renovation> _allRenovations;

        public List<Renovation> AllRenovations { get { return _allRenovations; } }

        public RenovationService(RoomService roomService) 
        {
            this._renovationRepository = new RenovationRepository();
            this._roomService = roomService;
            this._allRenovations = _renovationRepository.Load();
        }

        public bool IdExists(string id) 
        {
            foreach (Renovation renovation in _allRenovations)
            {
                if (renovation.Id.Equals(id))
                    return true;
            }
            return false;
        }

        public bool ActiveRenovationExists(string roomId) 
        {
            foreach (Renovation renovation in _allRenovations)
            {
                if (renovation.IsActive && renovation.RoomId.Equals(roomId))
                    return true;
            }
            return false;
        }

        public bool Create(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            if (IdExists(id) || endDate < startDate || !_roomService.IdExists(roomId) || ActiveRenovationExists(roomId))
                return false;
            Renovation renovation = new Renovation(id, startDate, endDate, roomId, true);
            _allRenovations.Add(renovation);
            _renovationRepository.Save(_allRenovations);
            return true;
        }
    }
}
