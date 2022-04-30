using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Equipment
    {
        public enum TypeOfEquipment
        {
            ExaminationEquipment = 1,
            OperationEquipment = 2,
            Furniture = 3,
            HallwayEquipment = 4
        }

        private string id;
        private string name;
        private TypeOfEquipment type;
        private int quantity;
        private string roomId;

        public string Id { get { return id; } }
        public string Name { get { return name; } }
        public TypeOfEquipment Type { get { return type; } }
        public int Quantity { get { return quantity; } }
        public string RoomId { get { return roomId; } }

        public string TypeStr
        {
            get
            {
                switch (type)
                {
                    case TypeOfEquipment.ExaminationEquipment:
                        return "Oprema za preglede";
                    case TypeOfEquipment.OperationEquipment:
                        return "Oprema za operacije";
                    case TypeOfEquipment.Furniture:
                        return "Sobni namestaj";
                    case TypeOfEquipment.HallwayEquipment:
                    default:
                        return "Oprema za hodnike";
                }
            }
        }

        public Equipment(string id, string name, TypeOfEquipment type, int quantity, string roomId)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.quantity = quantity;
            this.roomId = roomId;
        }
    }
}
