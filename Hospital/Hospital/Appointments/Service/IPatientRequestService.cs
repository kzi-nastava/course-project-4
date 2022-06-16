using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Appointments.Model;

namespace Hospital.Appointments.Service
{
	public interface IPatientRequestService
	{
		List<Appointment> Requests{	get; }
		Appointment FindInitialAppointment(string id);
		void DenyRequest(Appointment request);
		void AcceptRequest(Appointment request);
		void ProcessRequest(Appointment request, int choice);
		List<Appointment> FilterPending();
		void UpdateFile();
	}
}
