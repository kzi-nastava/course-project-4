using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Equipment
    {
        public enum Type
        {
            ExaminationEquipment = 1,
            OperationEquipment = 2,
            Furniture = 3,
            HallwayEquipment = 4
        }

        private string _id;
        private string _name;
        private Type _type;
        private int _quantity;
        private string _roomId;

        public string Id { get { return _id; } }
        public string Name { get { return _name; } }
        public Type EquipmentType { get { return _type; } }
        public int Quantity { get { return _quantity; } }
        public string RoomId { get { return _roomId; } }

        public string TypeDescription
        {
            get
            {
                switch (_type)
                {
                    case Type.ExaminationEquipment:
                        return "Oprema za preglede";
                    case Type.OperationEquipment:
                        return "Oprema za operacije";
                    case Type.Furniture:
                        return "Sobni namestaj";
                    case Type.HallwayEquipment:
                    default:
                        return "Oprema za hodnike";
                }
            }
        }

        public Equipment(string id, string name, Type type, int quantity, string roomId)
        {
            this._id = id;
            this._name = name;
            this._type = type;
            this._quantity = quantity;
            this._roomId = roomId;
        }
    }
}
