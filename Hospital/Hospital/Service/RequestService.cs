using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
	class RequestService
	{
		private AppointmentService appointmentService;
		private RequestRepository requestRepository;
		private List<Appointment> requests;

		public List<Appointment> Requests { get { return this.requests; } }

		public RequestService(AppointmentService appointmentService) {
			requestRepository = new RequestRepository();
			requests = requestRepository.Load();
			this.appointmentService = appointmentService;
		}

		public void ProcessRequest(Appointment request, int choice)
		{
			//if(choice == 2)
			//{
			//	DenyRequest(request);
			//}
			//else
			//{
			//	AcceptRequest(request);
			//}
		}


	}
}
