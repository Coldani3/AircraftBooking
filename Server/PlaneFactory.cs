namespace AircraftBooking.Server
{
	//Factory for making planes
	public static class PlaneFactory
	{
		public static Plane CreatePlane(PlaneTypes planeType)
		{
			switch (planeType)
			{
				case PlaneTypes.AirbusA340:
					return new AirbusA340Plane();
				
				case PlaneTypes.Boeing747:
					return new Boeing747Plane();

				case PlaneTypes.Boeing757:
					return new Boeing757Plane();

				default: 
					return new Boeing747Plane();
			}
		}
	}

	public enum PlaneTypes
	{
		AirbusA340,
		Boeing747,
		Boeing757,
	}
}