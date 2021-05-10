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

			int selectedPlane;

			if (!Int32.TryParse(selectedPlaneStr, out selectedPlane))
			{
				this.ErrorMessage = "The input was NOT a number!";
				return false;
			}

			if (selectedPlane > this.Plane.MaxNumberOfSeats - 1)
			{
				this.ErrorMessage = "The plane does not have that many seats!";
				return false;
			}

			foreach (int takenPlane in this.Plane.TakenSeats)
			{
				if (selectedPlane == takenPlane)
				{
					this.ErrorMessage = "That plane is already known to be taken!";
					return false;
				}
			}

			Client.GetClient().SendPacket(new BookPlaneSeatPacket().Construct(this.Plane.PlaneID, selectedPlane, SessionData.CurrentUser), Client.GetSocket());
			Program.MenuManager.ChangeMenu(Program.MenuManager.GetPreviousMenu());

			Utillity.UpdatePlanes(SessionData.CurrentUser);





			return base.Submit();
		}

		public override void Display()
		{
			Console.SetCursorPosition(2, 4);
			Console.WriteLine($"Max number of seats: {this.Plane.MaxNumberOfSeats}");
			Console.Write("Taken seats: ");

			for (int i = 0; i < this.Plane.TakenSeats.Length; i++)
			{
				string comma = i <= this.Plane.TakenSeats.Length - 1 ? "," : "";
				Console.Write($"{this.Plane.TakenSeats[i]}{comma} ");
			}

			// foreach (int takenSeat in this.Plane.TakenSeats)
			// {
			// 	Console.Write($"{takenSeat}");
			// }

			base.Display();
		}
	}

}