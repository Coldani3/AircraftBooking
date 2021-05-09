using ConsoleMenuLibrary;

namespace AircraftBooking.Client
{
	public class SelectSeatOnPlaneMenu : TextInputMenu
	{
		public SelectSeatOnPlaneMenu(int index) : base(new int[][] {
			new int[] {2, 4}
		}, new string[] {"Plane Number"})
		{

		}

		public override bool Submit()
		{
			return base.Submit();
		}
	}

}