using AircraftBooking.Shared.Packets;
using AircraftBooking.Shared;

namespace AircraftBooking.Client
{
	public static class Utillity
	{
		public static PlaneInfo[] UpdatePlanes(User user)
		{
			Client.GetClient().SendPacket(new RequestAvailablePlanesPacket().Construct(user), Client.GetSocket());
			SendAvailablePlanesPacket response2 = Client.GetClient().ReceivePacket<SendAvailablePlanesPacket>(Client.GetSocket());
			//this one is async for some reason so we wait until response2 is not null.
			while (response2 == null);
			SessionData.Planes = response2.PlaneInfos;
			return response2.PlaneInfos;
		}
	}
}