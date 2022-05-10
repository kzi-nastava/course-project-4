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
		private UserService userService;
		private List<Appointment> requests;

		public List<Appointment> Requests { get { return this.requests; } }

		public RequestService(AppointmentService appointmentService) {
			requestRepository = new RequestRepository();
			requests = requestRepository.Load();
			this.appointmentService = appointmentService;
			this.userService = new UserService();
		}

		public string ToStringForFile(Appointment request)
		{
			string line = request.AppointmentId + "," + request.PatientEmail + "," + request.DoctorEmail + "," + request.DateAppointment.ToString("MM/dd/yyyy") +
						"," + request.StartTime.ToString("HH:mm") + "," + request.EndTime.ToString("HH:mm") + "," + (int)request.AppointmentState + ","
						+ request.RoomNumber + "," + (int)request.TypeOfTerm + "," + "false";
			return line;
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
					line = ToStringForFile(request);
					lines.Add(line);
				}
				File.WriteAllLines(filePath, lines.ToArray());
			}

		}

		public Appointment FindInitialAppointment(string id)
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
					appointment.AppointmentState = Appointment.State.Created;
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

		public List<Appointment> FilterPending()
		{
			List<Appointment> pendingRequests = new List<Appointment>();
			foreach(Appointment request in requests)
			{
				if(request.DateAppointment > DateTime.Now)
				{
					pendingRequests.Add(request);
				}
			}
			return pendingRequests;
		} 

		public void ShowRequests(List<Appointment> requests)
		{

			for (int i = 0; i < requests.Count; i++)
			{
				Appointment request = requests[i];
				switch (request.AppointmentState)
				{
					case (Appointment.State.UpdateRequest):
						Appointment oldValuesAppointment = FindInitialAppointment(request.AppointmentId);
						Console.Write("{0}. {1}, {2}->{3}, {4}->{5}, {6}->{7}, ", i + 1, userService.GetUserFullName(oldValuesAppointment.PatientEmail),
							oldValuesAppointment.DateAppointment.ToString("MM/dd/yyyy"), request.DateAppointment.ToString("MM/dd/yyyy"),
							oldValuesAppointment.StartTime.ToString("HH:mm"), request.StartTime.ToString("HH:mm"),
							oldValuesAppointment.EndTime.ToString("HH:mm"), request.EndTime.ToString("HH:mm"));
						Console.Write("Izmena termina");
						break;
					case (Appointment.State.DeleteRequest):
						Console.Write("{0}. {1}, {2}, {3}, {4}, ", i + 1, userService.GetUserFullName(request.PatientEmail), request.DateAppointment.ToString("MM/dd/yyyy"),
							request.StartTime.ToString("HH:mm"), request.EndTime.ToString("HH:mm"));
						Console.Write("Brisanje termina");
						break;
				}
			}
			
		}
	}
}
