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
        private EquipmentService _equipmentService;
        private List<Renovation> _allRenovations;

        public List<Renovation> AllRenovations { get { return _allRenovations; } }

        public RenovationService(RoomService roomService, AppointmentService appointmentService, EquipmentService equipmentService) 
        {
            this._renovationRepository = new RenovationRepository();
            this._roomService = roomService;
            this._appointmentService = appointmentService;
            this._equipmentService = equipmentService;
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

        public bool IsRenovationValid(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type)
        {
            return !(IdExists(id) || endDate < startDate || !_roomService.IdExists(roomId) || ActiveRenovationExists(roomId)
                || _appointmentService.OverlapingAppointmentExists(startDate, endDate, roomId));
        }

        public bool CreateRenovation(string id, DateTime startDate, DateTime endDate, string roomId, Renovation.Type type)
        {
            if (!IsRenovationValid(id, startDate, endDate, roomId, type))
                return false;
            Renovation renovation = new Renovation(id, startDate, endDate, roomId, true, type);
            _allRenovations.Add(renovation);
            _renovationRepository.Save(_allRenovations);
            return true;
        }

        public bool CreateSimpleRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            return CreateRenovation(id, startDate, endDate, roomId, Renovation.Type.SimpleRenovation);
        }

        public bool CreateSplitRenovation(string id, DateTime startDate, DateTime endDate, string roomId)
        {
            return CreateRenovation(id, startDate, endDate, roomId, Renovation.Type.SplitRenovation);
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
            Renovation renovation = new MergeRenovation(id, startDate, endDate, roomId, true, otherRoomId);
            _allRenovations.Add(renovation);
            _renovationRepository.Save(_allRenovations);
            return true;
        }

        private void SplitRoom(string roomId)
        {
            Room roomBefore = _roomService.GetRoomById(roomId);
            _equipmentService.ChangeRoom(roomBefore.Id, roomBefore.Id + "_1");

            _roomService.DeleteRoom(roomBefore.Id);
            _roomService.CreateRoom(roomBefore.Id + "_1", roomBefore.Name + " 1", roomBefore.RoomType);
            _roomService.CreateRoom(roomBefore.Id + "_2", roomBefore.Name + " 2", roomBefore.RoomType);
        }

        private void MergeRooms(string roomId, string otherRoomId)
        {
            Room room1 = _roomService.GetRoomById(roomId);
            Room room2 = _roomService.GetRoomById(otherRoomId);

            string newId = room1.Id + "+" + room2.Id;
            string newName = room1.Name + " + " + room2.Name;

            _equipmentService.ChangeRoom(room1.Id, newId);
            _equipmentService.ChangeRoom(room2.Id, newId);

            _roomService.DeleteRoom(room1.Id);
            _roomService.DeleteRoom(room2.Id);
            _roomService.CreateRoom(newId, newName, room1.RoomType);
        }

        public void Renovate()
        {
            foreach (Renovation renovation in _allRenovations)
            {
                if (!renovation.IsActive || renovation.EndDate >= DateTime.Today)
                    continue;

                renovation.IsActive = false;

                if (renovation.RenovationType == Renovation.Type.SplitRenovation)
                    SplitRoom(renovation.RoomId);
                else if (renovation.RenovationType == Renovation.Type.MergeRenovation)
                    MergeRooms(renovation.RoomId, ((MergeRenovation)renovation).OtherRoomId);
            }
            _renovationRepository.Save(_allRenovations);
        }
    }
}
