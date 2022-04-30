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
        private EquipmentMovingRepository equipmentMovingRepository;
        private EquipmentService equipmentService;
        private RoomService roomService;
        private List<EquipmentMoving> allEquipmentMovings;

        public List<EquipmentMoving> AllEquipmentMovings { get { return allEquipmentMovings; } }

        public EquipmentMovingService(EquipmentService equipmentService, RoomService roomService)
        {
            equipmentMovingRepository = new EquipmentMovingRepository();
            allEquipmentMovings = equipmentMovingRepository.Load();
            this.equipmentService = equipmentService;
            this.roomService = roomService;
        }

        public void MoveEquipment()
        {
            foreach (EquipmentMoving equipmentMoving in allEquipmentMovings)
            {
                if (!equipmentMoving.Active)
                    continue;

                if (DateTime.Now < equipmentMoving.ScheduledTime)
                    continue;

                Equipment equipment = equipmentService.GetEquipmentById(equipmentMoving.EquipmentId);
                equipmentService.UpdateEquipment(equipment.Id, equipment.Name, equipment.Type, equipment.Quantity, 
                    equipmentMoving.DestinationRoomId);
                equipmentMoving.Active = false;
            }
            equipmentMovingRepository.Save(allEquipmentMovings);
        }

        public bool DoesIdExist(string id)
        {
            foreach (EquipmentMoving equipmentMoving in allEquipmentMovings)
            {
                if (equipmentMoving.Id.Equals(id))
                    return true;
            }
            return false;
        }

        public bool ActiveEquipmentMovingExist(string equipmentId)
        {
            foreach (EquipmentMoving equipmentMoving in allEquipmentMovings)
            {
                if (equipmentMoving.Active && equipmentMoving.EquipmentId.Equals(equipmentId))
                    return true;
            }
            return false;
        }

        public bool CreateEquipmentMoving(string id, string equipmentId, DateTime scheduledTime,
            string sourceRoomId, string destinationRoomId)
        {
            if (DoesIdExist(id) || ActiveEquipmentMovingExist(equipmentId) || !equipmentService.DoesIdExist(equipmentId)
                || !equipmentService.GetEquipmentById(equipmentId).RoomId.Equals(sourceRoomId)
                || !roomService.DoesIdExist(destinationRoomId))
                return false;
            EquipmentMoving equipmentMoving = new EquipmentMoving(id, equipmentId, scheduledTime,
                sourceRoomId, destinationRoomId, true);
            allEquipmentMovings.Add(equipmentMoving);
            equipmentMovingRepository.Save(allEquipmentMovings);
            return true;
        }
    }
}
