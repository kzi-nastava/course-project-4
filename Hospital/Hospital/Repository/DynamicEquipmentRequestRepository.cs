using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
	class DynamicEquipmentRequestRepository
	{
		public List<DynamicEquipmentRequest> Load()
		{
            List<DynamicEquipmentRequest> requests = new List<DynamicEquipmentRequest>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\warehouse.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    int amount = Int32.Parse(fields[1]);
                    DateTime addTime = DateTime.ParseExact(fields[2], "MM/dd/yyyy HH:MM", CultureInfo.InvariantCulture);
                    DynamicEquipmentRequest request = new DynamicEquipmentRequest(id, amount, addTime);
                    requests.Add(request);
                }
            }

            return requests;
        }
	}
}
