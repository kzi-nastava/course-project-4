using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
	class DynamicEquipmentRequest
	{
		private string _dynamicEquipmentId;
		private int _amount;
		private DateTime _addTime;

		public string DynamicEquipmentId { get {return _dynamicEquipmentId;} }
		public int Amount { get { return _amount; } }
		public DateTime AddTime { get { return _addTime; } }

		public DynamicEquipmentRequest(string id, int amount, DateTime addTime)
		{
			this._dynamicEquipmentId = id;
			this._amount = amount;
			this._addTime = addTime;
		}


	}
}
