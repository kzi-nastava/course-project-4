using System;
using System.Collections.Generic;
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
        private List<RequestForDaysOff> _requestsForDaysOff;


        public RequestForDaysOffService()
        {
            this._requestForDaysOffRepository = new RequestForDaysOffRepository();
            this._requestsForDaysOff = this._requestForDaysOffRepository.Load();
            
        }


        public List<RequestForDaysOff> RequirementsForDaysOff { get { return _requestsForDaysOff; } set { _requestsForDaysOff = value; } }
    }
}
