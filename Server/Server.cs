using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AircraftBooking.Shared.Packets;
using AircraftBooking.Shared.Serialisation;
using AircraftBooking.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AircraftBooking.Server
{
	public class Server 
	{
		private static Server Instance = new Server();
		public static bool Running = true;
		public UserDatabase Users = new UserDatabase("users.txt");
		public Hangar Planes = new Hangar();
		
		public LogInManager LogInManager = new LogInManager();

		//public List<string> LoggedInUsers = new List<string>();
		private Dictionary<string, Socket> UserToSockets = new Dictionary<string, Socket>();

		private Server()
		{
			User dummyUser = new User("DummyUser", "expandThatDomain");
			User dummyUser2 = new User("non kind shrine", "fukum4Mizu5h1");
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.AirbusA340).AddUserToSeat(dummyUser, 1).AddUserToSeat(dummyUser, 4).AddUserToSeat(dummyUser, 9).AddUserToSeat(dummyUser2, 16));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.AirbusA340).AddUserToSeat(dummyUser, 3).AddUserToSeat(dummyUser2, 6).AddUserToSeat(dummyUser2, 12).AddUserToSeat(dummyUser, 24));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.Boeing747).AddUserToSeat(dummyUser, 5));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.Boeing757).AddUserToSeat(dummyUser, 1).AddUserToSeat(dummyUser2, 75));
			this.Planes.AddPlane(PlaneFactory.CreatePlane(PlaneTypes.Boeing747).AddUserToSeat(dummyUser, 7).AddUserToSeat(dummyUser, 9));
		}

		public void Start()
		{
			Console.WriteLine("Starting up server ...");
			//initialise listener stuff
			IPHostEntry ipHostDetails = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddressDetails = ipHostDetails.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddressDetails, 4242);

			TcpListener listener = new TcpListener(localEndPoint);
			//start listening
			listener.Start();
			
			try
            {
				while (Running)
				{
					//continuously listen for new users
					Console.WriteLine("Listening ...");
					Socket clientSocket = listener.AcceptSocket();

					//spawn new user's session in another thread
					Task listenerTask = new Task(() => {
						ListenForUserAndBegin(localEndPoint, clientSocket);
					});

					listenerTask.Start();
				}
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
			finally
			{
				//shut down in case of error
				Console.WriteLine("Shutting down!");
				foreach (Socket socket in UserToSockets.Values)
				{
					socket.Shutdown(SocketShutdown.Both);
					socket.Close();
				}
			}
        }

		public void SendPacket(Packet packet, Socket socket)
		{
			//serialize packet and send
			socket.Send(Encoding.ASCII.GetBytes(Serializer.Serialize<Packet>(packet)));
		}

		public void ListenForUserAndBegin(IPEndPoint localEndPoint, Socket clientSocket)
		{
			//we have a connection
			System.Console.WriteLine("Connection established!");

			while (Running)
			{
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
				//deserialize
				string serialisedJson = data.Substring(0,data.Length - 5);
				Console.WriteLine($"Received serialised packet: {serialisedJson}");
				Packet packet = Serializer.Deserialize<Packet>(serialisedJson);
				Console.WriteLine($"Received packet {packet.PacketType}!");
				User currUser;

				//we seem to need this do while (false) 'loop' as the program seems to think breaks in the switch statement
				//mean break out of the whole loop.
				do
				{
					//switch based on packet IDs and act accordingly
					switch (packet.PacketType)
					{
						case 1:
							//UserInfo
							//casting from base class to superclass doesn't work so we just deserialize again
							UserInfoPacket userInfoPacket = Serializer.Deserialize<UserInfoPacket>(serialisedJson);//(UserInfoPacket) packet;
							User user = userInfoPacket.User;

							//check if credentials valid
							if (this.Users.TryLogin(user) && !this.LogInManager.LoggedInUsers.Contains(user.Username))
							{
								//log them in
								this.LogInManager.LoggedInUsers.Add(user.Username);
								this.UserToSockets.Add(user.Username, clientSocket);
								//send back packet to say it's successful
								//this is fine as we know clientsocket is the right socket
								this.SendPacket(new SuccessPacket().Construct("Successful login", 1), clientSocket);
								Console.WriteLine($"User {user.Username} logged in!");
							}
							else
							{
								//tell user off.
								Console.WriteLine("Invalid login attempted.");
								this.SendPacket(new InvalidPacket().Construct("Invalid login!"), clientSocket);
							}

							break;

						case 4:
						//BookPlaneSeat
							BookPlaneSeatPacket bpsPacket = Serializer.Deserialize<BookPlaneSeatPacket>(serialisedJson);//(BookPlaneSeatPacket) packet;
							currUser = bpsPacket.User;

							//check to see if user is logged in
							if (this.LogInManager.UserLoggedIn(bpsPacket.User))
							{
								//check to see if reasonable request
								if (this.Planes.GetPlanesLength() >= bpsPacket.PlaneID)
								{
									Plane desiredPlane = this.Planes.GetByID(bpsPacket.PlaneID);

									//check to see if seat available
									if (desiredPlane.SeatAvailable(bpsPacket.SeatID))
									{
										desiredPlane.AddUserToSeat(currUser, bpsPacket.SeatID);
										this.SendPacket(new SuccessPacket().Construct("Successfully booked seat", 2), this.UserToSockets[currUser.Username]);
									}
									else
									{
										this.SendPacket(new InvalidPacket().Construct("Seat already booked"), this.UserToSockets[currUser.Username]);
									}
								}
								else
								{
									this.SendPacket(new InvalidPacket().Construct("Invalid seat ID"), this.UserToSockets[currUser.Username]);
								}
							}
							else
							{
								this.SendPacket(new InvalidPacket().Construct("Cannot request a plane seat as you are not logged in!"), clientSocket);
							}

							break;

						case 5:
							Console.WriteLine("About to send planes.");
							//RequestAvailablePlanes
							RequestAvailablePlanesPacket rapPacket = Serializer.Deserialize<RequestAvailablePlanesPacket>(serialisedJson);//(RequestAvailablePlanesPacket) packet;
							currUser = rapPacket.User;

							if (this.LogInManager.UserLoggedIn(rapPacket.User))
							{
								PlaneInfo[] infos = this.Planes.GetPlaneInfos();

								Console.WriteLine($"Sending {infos.Length} planes!");

								this.SendPacket(new SendAvailablePlanesPacket().Construct(infos), this.UserToSockets[currUser.Username]);
							}
							else
							{
								this.SendPacket(new InvalidPacket().Construct("Cannot request planes as you are not logged in!"), clientSocket);
							}
							break;
						
						case 6:
							//Log out
							LogOutPacket logOutPacket = Serializer.Deserialize<LogOutPacket>(serialisedJson);

							if (this.LogInManager.LoggedInUsers.Contains(logOutPacket.User.Username))
							{
								Console.WriteLine($"Logging out user {logOutPacket.User}!");
								this.LogInManager.LoggedInUsers.Remove(logOutPacket.User.Username);
								// this.UserToSockets[logOutPacket.User.Username].Shutdown(SocketShutdown.Both);
								// this.UserToSockets[logOutPacket.User.Username].Close();
							}

							break;

					}
				} while (false);
			}
		}

		//Singleton as there should only ever be one Server.
		public static Server GetServer()
		{
			return Instance;
		}
	}
}