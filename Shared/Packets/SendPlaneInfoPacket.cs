using AircraftBooking.Shared.Packets;

namespace AircraftBooking.Shared
{
	public class SendPlaneInfoPacket : Packet
	{
		public int PlaneID;
		public string PlaneName;
		public bool[] TakenSeats;

		public SendPlaneInfoPacket(int id, string name, bool[] takenSeats) : base(3, "")
		{
			this.PlaneID = id;
			this.PlaneName = name;
			this.TakenSeats = takenSeats;
		}
	}
}