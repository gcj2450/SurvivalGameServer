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

            //TEMPORARY===============JUST FOR TEST===============
            PlayerCharacter player = new PlayerCharacter(
                Globals.GlobalTimer.ElapsedMilliseconds,
                1,
                "Main Player",
                100,
                100,
                1,
                0,
                new Vector3(0,0,0),
                new Vector3(0, 0, 0),
                1
                );
            player.SetTicketConnection(12345678);
            player.SetNewPosition(new Vector3(0, 0, 0));
            Globals.ActivePlayersByTicketID.Add(12345678, player);

            PlayerCharacter player1 = new PlayerCharacter(
                Globals.GlobalTimer.ElapsedMilliseconds,
                1,
                "Main Player2",
                100,
                100,
                1,
                0,
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                1
                );
            player1.SetTicketConnection(12345679);
            player1.SetNewPosition(new Vector3(0, 0, 0));
            Globals.ActivePlayersByTicketID.Add(12345679, player1);
            //====================================================

            //init game core
            GameServer gameServer = GameServer.GetGameServerInstance();
            
            Console.ReadLine();
            connections.StopServers();            
        }
    }
}