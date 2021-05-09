using System.Xml.Serialization;
using System;
using System.IO;
using AircraftBooking.Shared.Packets;
using Newtonsoft.Json;

namespace AircraftBooking.Shared.Serialisation
{
	public class Serializer
	{
		public static string SerializePacket(Packet packet)
		{
			return Serialize<Packet>(packet);
			// XmlSerializer serialiser = new XmlSerializer(packet.GetType());
            // using (StringWriter sw = new StringWriter())
            // {
            //     serialiser.Serialize(sw, packet);
            //     return sw.ToString();
            // }
		}

		public static Packet DeserializePacket(string data)
		{
			return Deserialize<Packet>(data);
			// XmlSerializer deserialiser = new XmlSerializer(typeof(Packet));
            // using (TextReader tr = new StringReader(data))
            // {
            //     return (Packet)deserialiser.Deserialize(tr);
            // }
		}

		public static string Serialize<T>(T data)
		{
			return JsonConvert.SerializeObject(data);
			// XmlSerializer serialiser = new XmlSerializer(data.GetType());
            // using (StringWriter sw = new StringWriter())
            // {
            //     serialiser.Serialize(sw, data);
            //     return sw.ToString();
            // }
		}

		public static T Deserialize<T>(string data)
		{
			return JsonConvert.DeserializeObject<T>(data);
			// XmlSerializer deserialiser = new XmlSerializer(typeof(T));
            // using (TextReader tr = new StringReader(data))
            // {
            //     return (T)deserialiser.Deserialize(tr);
            // }
		}
	}
}