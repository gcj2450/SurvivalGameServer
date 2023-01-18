using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    class TCPSession : TcpSession
    {
        public TCPSession(TcpServer server) : base(server) { }

        protected override void OnConnected()
        {
            Program.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP session with Id:{Id} and Endpoint:{Socket.RemoteEndPoint} connected!");
            

            // Send invite message
            //string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
            //SendAsync(message);
        }

        protected override void OnDisconnected()
        {
            Program.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            //string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            //Console.WriteLine(string.Join('=', Encoding.UTF8.GetBytes(message)));
            //Console.WriteLine("Incoming: " + message);

            
            ReceivedDataHandler.HandleData(new ReadOnlySpan<byte>(buffer, 0, (int)size), Id, Socket.RemoteEndPoint);
            
        }

        protected override void OnError(SocketError error)
        {
            Program.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP session caught an error with code {error}");
        }
    }

}
