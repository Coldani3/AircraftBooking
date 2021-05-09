using System;

namespace AircraftBooking.Server
{
    class ServerMain
    {
        static void Main(string[] args)
        {
            Server.GetServer().Start();
        }
    }
}
