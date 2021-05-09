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
		public Hangar Planes = new Hangar();
		public bool UserLoggedIn {get => this.CurrentUser != null;}
		private Socket ClientSocket;

		private Server()
		{
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.AirbusA340));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.AirbusA340));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.Boeing747));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.Boeing757));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.Boeing747));
		}

		public void Start()
		{
			Console.WriteLine("Starting up server ...");
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
                    ClientSocket = listenerSocket.Accept();
					Socket clientSocket = ClientSocket;
					System.Console.WriteLine("Connection established!");

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
					Console.WriteLine("Received packet!");

					switch (packet.PacketType)
					{
						case 1:
							//UserInfo
							UserInfoPacket userInfoPacket = (UserInfoPacket) packet;
							User user = userInfoPacket.DeserializeUser();

							if (this.Users.TryLogin(user))
							{
								this.CurrentUser = userInfoPacket.DeserializeUser();
								this.SendPacket(new SuccessPacket().Construct("Successful login", 1), clientSocket);
								Console.WriteLine($"User {user.Username} logged in!");
							}
							else
							{
								Console.WriteLine("Invalid login attempted.");
								this.SendPacket(new InvalidPacket().Construct("Invalid login!"), clientSocket);
							}

							break;
						case 2:
							
							break;

						// case 3:
						// 	//SendAvailablePlanes
						// 	if (this.UserLoggedIn)
						// 	{
						// 		this.SendPacket(new SendPlaneInfoPacket().Construct())
						// 	}
						// 	break;

						case 4:
						//RequestPlaneSeat
							if (this.UserLoggedIn)
							{
								RequestPlaneSeatPacket rpsPacket = (RequestPlaneSeatPacket) packet;

								if (this.Planes.GetPlanesLength() <= rpsPacket.PlaneID)
								{
									Plane desiredPlane = this.Planes.GetByID(rpsPacket.PlaneID);

									if (desiredPlane.SeatAvailable(rpsPacket.SeatID))
									{
										desiredPlane.AddUserToSeat(this.CurrentUser, rpsPacket.SeatID);
										this.SendPacket(new SuccessPacket().Construct("Successfully booked seat", 2), clientSocket);
									}
									else
									{
										this.SendPacket(new InvalidPacket().Construct("Seat already booked"), clientSocket);
									}
								}
								else
								{
									this.SendPacket(new InvalidPacket().Construct("Invalid seat ID"), clientSocket);
								}
							}
							else
							{
								this.SendPacket(new InvalidPacket().Construct("Cannot request a plane seat as you are not logged in!"), clientSocket);
							}

							break;

						case 5:
							//RequestAvailablePlanes
							if (this.UserLoggedIn)
							{
								
							}
							break;

					}
                    // DataItem dataItem = DataItemSerialisation.GetDataItem(serialisedXml);
                    // WriteLine("Text received -> {0} ", dataItem.Id);
                    // DataItem response = new DataItem("Green");
                    // string serialisedItem = DataItemSerialisation.GetSerialisedDataItem(response);
                    // byte[] message = null;//Encoding.ASCII.GetBytes(serialisedItem);

                    // clientSocket.Send(message);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
			finally
			{
				ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
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