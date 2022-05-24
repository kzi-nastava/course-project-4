using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    class RequestForDaysOffService
    {
        private RequestForDaysOffRepository _requestForDaysOffRepository;
        private AppointmentRepository _appointmentRepository;
        private List<Appointment> _appointments;
        private List<RequestForDaysOff> _requestsForDaysOff;


        public RequestForDaysOffService()
        {
            this._requestForDaysOffRepository = new RequestForDaysOffRepository();
            this._requestsForDaysOff = this._requestForDaysOffRepository.Load();
            this._appointmentRepository = new AppointmentRepository();
            this._appointments = this._appointmentRepository.Load();
            
        }


        public List<RequestForDaysOff> RequirementsForDaysOff { get { return _requestsForDaysOff; } set { _requestsForDaysOff = value; } }

        public void UpdateFile()
        {
            string filePath = @"..\..\Data\requestsForDaysOff.csv";
            List<string> lines = new List<String>();

            string line;
            foreach (RequestForDaysOff request in this._requestsForDaysOff)
            {
                line = request.Id + ";" + request.EmailDoctor + ";" + request.StartDate.ToString("MM/dd/yyyy") +
                    ";" + request.EndDate.ToString("MM/dd/yyyy") + ";" + request.ReasonRequired + ";" + (int)request.StateRequired + ";" + request.Urgen.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines.ToArray());
        }
        public string GetNewRequestId()
        {
            return (this._requestsForDaysOff.Count + 1).ToString();
        }

        public bool IsDateValid(string date)
        {
            DateTime checkDate;
            bool validDate = DateTime.TryParseExact(date, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out checkDate);
            if (!validDate)
            {
                Console.WriteLine("Nevalidan unos datuma");
                return false;
            }
            else if (checkDate <= DateTime.Now.AddDays(2))
            {
                Console.WriteLine("Zahtev mora biti zatražen barem dva dana pre slobodnih dana!");
                return false;
            }
            return true;
        }

        public bool CheckingAvailabilityOfDoctor(DateTime startDate, DateTime endDate, User doctor)
        {
            foreach (Appointment appointment in this._appointments)
            {
                if (appointment.DoctorEmail.Equals(doctor.Email) && appointment.AppointmentState!=Appointment.State.Deleted)
                {
                    if(startDate<= appointment.DateAppointment && appointment.DateAppointment <= endDate)
                    {
                        Console.WriteLine("Nije moguce podneti zahtev za slobodne dane u ovom terminu jer imate zakazane preglede!");
                        return false;
                    }
                }
            }
            return true;

        }


    }
}
