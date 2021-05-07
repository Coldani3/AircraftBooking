namespace AircraftBooking.Server
{
	public class Plane
	{
		public string Name {get; private set;}
		public Seat[] Seats;

		public Plane(string name)
		{
			this.Name = name;
		}
	}
}