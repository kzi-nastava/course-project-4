using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
	class RequestService
	{
		private AppointmentService _appointmentService;
		private RequestRepository _requestRepository;
		private List<Appointment> _requests;

		public List<Appointment> Requests { get { return this._requests; } }

		public RequestService()
		{
			_requestRepository = new RequestRepository();
			_requests = _requestRepository.Load();
			this._appointmentService = new AppointmentService();
		}

		public Appointment FindInitialAppointment(string id)
		{
			Appointment initialAppointment = null;
			foreach (Appointment appointment in _appointmentService.Appointments)
			{
				if (id == appointment.AppointmentId)
				{
					initialAppointment = appointment;
					break;
				}
			}
			return initialAppointment;
		}

		public void DenyRequest(Appointment request)
		{
			_requests.Remove(request);
			UpdateFile();
			List<Appointment> allAppointments = _appointmentService.Appointments;
			foreach (Appointment appointment in allAppointments)
			{
				if (appointment.AppointmentId == request.AppointmentId)
				{
					appointment.AppointmentState = Appointment.State.Created;
					break;
				}

			}
			this._appointmentService.Update();
			
		}

		public void AcceptRequest(Appointment request)
		{
			_requests.Remove(request);
			UpdateFile();
			List<Appointment> allAppointments = _appointmentService.Appointments;
			foreach (Appointment appointment in allAppointments)
			{
				if (appointment.AppointmentId == request.AppointmentId)
				{
					if (request.AppointmentState == Appointment.State.DeleteRequest)
						appointment.AppointmentState = Appointment.State.Deleted;
					else
					{
						appointment.DoctorEmail = request.DoctorEmail;
						appointment.DateAppointment = request.DateAppointment;
						appointment.StartTime = request.StartTime;
						appointment.EndTime = request.EndTime;
						appointment.AppointmentState = Appointment.State.Updated;
					}


					break;
				}
			}

			this._appointmentService.Update();
		
		}

		public void ProcessRequest(Appointment request, int choice)
		{
			if (choice == 2)
			{
				DenyRequest(request);
			}
			else
			{
				AcceptRequest(request);
			}
		}

		public List<Appointment> FilterPending()
		{
			List<Appointment> pendingRequests = new List<Appointment>();
			foreach (Appointment request in _requests)
			{
				if (request.DateAppointment > DateTime.Now)
				{
					pendingRequests.Add(request);
				}
			}
			return pendingRequests;
		}

		public void UpdateFile()
		{
			this._requestRepository.Save(Requests);
		}

	}
}
