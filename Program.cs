using NetCoreServer;
using System;
using System.Net;
using System.Numerics;

namespace SurvivalGameServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Servers.StartServers();            
            Console.ReadLine();
            Servers.StopServers();            
        }
    }
}