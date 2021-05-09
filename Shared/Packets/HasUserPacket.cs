namespace AircraftBooking.Shared.Packets
{
	public class HasUserPacket : Packet
	{
		public User User;
		public HasUserPacket()
		{

		}

		public HasUserPacket(int id, string data) : base(id, data)
		{

		}

		public override Packet Construct(params object[] args)
		{
			foreach (object obj in args)
			{
				if (obj is User)
				{
					this.User = (User) obj;
				}
			}
			return this;
		}
	}
}