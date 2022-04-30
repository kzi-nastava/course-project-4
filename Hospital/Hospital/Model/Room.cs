using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Room
    {
        public enum TypeOfRoom
        {
            OperationRoom = 1,
            ExaminationRoom = 2,
            RestRoom = 3,
            Other = 4
        }

        private string id;
        private string name;
        private TypeOfRoom type;

        public string Id { get { return id; } }
        public string Name { get { return name; } }
        public TypeOfRoom Type { get { return type; } }

        public Room(string id, string name, TypeOfRoom type)
        {
            this.id = id;
            this.name = name;
            this.type = type;
        }
    }
}
