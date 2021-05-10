using AircraftBooking.Shared;
using System.Collections.Generic;

namespace AircraftBooking.Server
{
	public class Plane
	{
		public string Name {get; private set;}
		public Seat[] Seats;
		public List<int> TakenSeats = new List<int>();

		public Plane(string name, int seats)
		{
			this.Name = name;
			this.Seats = new Seat[seats];
		}

		//Builder method
		public Plane AddUserToSeat(User user, int seatID)
		{
			//check to see if available
			if (this.Seats[seatID] == null)
			{
				//if no seat there instantiate one
				this.Seats[seatID] = new Seat(user);
				this.TakenSeats.Add(seatID);
			}
			else
			{
				//otherwise overwrite
				this.Seats[seatID].SetOccupant(user);
			}

			return this;
		}

		public bool SeatAvailable(int seatID)
		{
			return this.Seats[seatID] == null;
		}

		public int[] GetTakenSeatIndexes()
		{
			return this.TakenSeats.ToArray();
		}
	}
}