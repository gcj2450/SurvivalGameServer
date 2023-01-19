using NetCoreServer;
using Serilog;
using SurvivalGameServer.connections;
using System;
using System.Net;
using System.Numerics;

namespace SurvivalGameServer
{
    internal class Program
    {
        

        static void Main(string[] args)
        {          
            Globals.InitServerGlobals();

            Servers.StartServers();
                       
            Console.ReadLine();
            Servers.StopServers();            
        }
    }
}