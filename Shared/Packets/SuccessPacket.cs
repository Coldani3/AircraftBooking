namespace AircraftBooking.Shared.Packets
{
	public class SuccessPacket : Packet
	{
		public string Message;

		public SuccessPacket(string message) : base(-2, "")
		{
			this.Message = message;
		}
	}
}