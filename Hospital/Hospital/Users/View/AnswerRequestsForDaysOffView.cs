using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hospital.Users.Model;
using Hospital.Users.Service;

namespace Hospital.Users.View
{
    public class AnswerRequestsForDaysOffView
	{
		private List<RequestForDaysOff> _pendingRequests;
		private RequestForDaysOffService _requestForDaysOffService;
		private NotificationService _notificationService;
		private UserService _userService;

		public AnswerRequestsForDaysOffView()
		{
			this._requestForDaysOffService = new RequestForDaysOffService();
			this._pendingRequests = FilterPending(_requestForDaysOffService.RequestsForDaysOff);
			this._userService = new UserService();
			this._notificationService = new NotificationService();
		}

		public List<RequestForDaysOff> FilterPending(List<RequestForDaysOff> requests)
		{
			List<RequestForDaysOff> pendingRequests = new List<RequestForDaysOff>();
			foreach(RequestForDaysOff request in requests)
			{
				if(request.StateRequired == RequestForDaysOff.State.Waiting)
				{
					pendingRequests.Add(request);
				}
			}
			return pendingRequests;
		}

		public void ShowRequests()
		{
			for(int i = 0; i < _pendingRequests.Count; i++)
			{
				RequestForDaysOff request = _pendingRequests[i];
				Console.WriteLine("{0}. Doktor : {1} | Od: {2} | Do: {3} | Razlog : {4}", i + 1, _userService.GetUserFullName(request.EmailDoctor),
					request.StartDate.ToString("MM/dd/yyyy"), request.EndDate.ToString("MM/dd/yyyy"), request.ReasonRequired);
			}
		}

		public int EnterRequestIndex()
		{
			string requestIndexInput;
			int requestIndex;
			do
			{
				Console.WriteLine("Unesite redni broj zahteva koji zelite da obradite");
				Console.Write(">>");
				requestIndexInput = Console.ReadLine();
			} while (!int.TryParse(requestIndexInput, out requestIndex) || requestIndex < 1 || requestIndex > _pendingRequests.Count);
			return requestIndex;
		}

		public RequestForDaysOff SelectRequest()
		{
			ShowRequests();
			int index = EnterRequestIndex();
			return _pendingRequests[index-1];
		}

		public int GetAction()
		{
			string actionIndexInput;
			int actionIndex;
			do
			{
				Console.WriteLine("\nIzaberite akciju koju zelite da izvrsite: ");
				Console.WriteLine("1. Prihvati zahtev");
				Console.WriteLine("2. Odbij zahtev");
				Console.Write(">>");
				actionIndexInput = Console.ReadLine();
			}
			while (!int.TryParse(actionIndexInput, out actionIndex) || actionIndex < 1 || actionIndex > 2);
			return actionIndex;
		}

		public void AcceptRequest(RequestForDaysOff request)
		{
			request.StateRequired = RequestForDaysOff.State.Accepted;
			_requestForDaysOffService.AnswerRequest(request);
			_notificationService.SendVacationNotification(request.EmailDoctor, request.StartDate, request.EndDate, "");
		}

		public void RejectRequest(RequestForDaysOff request)
		{
			request.StateRequired = RequestForDaysOff.State.Rejected;
			Console.WriteLine("Unesite razlog za odbijanje zahteva");
			Console.Write(">>");
			var reason = Console.ReadLine();
			request.ReasonRequired = reason;
			_requestForDaysOffService.AnswerRequest(request);
			_notificationService.SendVacationNotification(request.EmailDoctor, request.StartDate, request.EndDate, reason);
		}

		public void AnswerRequest()
		{
			if(_pendingRequests.Count == 0)
			{
				Console.WriteLine("Trenutno nema zahteva za obradu.");
				return;
			}
			RequestForDaysOff request = SelectRequest();
			int actionIndex = GetAction();
			if (actionIndex == 1)
				AcceptRequest(request);
			else
				RejectRequest(request);
		}

	}
}
