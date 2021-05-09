using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AircraftBooking.Shared.Packets;
using AircraftBooking.Shared.Serialisation;
using AircraftBooking.Shared;

namespace AircraftBooking.Server
{
	public class Server 
	{
		private static Server Instance = new Server();
		public User CurrentUser;
		public UserDatabase Users = new UserDatabase("users.txt");
		public bool UserLoggedIn {get => this.CurrentUser != null;}

		private Server()
		{

		}

		public void Start()
		{
			IPHostEntry ipHostDetails = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddressDetails = ipHostDetails.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddressDetails, 4242);

            Socket listenerSocket = new Socket(ipAddressDetails.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);
			
			try
            {
                listenerSocket.Bind(localEndPoint);
                listenerSocket.Listen(10);

                while (true)
                {
                    Socket clientSocket = listenerSocket.Accept();

                    // Data buffer 
                    byte[] bytes = new Byte[4096];
                    string data = null;
                    // Get the data from the server
                    while (true)
                    {
                        int numberOfBytes = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numberOfBytes);
                        if (data.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
						{
                            break;
						}
                    }
                    string serialisedXml = data.Substring(0,data.Length - 5);
					Packet packet = Serializer.Deserialize<Packet>(serialisedXml);

					switch (packet.PacketType)
					{
						case 1:
							//UserInfo
							UserInfoPacket userInfoPacket = (UserInfoPacket) packet;
							User user = userInfoPacket.DeserializeUser();

							if (this.Users.TryLogin(user))
							{
								this.CurrentUser = userInfoPacket.DeserializeUser();
								this.SendPacket(new SuccessPacket("Successful login"), clientSocket);
							}
							else
							{
								this.SendPacket(new InvalidPacket("Invalid login!"), clientSocket);
							}

							break;
						case 2:
							//RequestPlaneSeat
							if (this.UserLoggedIn)
							{

							}
							else
							{
								this.SendPacket(new InvalidPacket("Cannot request a plane seat as you are not logged in!"), clientSocket);
							}
							break;

					}
                    // DataItem dataItem = DataItemSerialisation.GetDataItem(serialisedXml);
                    // WriteLine("Text received -> {0} ", dataItem.Id);
                    // DataItem response = new DataItem("Green");
                    // string serialisedItem = DataItemSerialisation.GetSerialisedDataItem(response);
                    byte[] message = null;//Encoding.ASCII.GetBytes(serialisedItem);

                    clientSocket.Send(message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

		public void SendPacket(Packet packet, Socket socket)
		{
			socket.Send(Encoding.ASCII.GetBytes(Serializer.Serialize<Packet>(packet)));
		}

		//Singleton as there should only ever be one Server.
		public static Server GetServer()
		{
			return Instance;
		}
	}
}