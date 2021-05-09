using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared.Packets
{
	[Serializable, XmlRoot("SuccessPacket")]
	public class SuccessPacket : Packet
	{
		public string Message;
		public int SuccessID;

		public SuccessPacket() : base(-2, "")
		{

		}

		public override Packet Construct(params object[] args)
		{
			this.Message = (string) args[0];
			this.SuccessID = (int) args[1];
			return this;
		}
	}
}