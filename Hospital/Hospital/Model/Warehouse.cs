using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    class Warehouse
    {
        private string _idDynamicEquipment;
        private string _nameDynamicEquipment;
        private int _amountOfDynamicEquipment;


        public Warehouse(string id, string nameDynamicEquipment, int amount)
        {
            this._idDynamicEquipment = id;
            this._nameDynamicEquipment = nameDynamicEquipment;
            this._amountOfDynamicEquipment = amount;
        }

        public string IdDynamicEquipment { get { return _idDynamicEquipment; } set { _idDynamicEquipment = value; } }

        public string NameDynamicEquipment { get { return _nameDynamicEquipment; } set { _nameDynamicEquipment = value; } }

        public int AmountOfDynamicEquipment { get { return _amountOfDynamicEquipment; } set { _amountOfDynamicEquipment = value; } }
    }
}
