using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    public class EquipmentRepository
    {
        private static string s_filePath = @"..\..\Data\equipment.csv";

        public List<Equipment> Load()
        {
            List<Equipment> allEquipment = new List<Equipment>();

            using (TextFieldParser parser = new TextFieldParser(s_filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string id = fields[0];
                    string name = fields[1];
                    Equipment.Type type = (Equipment.Type)int.Parse(fields[2]);
                    int quantity = int.Parse(fields[3]);
                    string roomId = fields[4];

                    Equipment equipment = new Equipment(id, name, type, quantity, roomId);
                    allEquipment.Add(equipment);
                }
            }

            return allEquipment;
        }

        public void Save(List<Equipment> allEquipment)
        {
            string[] lines = new string[allEquipment.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                Equipment equipment = allEquipment[i];
                lines[i] = equipment.Id + "," + equipment.Name + "," + ((int)equipment.EquipmentType).ToString() + "," 
                    + equipment.Quantity.ToString() + "," + equipment.RoomId;
            }

            File.WriteAllLines(s_filePath, lines);
        }
    }
}
