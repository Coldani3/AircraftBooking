using System;
using System.Collections.Generic;
using AircraftBooking.Shared.Serialisation;

namespace AircraftBooking.Shared.Packets
{
	//A Mediator to facilitate transfer of information between client and server.
	//Subclasses define specific behaviour, and should have properties based on that behaviour.
    public class Packet
    {
		public int PacketType {get; private set;}
		public string Data;

		public Packet(int type, string data) 
		{
			this.PacketType = type;
			this.Data = data;
		}

		//overload with a version that returns this but casted to the correct data type 
		public virtual T Deserialize<T>()
		{
			//take Data and convert into an object
			return (T) Serializer.Deserialize<T>(this.Data);
		}
    }
}
