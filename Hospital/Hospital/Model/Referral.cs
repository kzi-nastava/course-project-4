﻿using System;
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
		private Appointment.Type _typeOfAppointment;
		private bool _used;
		

		public Referral(string id, string patientEmail, string doctorEmail, Appointment.Type typeOfAppointment, bool used)
		{
			this._id = id;
			this._patientEmail = patientEmail;
			this._doctorEmail = doctorEmail;
			this._typeOfAppointment = typeOfAppointment;
			this._used = used;
		}

		public string Id { get { return this._id; } }
		public string Patient { get { return this._patientEmail; } }
		public string Doctor { get { return this._doctorEmail; } }
		public Appointment.Type TypeProp { get { return this._typeOfAppointment; } }
		public bool Used { get { return this._used; } set { this._used = value; } }


	}
}
