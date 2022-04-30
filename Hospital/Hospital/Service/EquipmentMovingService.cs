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
        private List<EquipmentMoving> allEquipmentMovings;

        public List<EquipmentMoving> AllEquipmentMovings { get { return allEquipmentMovings; } }

        public EquipmentMovingService(EquipmentService equipmentService)
        {
            equipmentMovingRepository = new EquipmentMovingRepository();
            this.equipmentService = equipmentService;
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
    }
}
