using Serilog;
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

            Program.Logger.Write(Serilog.Events.LogEventLevel.Information, "started TCP and UDP servers");
        }

        public static void StopServers()
        {
            tcpServer.Stop();
            udpServer.Stop();
            Program.Logger.Write(Serilog.Events.LogEventLevel.Information, "stopped TCP and UDP servers");
        }

        public static bool SendTCP(byte[] data, Guid id)
        {
            if (tcpServer.FindSession(id) != null)
            {
                return tcpServer.FindSession(id).SendAsync(data);
            }
            else
            {
                return false;
            }
            
        }

        public static bool SendTCP(string data, Guid id)
        {
            if (tcpServer.FindSession(id) != null)
            {
                return tcpServer.FindSession(id).SendAsync(data);
            }
            else
            {
                return false;
            }
        }
    }
}
