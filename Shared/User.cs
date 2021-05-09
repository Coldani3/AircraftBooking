namespace AircraftBooking.Shared
{
	public class User
	{
		public string Username;
		private string Password;

		public User(string username, string password)
		{
			this.Username = username;
			this.Password = password;
			
		}

		public bool CheckPassword(string password)
		{
			return password == this.Password;
		}

		public string GetPassword()
		{
			return this.Password;
		}

		public override string ToString()
		{
			return this.Username;
		}
	}
}