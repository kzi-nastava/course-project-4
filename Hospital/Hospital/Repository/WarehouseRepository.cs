using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    class WarehouseRepository
    {
        public List<Warehouse> Load()
        {
            List<Warehouse> warehouse = new List<Warehouse>();

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
                    Warehouse equipment = new Warehouse(id, name, amount);
                    warehouse.Add(equipment);




                }
            }

            return warehouse;
        }
    }
}
