namespace AircraftBooking.Shared.Packets
{
	public class RequestPlaneSeatPacket : Packet
	{
		public int PlaneID;
		public int SeatID;
		public RequestPlaneSeatPacket(int planeID, int seatID) : base(2, "")
		{
			this.PlaneID = planeID;
			this.SeatID = seatID;
		}
	}
}