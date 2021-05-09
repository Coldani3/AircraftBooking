namespace AircraftBooking.Shared.Packets
{
	public class InvalidPacket : Packet
	{
		public string ErrorMessage;
		public InvalidPacket(string errorMessage) : base(-1, "")
		{
			this.ErrorMessage = errorMessage;
		}

		public string GetMessage()
		{
			return this.ErrorMessage;
		}
	}
}