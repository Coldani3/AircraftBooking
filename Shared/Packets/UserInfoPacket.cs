using AircraftBooking.Shared.Serialisation;
using System.Xml.Serialization;
using System.Xml;
using System;

namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	[Serializable, XmlRoot("UserInfoPacket")]
	public class UserInfoPacket : HasUserPacket
	{
		public UserInfoPacket() : base(1, "")
		{

		}

		public UserInfoPacket(User user) : base(1, Serializer.Serialize<User>(user))
		{

		}

		public User DeserializeUser()
		{
			return base.Deserialize<User>();
		}
	}
}