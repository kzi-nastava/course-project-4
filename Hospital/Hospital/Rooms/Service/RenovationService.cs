using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Rooms.Repository;
using Hospital.Appointments.Service;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public class RenovationService : IRenovationService
    {
        private IRenovationRepository _renovationRepository;
        private IRoomService _roomService;
        private AppointmentService _appointmentService;
        private IEquipmentService _equipmentService;
        
        public RenovationService(IRenovationRepository renovationRepository, IRoomService roomService, AppointmentService appointmentService, IEquipmentService equipmentService) 
        {
            this._renovationRepository = renovationRepository;
            this._roomService = roomService;
            this._appointmentService = appointmentService;
            this._equipmentService = equipmentService;
        }
        
        public List<Renovation> AllRenovations { get { return _renovationRepository.AllRenovations; } }

        public bool IdExists(string id) 
        {
            return _renovationRepository.IdExists(id);
        }

        public bool ActiveRenovationExists(string roomId) 
        {
            foreach (Renovation renovation in AllRenovations)
            {
                if (renovation.IsActive && renovation.RoomId.Equals(roomId))
                    return true;
            }
            return false;
        }

        public bool IsRenovationValid(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type)
        {
            return !(IdExists(id) || endDate < startDate || !_roomService.IdExists(roomId) || ActiveRenovationExists(roomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, roomId));
        }

        public bool CreateRenovation(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type)
        {
            if (!IsRenovationValid(id, startDate, endDate, roomId, type))
                return false;
            _renovationRepository.CreateRenovation(id, startDate, endDate, roomId, type);
            return true;
        }

        public void CreateSimpleRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            _renovationRepository.CreateSimpleRenovation(id, startDate, endDate, roomId);
        }

        public void CreateSplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            _renovationRepository.CreateSplitRenovation(id, startDate, endDate, roomId);
        }

        public bool IsMergeRenovationValid(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId)
        {
            return !(IdExists(id) || endDate < startDate || !_roomService.IdExists(roomId) || !_roomService.IdExists(otherRoomId)
                || ActiveRenovationExists(roomId) || ActiveRenovationExists(otherRoomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, roomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, otherRoomId));
        }

        public bool CreateMergeRenovation(string id, DateTime startDate, DateTime endDate, string roomId, string otherRoomId)
        {
            if (!IsMergeRenovationValid(id, startDate, endDate, roomId, otherRoomId))
                return false;
            _renovationRepository.CreateMergeRenovation(id, startDate, endDate, roomId, otherRoomId);
            return true;
        }

        public void Renovate()
        {
            foreach (Renovation renovation in AllRenovations)
            {
                if (!renovation.IsActive || renovation.EndDate >= DateTime.Today)
                    continue;

                renovation.Renovate(_roomService, _equipmentService);
            }
            _renovationRepository.Save(AllRenovations);
        }
    }
}
