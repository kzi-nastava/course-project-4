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
		private AppointmentService appointmentService;
		private RequestRepository requestRepository;
		private List<Appointment> requests;

		public List<Appointment> Requests { get { return this.requests; } }

		public RequestService(AppointmentService appointmentService) {
			requestRepository = new RequestRepository();
			requests = requestRepository.Load();
			this.appointmentService = appointmentService;
		}

		public void UpdateFile()
		{
			string filePath = @"..\..\Data\requests.csv";

			if(requests.Count == 0)
			{
				File.WriteAllText(filePath, string.Empty);

			}
			else
			{
				List<string> lines = new List<String>();

				string line;
				foreach (Appointment request in requests)
				{
					line = request.AppointmentId+","+request.PatientEmail+","+request.DoctorEmail+","+request.DateAppointment.ToString("MM/dd/yyyy")+
						","+request.StartTime.ToString("HH:mm")+","+request.EndTime.ToString("HH:mm") + ","+(int)request.AppointmentStateProp+","
						+request.RoomNumber+","+(int)request.GetTypeOfTerm;
					lines.Add(line);
				}
				File.WriteAllLines(filePath, lines.ToArray());
			}

		}

		public Appointment findInitialAppointment(string id)
		{
			Appointment initialAppointment = null;
			foreach(Appointment appointment in appointmentService.Appointments)
			{
				if(id == appointment.AppointmentId)
				{
					initialAppointment = appointment;
					break;
				}
			}
			return initialAppointment;
		}

		public void DenyRequest(Appointment request)
		{
			requests.Remove(request);
			UpdateFile();
			List<Appointment> allAppointments = appointmentService.Appointments;
			foreach(Appointment appointment in allAppointments)
			{
				if(appointment.AppointmentId == request.AppointmentId)
				{
					appointment.AppointmentStateProp = Appointment.AppointmentState.Created;
					break;
				}
					
			}
			appointmentService.UpdateFile();
			
		}

		public void AcceptRequest(Appointment request)
		{
			requests.Remove(request);
			UpdateFile();
			List<Appointment> allAppointments = appointmentService.Appointments;
			foreach(Appointment appointment in allAppointments)
			{
				if(appointment.AppointmentId == request.AppointmentId)
				{
					if (request.AppointmentStateProp == Appointment.AppointmentState.DeleteRequest)
						appointment.AppointmentStateProp = Appointment.AppointmentState.Deleted;
					else
					{
						appointment.DoctorEmail = request.DoctorEmail;
						appointment.DateAppointment = request.DateAppointment;
						appointment.StartTime = request.StartTime;
						appointment.EndTime = request.EndTime;
						appointment.AppointmentStateProp = Appointment.AppointmentState.Updated;
					}
						
						
					break;
				}
			}

			appointmentService.UpdateFile();
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
	}
}
