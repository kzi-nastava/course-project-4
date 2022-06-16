using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;
namespace Hospital.Users.Service
{
    public interface IRequestForDaysOffService: IService<RequestForDaysOff>
    {
        void AnswerRequest(RequestForDaysOff pendingRequest);
        void Add(RequestForDaysOff request);
        bool CheckOverlapDate(DateTime startDate, DateTime endDate, RequestForDaysOff request);
        bool CheckingAvailabilityOfDoctor(DateTime startDate, DateTime endDate, User doctor);
        bool CheckAlreadyAvailableForSelectedDate(DateTime startDate, DateTime endDate, User doctor);
        bool IsDateValid(string date);
        string GetNewId();
    }
}
