using AircraftBooking.Shared.Packets;

namespace AircraftBooking.Shared
{
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