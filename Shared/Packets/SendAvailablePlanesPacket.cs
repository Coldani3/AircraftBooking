

namespace AircraftBooking.Shared.Packets
{
	//Client -> Server
	public class SendAvailablePlanesPacket : Packet
	{
		public PlaneInfo[] PlaneInfos;
		public SendAvailablePlanesPacket() : base(3, "")
		{

		}

		public override Packet Construct(params object[] args)
		{
			PlaneInfos = (PlaneInfo[]) args[0];
			return this;
		}
	}
}