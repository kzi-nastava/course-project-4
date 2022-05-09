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
    class PrescriptionRepository
    {
        public List<Prescription> Load()
        {
            List<Prescription> allPrecriptions = new List<Prescription>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\prescriptions.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string idAppointment = fields[0];
                    string idDrug = fields[1];
                    DateTime startConsuming = DateTime.ParseExact(fields[2], "HH:mm", CultureInfo.InvariantCulture);
                    int dose = Int32.Parse(fields[3]);

                    Prescription newPrescription = new Prescription(idAppointment, idDrug, startConsuming, dose);
                    allPrecriptions.Add(newPrescription);


                }
            }
            return allPrecriptions;
        }
    }
}
