namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	public class RequestPlaneSeatPacket : Packet
	{
		public int PlaneID;
		public int SeatID;
		public RequestPlaneSeatPacket() : base(4, "")
		{
			
		}

		public override Packet Construct(int type, string data, params object[] args)
		{
			base.Construct(type, data);
			this.PlaneID = (int) args[0];
			this.SeatID = (int) args[1];
			return base.Construct(type, data);
		}
	}
}