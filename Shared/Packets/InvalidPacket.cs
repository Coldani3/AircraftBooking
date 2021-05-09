using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared.Packets
{
	[Serializable, XmlRoot("InvalidPacket")]
	public class InvalidPacket : Packet
	{
		public string ErrorMessage;
		public InvalidPacket() : base(-1, "")
		{

		}

		public override Packet Construct(params object[] args)
		{
			this.ErrorMessage = (string) args[0];
			return this;
		}

		public string GetMessage()
		{
			return this.ErrorMessage;
		}
	}
}