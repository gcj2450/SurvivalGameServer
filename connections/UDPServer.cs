using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace SurvivalGameServer.connections
{
    internal class UDPServer : UdpServer
    {
        public UDPServer(IPAddress address, int port) : base(address, port) { }

        protected override void OnStarted()
        {
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            //Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size) + " = " + Id);
            ReceivedDataHandler.HandleData(new ReadOnlySpan<byte>(buffer, 0, (int)size), Id, endpoint);
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            //ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, $"UDP server caught an error with code {error}");
        }
    }
}
