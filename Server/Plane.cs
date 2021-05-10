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

		public Plane AddUserToSeat(User user, int seatID)
		{
			if (this.Seats[seatID] == null)
			{
				this.Seats[seatID] = new Seat(user);
				this.TakenSeats.Add(seatID);
			}
			else
			{
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
			// List<int> Indexes = new List<int>();

			// for (int i = 0; i < this.Seats.Length; i++)
			// {
			// 	if (this.Seats[i].Occupied())
			// 	{
			// 		Indexes.Add(i);
			// 	}
			// }

			// return Indexes.ToArray();

			return this.TakenSeats.ToArray();
		}
	}
}