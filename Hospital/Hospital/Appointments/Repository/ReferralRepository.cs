using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using Hospital.Appointments.Model;
using Hospital.Users.Model;
using Hospital.Users.View;

namespace Hospital.Appointments.Repository
{
    public class ReferralRepository
	{
        public List<Referral> Load()
        {
            List<Referral> allReferrals = new List<Referral>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\referrals.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string id = fields[0];
                    string patientEmail = fields[1];
                    string doctorEmail = fields[2];
                    DoctorUser.Speciality speciality = (DoctorUser.Speciality)int.Parse(fields[3]);
                    Appointment.Type type = (Appointment.Type)int.Parse(fields[4]);
                    bool used = Convert.ToBoolean(fields[5]);
                    Referral newReferral = new Referral(id, patientEmail, doctorEmail, speciality,type, used);
                    allReferrals.Add(newReferral);
                }
            }

            return allReferrals;
        }

        public void Save(List<Referral> referrals)
        {
            string filePath = @"..\..\Data\referrals.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (Referral referral in referrals)
            {
                line = referral.Id + "," + referral.Patient + "," + referral.Doctor + "," + ((int)referral.DoctorSpeciality) + "," + ((int)referral.TypeProp) + "," + referral.Used;
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
    }
}
