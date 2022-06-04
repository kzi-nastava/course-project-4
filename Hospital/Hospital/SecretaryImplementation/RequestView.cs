﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.SecretaryImplementation
{
	class RequestView
	{
		public RequestService _requestService;
		private UserService _userService;

		public RequestView()
		{
			this._requestService = new RequestService();
			this._userService = new UserService();
		}

		public Appointment SelectRequest()
		{
			List<Appointment> requests = _requestService.FilterPending();
			if (requests.Count == 0)
			{
				Console.WriteLine("Trenutno nema zahteva za obradu. ");
				return null;
			}
			ShowRequests(requests);
			Console.WriteLine("\nx. Odustani");
			Console.WriteLine("--------------------------------------------");
			string requestIndexInput;
			int requestIndex;
			do
			{
				Console.WriteLine("Unesite redni broj zahteva koji zelite da obradite");
				Console.Write(">>");
				requestIndexInput = Console.ReadLine();
				if (requestIndexInput == "x")
				{
					return null;
				}
			} while (!int.TryParse(requestIndexInput, out requestIndex) || requestIndex < 1 || requestIndex > requests.Count);

			return requests[requestIndex - 1];
		}

		public void ShowRequests(List<Appointment> requests)
		{

			for (int i = 0; i < requests.Count; i++)
			{
				Appointment request = requests[i];
				switch (request.AppointmentState)
				{
					case (Appointment.State.UpdateRequest):
						Appointment oldValuesAppointment = _requestService.FindInitialAppointment(request.AppointmentId);
						Console.Write("{0}. {1}, {2}->{3}, {4}->{5}, {6}->{7}, ", i + 1, _userService.GetUserFullName(oldValuesAppointment.PatientEmail),
							oldValuesAppointment.DateAppointment.ToString("MM/dd/yyyy"), request.DateAppointment.ToString("MM/dd/yyyy"),
							oldValuesAppointment.StartTime.ToString("HH:mm"), request.StartTime.ToString("HH:mm"),
							oldValuesAppointment.EndTime.ToString("HH:mm"), request.EndTime.ToString("HH:mm"));
						Console.Write("Izmena termina");
						break;
					case (Appointment.State.DeleteRequest):
						Console.Write("{0}. {1}, {2}, {3}, {4}, ", i + 1, _userService.GetUserFullName(request.PatientEmail), request.DateAppointment.ToString("MM/dd/yyyy"),
							request.StartTime.ToString("HH:mm"), request.EndTime.ToString("HH:mm"));
						Console.Write("Brisanje termina");
						break;
				}
			}

		}
	}
}
