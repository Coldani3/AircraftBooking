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
			//get user inputs
			User user = new User(this.GetInputByIndex(0), this.GetInputByIndex(1));
			//send login packet
			Client.GetClient().SendPacket(new UserInfoPacket().Construct(user), Client.GetSocket());
			//wait for response
			Packet response = Client.GetClient().ReceivePacket<Packet>(Client.GetSocket());
			
			if (response.PacketType == -2)
			{
				//if successful, request available planes
				// Client.GetClient().SendPacket(new RequestAvailablePlanesPacket().Construct(user), Client.GetSocket());
				// SendAvailablePlanesPacket response2 = Client.GetClient().ReceivePacket<SendAvailablePlanesPacket>(Client.GetSocket());
				// //this one is async for some reason so we wait until response2 is not null.
				// while (response2 == null);
				// SessionData.Planes = response2.PlaneInfos;
				PlaneInfo[] infos = Utillity.UpdatePlanes(user);
				SessionData.CurrentUser = user;
				//go to next menu
				Program.MenuManager.ChangeMenu(new SelectPlaneMenu(infos));

				return true;
			}
			else
			{
				this.ErrorMessage = "Invalid login (check username and password)";
				return false;
			}
		}
	}
}