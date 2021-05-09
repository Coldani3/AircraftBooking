using AircraftBooking.Shared.Serialisation;

namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	public class UserInfoPacket : Packet
	{
		public UserInfoPacket(User user) : base(1, Serializer.Serialize<User>(user))
		{

		}

		public User DeserializeUser()
		{
			return base.Deserialize<User>();
		}
	}
}