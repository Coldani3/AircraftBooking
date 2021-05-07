using System.Xml.Serialization;
using System;
using System.IO;
using AircraftBooking.Shared.Packets;

namespace AircraftBooking.Shared.Serialisation
{
	public class Serializer
	{
		public static string SerializePacket(Packet packet)
		{
			XmlSerializer serialiser = new XmlSerializer(packet.GetType());
            using (StringWriter sw = new StringWriter())
            {
                serialiser.Serialize(sw, packet);
                return sw.ToString();
            }
		}

		public static Packet DeserializePacket(string data)
		{
			XmlSerializer deserialiser = new XmlSerializer(typeof(Packet));
            using (TextReader tr = new StringReader(data))
            {
                return (Packet)deserialiser.Deserialize(tr);
            }
		}

		public static string Serialize<T>(T data)
		{
			XmlSerializer serialiser = new XmlSerializer(data.GetType());
            using (StringWriter sw = new StringWriter())
            {
                serialiser.Serialize(sw, data);
                return sw.ToString();
            }
		}

		public static T Deserialize<T>(string data)
		{
			XmlSerializer deserialiser = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(data))
            {
                return (T)deserialiser.Deserialize(tr);
            }
		}
	}
}