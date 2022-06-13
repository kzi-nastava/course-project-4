using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Rooms.Model
{
    public class DynamicEquipment
    {
        private string _id;
        private string _name;
        private int _amount;

        public DynamicEquipment(string id, string name, int amount)
        {
            this._id = id;
            this._name = name;
            this._amount = amount;
        }

        public string Id { get { return _id; } set { _id = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public int Amount { get { return _amount; } set { _amount = value; } }
    }
}
