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

		public Client()
		{

		}

		public void Start()
		{
			try
            {
                IPHostEntry ipHostDetails = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddressDetails = ipHostDetails.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddressDetails, 4242);

                SenderSocket = new Socket(ipAddressDetails.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    // DataItem request = new DataItem("Theresa");
                    // string serialisedItem = DataItemSerialisation.GetSerialisedDataItem(request);
                    SenderSocket.Connect(localEndPoint);
                    // Console.WriteLine("Socket connected to -> {0} ",
                    //             SenderSocket.RemoteEndPoint);
					
					//while (Program.Running)
					//{
                    // Send data request to server
                    // byte[] messageToSend = Encoding.ASCII.GetBytes(serialisedItem + "<EOF>");
                    // int byteSent = SenderSocket.Send(messageToSend);

                    // // Data buffer 
                    // byte[] messageReceived = new byte[4096];

                    // // Recieve answer from server
                    // int byteRecv = SenderSocket.Receive(messageReceived);
                    // string response = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    //DataItem dataItem = DataItemSerialisation.GetDataItem(response);
                    //Console.WriteLine("Received from Server -> {0}", dataItem.Id);
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

		public Packet ReceivePacket(Socket socket)
		{
			try
			{
				byte[] messageReceived = new byte[4096];

				// Recieve answer from server
				int byteRecv = socket.Receive(messageReceived);
				string response = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

				return Serializer.Deserialize<Packet>(response);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return new InvalidPacket().Construct("A network exception occurred.");
			}
		}

		public void SendPacket(Packet packet, Socket socket)
		{
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