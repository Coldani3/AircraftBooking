using ConsoleMenuLibrary;
using AircraftBooking.Shared;
using AircraftBooking.Shared.Packets;

namespace AircraftBooking.Client
{
	public class MainMenu : TextInputMenu
	{
		public MainMenu() : base(new int[][] {
			new int[] {4, 4},
			new int[] {4, 5},
		}, new string[] {"Username:", "Password:"})
		{

		}

		public override bool Submit()
		{
			User user = new User(this.GetInputByIndex(0), this.GetInputByIndex(1));
			Client.GetClient().SendPacket(new UserInfoPacket(user), Client.GetSocket());
			Packet response = Client.GetClient().ReceivePacket(Client.GetSocket());

			if (response.PacketType != -1)
			{
				Client.GetClient().SendPacket(new RequestAvailablePlanesPacket(), Client.GetSocket());
			}

			return base.Submit();
		}
	}
}