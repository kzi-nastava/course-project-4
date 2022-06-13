using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Rooms.Model;

namespace Hospital.Rooms.Repository
{
    public class EquipmentMovingRepository
    {
        private static string s_filePath = @"..\..\Data\equipmentMovings.csv";

        public List<EquipmentMoving> Load()
        {
            List<EquipmentMoving> equipmentMovings = new List<EquipmentMoving>();

            using (TextFieldParser parser = new TextFieldParser(s_filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string id = fields[0];
                    string equipmentId = fields[1];
                    DateTime scheduledTime = DateTime.ParseExact(fields[2], "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string sourceRoomId = fields[3];
                    string destinationRoomId = fields[4];
                    bool active = bool.Parse(fields[5]);

                    EquipmentMoving equipmentMoving = new EquipmentMoving(id, equipmentId, scheduledTime,
                        sourceRoomId, destinationRoomId, active);
                    equipmentMovings.Add(equipmentMoving);
                }
            }

            return equipmentMovings;
        }

        public void Save(List<EquipmentMoving> equipmentMovings)
        {
            string[] lines = new string[equipmentMovings.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                EquipmentMoving equipmentMoving = equipmentMovings[i];
                lines[i] = equipmentMoving.Id + "," + equipmentMoving.EquipmentId + "," 
                    + equipmentMoving.ScheduledTime.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + ","
                    + equipmentMoving.SourceRoomId + "," + equipmentMoving.DestinationRoomId + "," + equipmentMoving.IsActive;
            }

            File.WriteAllLines(s_filePath, lines);
        }
    }
}
