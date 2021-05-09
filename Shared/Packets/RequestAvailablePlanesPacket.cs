namespace AircraftBooking.Shared.Packets
{
	public class RequestAvailablePlanesPacket : HasUserPacket
	{
		public RequestAvailablePlanesPacket() : base(5, "")
		{

		}
	}
}