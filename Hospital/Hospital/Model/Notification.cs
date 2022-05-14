using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
	public class Notification
	{
		private string _id;
		private string _userEmail;
		private string _content;
		private bool _read;

		public string Id { get { return _id; } }
		public string UserEmail { get { return _userEmail; } }
		public string Content { get { return _content; } }
		public bool Read { get { return _read; } set { _read = value; } }


		public Notification(string id, string userEmail, string content, bool read)
		{
			this._id = id;
			this._userEmail = userEmail;
			this._content = content;
			this._read = read;
		}
	}
}
