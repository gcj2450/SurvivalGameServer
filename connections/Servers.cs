using System.Net;

namespace SurvivalGameServer.connections
{
    internal class Servers
    {
        private TCPServer tcpServer;
        private UDPServer udpServer;
        private bool isActive;

        private static Servers instance;
        private Servers()
        {
            StartServers();
        }
        public static Servers GetInstance()
        {
            if (instance == null)
            {
                instance = new Servers();
            }
            return instance;
        }

        private void StartServers()
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

        public void StopServers()
        {
            try
            {
                tcpServer.Stop();
                udpServer.Stop();
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, "stopped TCP and UDP servers");
                isActive = false;
                instance = null;
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error stopping servers");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }
        }

        public bool SendTCP(byte[] data, Guid id)
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

        public bool SendTCP(string data, Guid id)
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
