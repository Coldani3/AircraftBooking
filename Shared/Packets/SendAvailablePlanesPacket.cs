namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	public class SendAvailablePlanesPacket : Packet
	{
		public SendAvailablePlanesPacket() : base(3, "")
		{
		}
	}
}