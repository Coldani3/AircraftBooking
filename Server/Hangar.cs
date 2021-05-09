using System.Collections.Generic;

namespace AircraftBooking.Server
{
	public class Hangar
	{
		public List<Plane> Planes = new List<Plane>();

		public Hangar()
		{

		}

		public void AddPlane(Plane plane)
		{
			this.Planes.Add(plane);
		}

		public Plane GetByID(int planeID)
		{
			return this.Planes[planeID];
		}
	}
}