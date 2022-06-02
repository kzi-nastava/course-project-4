using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class WarehouseService
    {
        private WarehouseRepository _warehouseRepository;
        private List<DynamicEquipment> _warehouseEquipment;


        public WarehouseService()
        {
            this._warehouseRepository = new WarehouseRepository();
            this._warehouseEquipment = this._warehouseRepository.Load();
        }
        
        public List<DynamicEquipment> WarehouseEquipment { get { return _warehouseEquipment; } }

        public string GetNameEquipment(string id)
        {
            foreach(DynamicEquipment equipment in this._warehouseEquipment)
            {
                if (equipment.Id.Equals(id))
                {
                    return equipment.Name;
                }
            }
            return "";
        }
    }
}
