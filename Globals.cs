using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class Globals
    {
        public const int TCP_PORT = 3000;
        public const int UDP_PORT = 3001;

        public static Stopwatch GlobalTimer;

        public static ILogger Logger;

        public enum PacketCode
        {
            None = 0,
            Move = 1, //joystick press data
            Abut = 2  //action buttons pressed

        }

        public static void InitServerGlobals()
        {
            GlobalTimer = new Stopwatch();
            GlobalTimer.Start();

            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("GameServer.log")
                .CreateLogger();
        }
    }
}
