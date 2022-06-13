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
    public class DynamicEquipmentRequestRepository
	{
		public List<DynamicEquipmentRequest> Load()
		{
            List<DynamicEquipmentRequest> requests = new List<DynamicEquipmentRequest>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\equipmentRequests.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    int amount = Int32.Parse(fields[1]);
                    DateTime addTime = DateTime.ParseExact(fields[2], "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                    bool used = Convert.ToBoolean(fields[3]);
                    DynamicEquipmentRequest request = new DynamicEquipmentRequest(id, amount, addTime, used);
                    requests.Add(request);
                }
            }

            return requests;
        }

        public void Save(List<DynamicEquipmentRequest> equipmentRequests)
		{
            string filePath = @"..\..\Data\equipmentRequests.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (DynamicEquipmentRequest request in equipmentRequests)
            {
                line = request.DynamicEquipmentId + "," + request.Amount.ToString() + ","
                    + request.AddTime.ToString("MM/dd/yyyy HH:mm") + "," + request.Updated;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());

        }
	}
}
