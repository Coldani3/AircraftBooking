using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	[Serializable, XmlRoot("BookPlaneSeatPacket")]
	public class BookPlaneSeatPacket : HasUserPacket
	{
		public int PlaneID;
		public int SeatID;
		public BookPlaneSeatPacket() : base(4, "")
		{
			
		}

		public override Packet Construct(params object[] args)
		{
			this.PlaneID = (int) args[0];
			this.SeatID = (int) args[1];
			return base.Construct(args);
		}
	}
}