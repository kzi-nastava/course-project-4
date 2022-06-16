using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital;
using Autofac;
using Hospital.Rooms.Repository;
using Hospital.Rooms.Model;

namespace Hospital.Rooms.Service
{
    public class EquipmentMovingService
    {
        private IEquipmentMovingRepository _equipmentMovingRepository;
        private IEquipmentService _equipmentService;
        private IRoomService _roomService;

        public List<EquipmentMoving> AllEquipmentMovings { get { return _equipmentMovingRepository.AllEquipmentMovings; } }

        public EquipmentMovingService()
        {
            this._equipmentMovingRepository = Globals.container.Resolve<IEquipmentMovingRepository>();
            this._equipmentService = Globals.container.Resolve<IEquipmentService>();
            this._roomService = Globals.container.Resolve<IRoomService>();
        }

        public void MoveEquipment()
        {
            foreach (EquipmentMoving equipmentMoving in AllEquipmentMovings)
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
            _equipmentMovingRepository.Save(AllEquipmentMovings);
        }

        public bool IdExists(string id)
        {
            return _equipmentMovingRepository.IdExists(id);
        }

        public bool ActiveMovingExists(string equipmentId)
        {
            return _equipmentMovingRepository.ActiveMovingExists(equipmentId);
        }

        public bool IsValidEquipmentMoving(string id, string equipmentId, DateTime scheduledTime,
            string sourceRoomId, string destinationRoomId)
        {
            return !(IdExists(id) || ActiveMovingExists(equipmentId) || !_equipmentService.IdExist(equipmentId)
                || !_equipmentService.GetEquipmentById(equipmentId).RoomId.Equals(sourceRoomId)
                || !_roomService.IdExists(destinationRoomId));
        }

        public bool CreateEquipmentMoving(string id, string equipmentId, DateTime scheduledTime,
            string sourceRoomId, string destinationRoomId)
        {
            if (!IsValidEquipmentMoving(id, equipmentId, scheduledTime, sourceRoomId, destinationRoomId))
                return false;
            _equipmentMovingRepository.CreateEquipmentMoving(id, equipmentId, scheduledTime, sourceRoomId, destinationRoomId);
            return true;
        }
    }
}
