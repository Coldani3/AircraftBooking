using System;
using System.Collections.Generic;
using AircraftBooking.Shared.Serialisation;
using System.Xml.Serialization;
using System;

namespace AircraftBooking.Shared.Packets
{
	//A Mediator to facilitate transfer of information between client and server.
	//Subclasses define specific behaviour, and should have properties based on that behaviour.
	[Serializable, XmlRoot("Packet")]
    public class Packet
    {
		public int PacketType {get; set;}
		public string Data;

		public Packet()
		{
			
		}

		public Packet(int type, string data) 
		{
			this.Construct(type, data);
		}

		//Builder method because you can't serialise something without a parameterless constructor

		public virtual Packet Construct(int type, string data, params object[] args)
		{
			this.PacketType = type;
			this.Data = data;
			return this;
		}

		public virtual Packet Construct(params object[] args)
		{
			return this;
		}

		//overload with a version that returns this but casted to the correct data type 
		public virtual T Deserialize<T>()
		{
			//take Data and convert into an object
			return (T) Serializer.Deserialize<T>(this.Data);
		}
    }
}
