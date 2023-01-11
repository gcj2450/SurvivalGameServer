using survival_game_server;
using System;
using System.Net;
using System.Numerics;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TCP server port
            int port = 1111;
         
            // Create a new TCP chat server
            var server = new TCPServer(IPAddress.Any, port);

            // Start the server
            server.Start();

            Console.ReadLine();

            // Stop the server !!


            Console.Write("Server stopping...");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}