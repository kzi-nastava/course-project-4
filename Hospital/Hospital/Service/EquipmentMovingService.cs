using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    public class EquipmentMovingService
    {
        private EquipmentMovingRepository _equipmentMovingRepository;
        private EquipmentService _equipmentService;
        private RoomService _roomService;
        private List<EquipmentMoving> _allEquipmentMovings;

        public List<EquipmentMoving> AllEquipmentMovings { get { return _allEquipmentMovings; } }

        public EquipmentMovingService(EquipmentService equipmentService, RoomService roomService)
        {
            _equipmentMovingRepository = new EquipmentMovingRepository();
            _allEquipmentMovings = _equipmentMovingRepository.Load();
            this._equipmentService = equipmentService;
            this._roomService = roomService;
        }

        public void MoveEquipment()
        {
            foreach (EquipmentMoving equipmentMoving in _allEquipmentMovings)
            {
                if (!equipmentMoving.IsActive)
                    continue;

                if (DateTime.Now < equipmentMoving.ScheduledTime)
                    continue;

                Equipment equipment = _equipmentService.GetEquipmentById(equipmentMoving.EquipmentId);
                _equipmentService.UpdateEquipment(equipment.Id, equipment.Name, equipment.EquipmentType, equipment.Quantity, 
                    equipmentMoving.DestinationRoomId);
                equipmentMoving.IsActive = false;
            }
            _equipmentMovingRepository.Save(_allEquipmentMovings);
        }

        public bool IdExists(string id)
        {
            foreach (EquipmentMoving equipmentMoving in _allEquipmentMovings)
            {
                if (equipmentMoving.Id.Equals(id))
                    return true;
            }
            return false;
        }

        public bool ActiveMovingExists(string equipmentId)
        {
            foreach (EquipmentMoving equipmentMoving in _allEquipmentMovings)
            {
                if (equipmentMoving.IsActive && equipmentMoving.EquipmentId.Equals(equipmentId))
                    return true;
            }
            return false;
        }

        public bool CreateEquipmentMoving(string id, string equipmentId, DateTime scheduledTime,
            string sourceRoomId, string destinationRoomId)
        {
            if (IdExists(id) || ActiveMovingExists(equipmentId) || !_equipmentService.IdExist(equipmentId)
                || !_equipmentService.GetEquipmentById(equipmentId).RoomId.Equals(sourceRoomId)
                || !_roomService.IdExists(destinationRoomId))
                return false;
            EquipmentMoving equipmentMoving = new EquipmentMoving(id, equipmentId, scheduledTime,
                sourceRoomId, destinationRoomId, true);
            _allEquipmentMovings.Add(equipmentMoving);
            _equipmentMovingRepository.Save(_allEquipmentMovings);
            return true;
        }
    }
}
