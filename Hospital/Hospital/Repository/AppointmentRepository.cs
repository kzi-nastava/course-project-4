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
    public class AppointmentRepository
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
                    DateTime dateAppointment = DateTime.ParseExact(fields[3], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime startExamination = DateTime.ParseExact(fields[4], "HH:mm", CultureInfo.InvariantCulture);
                    DateTime endExamination = DateTime.ParseExact(fields[5], "HH:mm", CultureInfo.InvariantCulture);
                    Appointment.State state = (Appointment.State)int.Parse(fields[6]);
                    int roomNumber = Int32.Parse(fields[7]);
                    Appointment.Type term = (Appointment.Type)int.Parse(fields[8]);
                    bool appointmentPerformed = Convert.ToBoolean(fields[9]);
                    bool urgent = Convert.ToBoolean(fields[10]);

                    Appointment appointment = new Appointment(id, patientEmail, doctorEmail, dateAppointment, 
                        startExamination, endExamination, state, roomNumber, term, appointmentPerformed, urgent);
                    allApointments.Add(appointment);
                }
            }

            return allApointments;
        }
    }
}
