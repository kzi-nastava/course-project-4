using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Rooms.Model
{
    public class DynamicRoomEquipment
    {
        private string _idRoom;
        private Dictionary<string, int> _amountOfEquipment;

        public DynamicRoomEquipment(string idRoom, Dictionary<string,int> amount)
        {
            this._idRoom = idRoom;
            this._amountOfEquipment = amount;
        }

        public string IdRoom { get { return _idRoom; } set { _idRoom = value; } }

       
        public Dictionary<string, int> AmountEquipment { get { return _amountOfEquipment; } set { _amountOfEquipment = value; } }

        public override string ToString()
        {
            string amountOfEquipments ="";
            foreach(KeyValuePair<string,int> pair in this._amountOfEquipment)
            {
                amountOfEquipments += pair.Key + ":" + pair.Value.ToString() + ";";
            }
            return this._idRoom + "," + amountOfEquipments.Remove(amountOfEquipments.Length - 1);
        }
    }
}
