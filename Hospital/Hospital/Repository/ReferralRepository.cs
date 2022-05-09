﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
	class ReferralRepository
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
                    bool used = Convert.ToBoolean(fields[3]);
                    Referral newReferral = new Referral(id, patientEmail, doctorEmail, used);
                    allReferrals.Add(newReferral);
                }
            }

            return allReferrals;
        }
    }
}
