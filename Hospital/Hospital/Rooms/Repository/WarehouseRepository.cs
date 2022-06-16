using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public class WarehouseRepository: IWarehouseRepository
    {
        public List<DynamicEquipment> Load()
        {
            List<DynamicEquipment> warehouse = new List<DynamicEquipment>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\warehouse.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    string name = fields[1];
                    int amount = Int32.Parse(fields[2]);
                    DynamicEquipment equipment = new DynamicEquipment(id, name, amount);
                    warehouse.Add(equipment);

                }
            }

            return warehouse;
        }

        public void Save(List<DynamicEquipment> warehouseEquipment)
		{
            string filePath = @"..\..\Data\warehouse.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (DynamicEquipment equipment in warehouseEquipment)
            {
                line = equipment.Id + "," + equipment.Name + "," + equipment.Amount;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
