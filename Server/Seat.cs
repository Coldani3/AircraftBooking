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
			this.Occupant = occupant;
		}
	}
}