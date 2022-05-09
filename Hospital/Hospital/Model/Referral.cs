using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
	class Referral
	{

		private string id;
		private string patientEmail;
		private string doctorEmail;
		private Appointment.Type typeOfAppointment;
		private bool used;
		

		public Referral(string id, string patientEmail, string doctorEmail, Appointment.Type typeOfAppointment, bool used)
		{
			this.id = id;
			this.patientEmail = patientEmail;
			this.doctorEmail = doctorEmail;
			this.typeOfAppointment = typeOfAppointment;
			this.used = used;
		}

		public string Id { get { return this.id; } }
		public string Patient { get { return this.patientEmail; } }
		public string Doctor { get { return this.doctorEmail; } }
		public Appointment.Type TypeProp { get { return this.typeOfAppointment; } }
		public bool Used { get { return this.used; } }


	}
}
