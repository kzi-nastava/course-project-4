using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Rooms.Model
{
    public class Room
    {
        public enum Type
        {
            OperationRoom = 1,
            ExaminationRoom = 2,
            RestRoom = 3,
            Other = 4,
            Warehouse = 5
        }

        private string _id;
        private string _name;
        private Type _type;
        private bool _deleted;

        public string Id { get { return _id; } }
        public string Name { get { return _name; } }
        public Type RoomType { get { return _type; } }
        public bool IsDeleted { get { return _deleted; } set { _deleted = value; } }

        public string TypeDescription 
        { 
            get 
            { 
                switch(_type)
                {
                    case Type.OperationRoom:
                        return "Operaciona sala";
                    case Type.ExaminationRoom:
                        return "Sala za preglede";
                    case Type.RestRoom:
                        return "Soba za odmor";
                    case Type.Warehouse:
                        return "Magacin";
                    case Type.Other:
                    default:
                        return "Druga soba";
                }
            } 
        }

        public Room(string id, string name, Type type, bool deleted)
        {
            this._id = id;
            this._name = name;
            this._type = type;
            this._deleted = deleted;
        }
    }
}
