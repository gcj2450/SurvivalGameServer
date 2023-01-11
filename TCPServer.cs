using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace survival_game_server
{
    class TCPServer : TcpServer
    {
        public TCPServer(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new TCPSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP server caught an error with code {error}");
        }
    }
}
