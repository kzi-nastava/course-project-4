using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class RequestForDaysOffManagment
	{

		private List<RequestForDaysOff> _requests;
		private RequestForDaysOffService _requestForDaysOffService;

		public RequestForDaysOffManagment() {
			this._requestForDaysOffService = new RequestForDaysOffService();
			this._requests = _requestForDaysOffService.RequestsForDaysOff;
		}


	}
}
