using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace SurvivalGameServer
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
            Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size) + " = " + Id);

            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            //ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"UDP server caught an error with code {error}");
        }
    }
}
