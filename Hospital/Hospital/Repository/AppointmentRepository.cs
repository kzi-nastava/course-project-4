using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace Hospital.Repository
{
    class AppointmentRepository
    {
        public List<Appointment> Load()
        {
            List<Appointment> allApointments = new List<Appointment>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\appointments.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string patientEmail = fields[0];
                    string doctorEmail = fields[1];
                    DateTime dateExamination = DateTime.Parse(fields[2]);
                    DateTime startExamination = DateTime.Parse(fields[3]);
                    DateTime endExamination = DateTime.Parse(fields[4]);
                    Appointment.AppointmentState state = (Appointment.AppointmentState)int.Parse(fields[5]);

                    Appointment appointment = new Appointment(patientEmail, doctorEmail, dateExamination, startExamination, endExamination, state);
                    allApointments.Add(appointment);
                }
            }

            return allApointments;
        }
    }
}
