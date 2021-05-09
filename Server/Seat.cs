using AircraftBooking.Shared;

namespace AircraftBooking.Server
{
	public class Seat
	{
		public User Occupant;

		public Seat()
		{

		}

		public Seat(User occupant)
		{
			this.SetOccupant(occupant);
		}

		public void SetOccupant(User occupant)
		{
			this.Occupant = occupant;
		}

		public void Empty()
		{
			this.Occupant = null;
		}
	}
}