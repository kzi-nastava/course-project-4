using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
namespace Hospital.Repository
{
    class DynamicRoomEquipmentRepository
    {
        public List<DynamicRoomEquipment> Load()
        {
            List<DynamicRoomEquipment> allDynamicEquipment = new List<DynamicRoomEquipment>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\dynamicRoomEquipments.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    List<string> pairs = fields[1].Split(';').ToList();
                    Dictionary<string, int> amount = new Dictionary<string, int>();
                    foreach (string pair in pairs)
                    {
                        amount.Add(pair.Split(':')[0], Int32.Parse(pair.Split(':')[1]));
                    }

                    DynamicRoomEquipment dynamicRoomEquipment = new DynamicRoomEquipment(id, amount);
                    allDynamicEquipment.Add(dynamicRoomEquipment);


                }
            }

            return allDynamicEquipment;
        }

    }
}
