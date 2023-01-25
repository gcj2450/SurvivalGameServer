using NetCoreServer;
using SurvivalGameServer.connection;
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
        private ReceivedDataHandler receivedDataHandler;

        public TCPSession(TcpServer server) : base(server) 
        {
            receivedDataHandler = new ReceivedDataHandler();
        }
        
        protected override void OnConnected()
        {
            Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP session with Id:{Id} and Endpoint:{Socket.RemoteEndPoint} connected!");

            // Send invite message
            //string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
            //SendAsync(message);
        }

        protected override void OnDisconnected()
        {
            Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            //string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            //Console.WriteLine(string.Join('=', Encoding.UTF8.GetBytes(message)));
            //Console.WriteLine("Incoming: " + message);
            //Console.WriteLine(size);
            //Console.WriteLine(string.Join('=', new ReadOnlySpan<byte>(buffer, (int)offset, (int)size).ToArray()));

            receivedDataHandler.HandleData(new ReadOnlySpan<byte>(buffer, 0, (int)size), Id, Socket.RemoteEndPoint);
        }

        protected override void OnError(SocketError error)
        {
            Globals.Logger.Write(Serilog.Events.LogEventLevel.Information, $"TCP session caught an error with code {error}");
        }
    }

}
