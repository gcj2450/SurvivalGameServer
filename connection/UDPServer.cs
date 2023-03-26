using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace SurvivalGameServer
{
    internal class UDPServer : UdpServer
    {
        private ReceivedDataHandler receivedDataHandler;
        public UDPServer(IPAddress address, int port) : base(address, port) 
        {
            receivedDataHandler = new ReceivedDataHandler();
        }

        protected override void OnStarted()
        {
            ReceiveAsync();
        }
        
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            //Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size) + " = " + Id);
            //Console.WriteLine(endpoint.ToString());
            receivedDataHandler.HandleData(new ReadOnlySpan<byte>(buffer, 0, (int)size), Id, endpoint);
            ReceiveAsync();
        }
        
        protected override void OnSent(EndPoint endpoint, long sent)
        {
            //Console.WriteLine(  sent);
            //ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, $"UDP server caught an error with code {error}");
        }
    }
}
