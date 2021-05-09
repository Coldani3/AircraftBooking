using AircraftBooking.Shared.Packets;
using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared
{
	[Serializable, XmlRoot("SendPlaneInfoPacket")]
	public class SendPlaneInfoPacket : Packet
	{
		public int PlaneID;
		public string PlaneName;
		public int[] TakenSeats;

		public SendPlaneInfoPacket() : base(2, "")
		{
			
		}

		public override Packet Construct(params object[] args)
		{
			this.PlaneID = (int) args[0];
			this.PlaneName = (string) args[1];
			this.TakenSeats = (int[]) args[3];
			return this;
		}
	}
}