using NetCoreServer;
using Serilog;
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

            Servers connections = Servers.GetInstance();


            Console.ReadLine();
            connections.StopServers();            
        }
    }
}