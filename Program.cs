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
            
            //init servers
            Servers connections = Servers.GetInstance();

            //TEMPORARY=============================
            PlayerCharacter player = new PlayerCharacter(12345678);
            player.SetNewPosition(new Vector3(5, 0, 5));
            Globals.ActivePlayersByTicketID.Add(12345678, player);
            //======================================

            //init game core
            GameServer gameServer = GameServer.GetGameServerInstance();
            
            Console.ReadLine();
            connections.StopServers();            
        }
    }
}