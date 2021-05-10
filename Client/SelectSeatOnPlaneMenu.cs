using ConsoleMenuLibrary;
using AircraftBooking.Shared;
using AircraftBooking.Shared.Packets;
using System;

namespace AircraftBooking.Client
{
	public class SelectSeatOnPlaneMenu : TextInputMenu
	{
		PlaneInfo Plane;
		public SelectSeatOnPlaneMenu(int index) : base(new int[][] {
			new int[] {2, 8}
		}, new string[] {"Plane Seat Number:"})
		{
			Console.WriteLine($"{index}, {SessionData.Planes.Length}");
			Plane = SessionData.Planes[index];
		}

		public override bool Submit()
		{
			string selectedPlaneStr = this.GetInputByIndex(0);

			int selectedSeat;

			if (!Int32.TryParse(selectedPlaneStr, out selectedSeat))
			{
				this.ErrorMessage = "The input was NOT a number!";
				return false;
			}

			if (selectedSeat > this.Plane.MaxNumberOfSeats - 1)
			{
				this.ErrorMessage = "The plane does not have that many seats!";
				return false;
			}

			foreach (int takenPlane in this.Plane.TakenSeats)
			{
				if (selectedSeat == takenPlane)
				{
					this.ErrorMessage = "That plane is already known to be taken!";
					return false;
				}
			}

			Client.GetClient().SendPacket(new BookPlaneSeatPacket().Construct(this.Plane.PlaneID, selectedSeat, SessionData.CurrentUser), Client.GetSocket());
			//receive response packet
			Packet response = Client.GetClient().ReceivePacket<Packet>(Client.GetSocket());

			if (response.PacketType != -2)
			{
				this.ErrorMessage = "An error occurred.";
				return false;
			}

			Program.MenuManager.ChangeMenu(new SelectPlaneMenu(Utillity.UpdatePlanes(SessionData.CurrentUser)));

			return true;
		}

		public override void Display()
		{
			Console.SetCursorPosition(2, 3);
			Console.Write($"Plane name: {this.Plane.PlaneName}");
			Console.SetCursorPosition(2, 4);
			Console.Write($"Max number of seats: {this.Plane.MaxNumberOfSeats}");
			Console.SetCursorPosition(2, 5);
			Console.Write("Taken seats: ");

			for (int i = 0; i < this.Plane.TakenSeats.Length; i++)
			{
				string comma = i <= this.Plane.TakenSeats.Length - 1 ? "," : "";
				Console.Write($"{this.Plane.TakenSeats[i]}{comma} ");
			}

			base.Display();
		}
	}

}