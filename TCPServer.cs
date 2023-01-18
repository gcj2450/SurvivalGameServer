using NetCoreServer;
using System.Net.Sockets;
using System.Net;

namespace SurvivalGameServer
{
    class TCPServer : TcpServer
    {
        public TCPServer(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new TCPSession(this); }

        protected override void OnError(SocketError error)
        {
            Program.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP server caught an error with code {error}");
        }
    }
}
