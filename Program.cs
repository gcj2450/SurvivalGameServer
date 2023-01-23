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

            //TEMPORARY=============================
            PlayerCharacter player = new PlayerCharacter(12345678);
            Globals.ActivePlayersByTicketID.Add(12345678, player);
            //======================================

            Console.ReadLine();
            connections.StopServers();            
        }
    }
}