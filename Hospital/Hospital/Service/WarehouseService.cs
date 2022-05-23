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
        private List<Warehouse> _warehouse;


        public WarehouseService()
        {
            this._warehouseRepository = new WarehouseRepository();
            this._warehouse = this._warehouseRepository.Load();
        }


        public string GetNameEquipment(string id)
        {
            foreach(Warehouse equipment in this._warehouse)
            {
                if (equipment.IdDynamicEquipment.Equals(id))
                {
                    return equipment.NameDynamicEquipment;
                }
            }
            return "";
        }
    }
}
