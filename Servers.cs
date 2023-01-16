using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class Servers
    {
        private static TCPServer tcpServer;
        private static UDPServer udpServer;
        public static void StartServers()
        {
            tcpServer = new TCPServer(IPAddress.Any, Globals.TCP_PORT);
            udpServer = new UDPServer(IPAddress.Any, Globals.UDP_PORT);

            tcpServer.Start();
            udpServer.Start();

            Console.Write("started TCP and UDP servers\n");
        }

        public static void StopServers()
        {
            tcpServer.Stop();
            udpServer.Stop();
            Console.Write("stopped TCP and UDP servers\n");
        }
    }
}
