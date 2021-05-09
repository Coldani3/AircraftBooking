using AircraftBooking.Shared;

namespace AircraftBooking.Server
{
	public class Plane
	{
		public string Name {get; private set;}
		public Seat[] Seats;

		public Plane(string name, int seats)
		{
			this.Name = name;
			this.Seats = new Seat[seats];
		}

		public void AddUserToSeat(User user, int seatID)
		{
			if (this.Seats[seatID] == null)
			{
				this.Seats[seatID] = new Seat(user);
			}
			else
			{
				this.Seats[seatID].SetOccupant(user);
			}
		}
	}
}