namespace AircraftBooking.Shared
{
	public class PlaneInfo
	{
		public int PlaneID;
		public string PlaneName;
		public int[] TakenSeats;
		public int MaxNumberOfSeats;

		public PlaneInfo()
		{
			
		}

		public PlaneInfo(int id, string name, int[] takenSeats, int maxNumberOfSeats)
		{
			this.PlaneID = id;
			this.PlaneName = name;
			this.TakenSeats = takenSeats;
			this.MaxNumberOfSeats = maxNumberOfSeats;
		}

	}
}