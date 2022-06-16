using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Appointments.Model;

namespace Hospital.Appointments.Repository
{
    public class PrescriptionRepository: IPrescriptionRepository
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
                    Prescription.TimeOfConsuming timeOfConsuming = (Prescription.TimeOfConsuming)int.Parse(fields[4]);

                    Prescription newPrescription = new Prescription(idAppointment, idDrug, startConsuming, dose, timeOfConsuming);
                    allPrecriptions.Add(newPrescription);

                }
            }
            return allPrecriptions;
        }

        public void Save(List<Prescription> prescriptions)
        {
            string filePath = @"..\..\Data\prescriptions.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (Prescription prescription in prescriptions)
            {
                line = prescription.IdAppointment + "," + prescription.IdDrug + "," + prescription.StartConsuming.ToString("HH:mm") + "," + prescription.Dose.ToString() +
                    "," + (int)prescription.TimeConsuming;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
