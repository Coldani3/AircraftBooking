using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AircraftBooking.Shared.Packets;
using AircraftBooking.Shared.Serialisation;
using AircraftBooking.Shared;

namespace AircraftBooking.Client
{
	public class Client
	{
		private static Client Instance = new Client();
		private static Socket SenderSocket;
		private static IPEndPoint endPoint;

		public Client()
		{

		}

		public void Start()
		{
			try
            {
                IPHostEntry ipHostDetails = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddressDetails = ipHostDetails.AddressList[0];
                endPoint = new IPEndPoint(ipAddressDetails, 4242);

                SenderSocket = new Socket(ipAddressDetails.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
					//set up socket
                    SenderSocket.Connect(endPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
		}

		public void Shutdown()
		{
			SenderSocket.Shutdown(SocketShutdown.Both);
			SenderSocket.Close();
		}

		public T ReceivePacket<T>(Socket socket) where T : Packet
		{
			try
			{
				byte[] messageReceived = new byte[4096];

				// Receive answer from server
				int byteRecv = socket.Receive(messageReceived);
				string response = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

				return Serializer.Deserialize<T>(response);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return (T) (new InvalidPacket().Construct("A network exception occurred."));
			}
		}

		public void SendPacket(Packet packet, Socket socket)
		{
			//socket.Connect(endPoint);
			socket.Send(Encoding.ASCII.GetBytes(Serializer.Serialize<Packet>(packet) + "<EOF>"));
		}

		public static Client GetClient()
		{
			return Instance;
		}

		public static Socket GetSocket()
		{
			return SenderSocket;
		}
	}
}