namespace AircraftBooking.Shared
{
	public class PlaneInfo
	{
		public int PlaneID;
		public string PlaneName;
		public int[] TakenSeats;

		public PlaneInfo()
		{
			
		}

		public PlaneInfo(int id, string name, int[] takenSeats)
		{
			this.PlaneID = id;
			this.PlaneName = name;
			this.TakenSeats = takenSeats;
		}

	}
}