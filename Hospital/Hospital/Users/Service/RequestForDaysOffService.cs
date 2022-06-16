using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital;
using Autofac;
using Hospital.Users.Repository;
using Hospital.Appointments.Repository;
using Hospital.Appointments.Model;
using Hospital.Users.Model;

namespace Hospital.Users.Service
{
    public class RequestForDaysOffService : IRequestForDaysOffService
    {
        private IRequestForDaysOffRepository _requestForDaysOffRepository;
        private IAppointmentRepository _appointmentRepository;
        private List<Appointment> _appointments;
        private List<RequestForDaysOff> _requestsForDaysOff;

        public RequestForDaysOffService()
        {
            this._requestForDaysOffRepository = Globals.container.Resolve<IRequestForDaysOffRepository>();
            this._requestsForDaysOff = this._requestForDaysOffRepository.Load();
            this._appointmentRepository = Globals.container.Resolve<IAppointmentRepository>();
            this._appointments = this._appointmentRepository.Load();

        }

        public List<RequestForDaysOff> RequestsForDaysOff { get { return _requestsForDaysOff; } set { _requestsForDaysOff = value; } }


        public string GetNewId()
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
                Console.WriteLine("Zahtev mora biti zatražen barem dva dana pre slobodnih dana ili ste uneli datum u proslosti! Pokusajte ponovo.");
                return false;
            }

            return true;
        }


        public bool CheckAlreadyAvailableForSelectedDate(DateTime startDate, DateTime endDate, User doctor)
        {
            foreach (RequestForDaysOff request in this._requestsForDaysOff)
            {
                if (request.EmailDoctor.Equals(doctor.Email))
                {
                    if (CheckOverlapDate(startDate, endDate, request) && request.StateRequired != RequestForDaysOff.State.Rejected)
                    {
                        return true;
                    }
                }

            }
            return false;

        }
        public bool CheckingAvailabilityOfDoctor(DateTime startDate, DateTime endDate, User doctor)
        {
            foreach (Appointment appointment in this._appointments)
            {
                if (appointment.DoctorEmail.Equals(doctor.Email) && appointment.AppointmentState != Appointment.State.Deleted)
                {
                    if (startDate <= appointment.DateAppointment && appointment.DateAppointment <= endDate)
                    {
                        Console.WriteLine("Nije moguce podneti zahtev za slobodne dane u ovom terminu jer imate zakazane preglede!");
                        return false;
                    }
                }
            }
            if (CheckAlreadyAvailableForSelectedDate(startDate, endDate, doctor))
            {
                Console.WriteLine("Vec ste slobodni u ovo vreme, pogledajte raspored i izaberite validan zahtev!");
                return false;
            }
            return true;

        }


        public bool CheckOverlapDate(DateTime startDate, DateTime endDate, RequestForDaysOff request)
        {
            if ((startDate <= request.StartDate) && (request.EndDate <= endDate))
            {
                return true;
            }
            else if ((request.StartDate <= endDate) && (endDate <= request.EndDate))
            {
                return true;
            }
            else if ((request.StartDate <= startDate) && (startDate <= request.EndDate))
            {
                return true;
            }
            else if ((request.StartDate <= startDate) && endDate <= request.EndDate)
            {
                return true;
            }
            return false;
        }

        public void Add(RequestForDaysOff request)
        {
            this._requestsForDaysOff.Add(request);
            this._requestForDaysOffRepository.Save(this._requestsForDaysOff);
        }

        public void AnswerRequest(RequestForDaysOff pendingRequest)
        {
            foreach (RequestForDaysOff request in _requestsForDaysOff)
            {
                if (request.Id == pendingRequest.Id)
                {
                    request.StateRequired = pendingRequest.StateRequired;
                    request.ReasonRequired = pendingRequest.ReasonRequired;
                    break;
                }
            }
            this._requestForDaysOffRepository.Save(this._requestsForDaysOff);
        }

    }
}