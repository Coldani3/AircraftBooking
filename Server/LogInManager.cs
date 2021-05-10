using System.Collections.Generic;
using AircraftBooking.Shared;

namespace AircraftBooking.Server
{
	public class LogInManager
	{
		public List<string> LoggedInUsers = new List<string>();

		public bool UserLoggedIn(User user)
		{
			return this.LoggedInUsers.Contains(user.Username);
		}
	}
}