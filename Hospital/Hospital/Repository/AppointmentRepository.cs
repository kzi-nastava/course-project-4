using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Microsoft.VisualBasic.FileIO;

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

                    string id = fields[0];
                    string patientEmail = fields[1];
                    string doctorEmail = fields[2];
                    DateTime dateExamination = DateTime.Parse(fields[3]);
                    DateTime startExamination = DateTime.Parse(fields[4]);
                    DateTime endExamination = DateTime.Parse(fields[5]);
                    Appointment.AppointmentState state = (Appointment.AppointmentState)int.Parse(fields[6]);

                    Appointment appointment = new Appointment(id, patientEmail, doctorEmail, dateExamination, startExamination, endExamination, state);
                    allApointments.Add(appointment);
                }
            }

            return allApointments;
        }
    }
}
