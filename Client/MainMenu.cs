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
			Client.GetClient().SendPacket(new UserInfoPacket().Construct(user), Client.GetSocket());
			Packet response = Client.GetClient().ReceivePacket<Packet>(Client.GetSocket());

			System.Console.WriteLine("response: " + response.PacketType);

			if (response.PacketType != -1)
			{
				Client.GetClient().SendPacket(new RequestAvailablePlanesPacket().Construct(user), Client.GetSocket());
				SendAvailablePlanesPacket response2 = Client.GetClient().ReceivePacket<SendAvailablePlanesPacket>(Client.GetSocket());
				Program.MenuManager.ChangeMenu(new SelectPlaneMenu(response2.PlaneInfos));
			}
			else
			{
				this.ErrorMessage = "Invalid login (check username and password)";
				return false;
			}

			return base.Submit();
		}
	}
}