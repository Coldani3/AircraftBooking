using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	[Serializable, XmlRoot("SendAvailablePlanesPacket")]
	public class SendAvailablePlanesPacket : Packet
	{
		public PlaneInfo[] PlaneInfos;
		public SendAvailablePlanesPacket() : base(3, "")
		{

		}

		public override Packet Construct(params object[] args)
		{
			PlaneInfos = (PlaneInfo[]) args[0];
			return this;
		}
	}
}