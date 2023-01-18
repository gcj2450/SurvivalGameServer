using NetCoreServer;
using Serilog;
using System;
using System.Net;
using System.Numerics;

namespace SurvivalGameServer
{
    internal class Program
    {
        public static ILogger Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("GameServer.log")
                .CreateLogger();

        static void Main(string[] args)
        {            
            Servers.StartServers();
                       
            Console.ReadLine();
            Servers.StopServers();            
        }
    }
}