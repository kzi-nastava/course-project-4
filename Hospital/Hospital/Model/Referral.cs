using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
	class Referral
	{

		private string _id;
		private string _patientEmail;
		private string _doctorEmail;
		private DoctorUser.Speciality _speciality;
		private Appointment.Type _typeOfAppointment;
		private bool _used;
		


		public Referral(string id, string patientEmail, string doctorEmail, DoctorUser.Speciality speciality, Appointment.Type typeOfAppointment, bool used)
		{
			this._id = id;
			this._patientEmail = patientEmail;
			this._doctorEmail = doctorEmail;
			this._speciality = speciality;
			this._typeOfAppointment = typeOfAppointment;
			this._used = used;
		}

		public string Id { get { return this._id; } }
		public string Patient { get { return this._patientEmail; } }

		public string Doctor { get { return this._doctorEmail; }  }

<<<<<<< HEAD
		public DoctorUser.Speciality DoctorSpeciality { get; set; }
=======
		public DoctorUser.Speciality DoctorSpeciality { get { return _speciality; } set { _speciality = value; } }
>>>>>>> 2b1fd344e0346c177ca9bb10390f9556ba3a044e

		public Appointment.Type TypeProp { get { return this._typeOfAppointment; } }
		public bool Used { get { return this._used; } set { this._used = value; } }


	}
}
