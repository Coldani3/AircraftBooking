namespace AircraftBooking.Shared.Packets
{
	public class UserInfoPacket : Packet
	{
		public UserInfoPacket(string data) : base(1, data)
		{

		}

		public User DeserializeUser()
		{
			return base.Deserialize<User>();
		}
	}
}