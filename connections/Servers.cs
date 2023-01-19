using System.Net;

namespace SurvivalGameServer.connections
{
    internal class Servers
    {
        private static TCPServer tcpServer;
        private static UDPServer udpServer;
        private static bool isActive;

        public static void StartServers()
        {
            if (isActive)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Warning, "Servers allready started!");
                return;
            }

            try
            {
                tcpServer = new TCPServer(IPAddress.Any, Globals.TCP_PORT);
                udpServer = new UDPServer(IPAddress.Any, Globals.UDP_PORT);

                tcpServer.Start();                
                udpServer.Start();
                isActive = true;
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, "started TCP and UDP servers");
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error starting servers");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }
        }

        public static void StopServers()
        {
            try
            {
                tcpServer.Stop();
                udpServer.Stop();
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, "stopped TCP and UDP servers");
                isActive = false;
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error stopping servers");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }
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
