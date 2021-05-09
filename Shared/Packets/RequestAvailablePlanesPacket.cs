using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared.Packets
{
	[Serializable, XmlRoot("RequestAvailablePlanesPacket")]
	public class RequestAvailablePlanesPacket : HasUserPacket
	{
		public RequestAvailablePlanesPacket() : base(5, "")
		{

		}
	}
}