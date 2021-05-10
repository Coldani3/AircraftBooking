using System.Collections.Generic;
using AircraftBooking.Shared;

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

		public int GetPlanesLength()
		{
			return this.Planes.Count;
		}

		public PlaneInfo[] GetPlaneInfos()
		{
			List<PlaneInfo> infos = new List<PlaneInfo>();

			for (int i = 0; i < this.Planes.Count; i++)
			{
				infos.Add(new PlaneInfo(i, this.Planes[i].Name, this.Planes[i].GetTakenSeatIndexes(), this.Planes[i].Seats.Length));
			}

			return infos.ToArray();
		}
	}
}