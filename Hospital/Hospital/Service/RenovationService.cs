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
        private AppointmentService _appointmentService;
        private List<Renovation> _allRenovations;

        public List<Renovation> AllRenovations { get { return _allRenovations; } }

        public RenovationService(RoomService roomService, AppointmentService appointmentService) 
        {
            this._renovationRepository = new RenovationRepository();
            this._roomService = roomService;
            this._appointmentService = appointmentService;
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

        public bool Create(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type)
        {
            if (IdExists(id) || endDate < startDate || !_roomService.IdExists(roomId) || ActiveRenovationExists(roomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, roomId))
                return false;
            Renovation renovation = new Renovation(id, startDate, endDate, roomId, true, type);
            _allRenovations.Add(renovation);
            _renovationRepository.Save(_allRenovations);
            return true;
        }

        public bool CreateSimpleRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            return Create(id, startDate, endDate, roomId, Renovation.Type.SimpleRenovation);
        }

        public bool CreateSplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            return Create(id, startDate, endDate, roomId, Renovation.Type.SplitRenovation);
        }

        public bool CreateMergeRenovation(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId)
        {
            if (IdExists(id) || endDate < startDate || !_roomService.IdExists(roomId) || !_roomService.IdExists(otherRoomId)
                || ActiveRenovationExists(roomId) || ActiveRenovationExists(otherRoomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, roomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, otherRoomId))
                return false;
            Renovation renovation = new MergeRenovation(id, startDate, endDate, roomId, true, otherRoomId);
            _allRenovations.Add(renovation);
            _renovationRepository.Save(_allRenovations);
            return true;
        }
    }
}
