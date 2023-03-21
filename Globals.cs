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
        public const int TICKi = 60;
        public const float TICKf = 0.06f;

        public const int TCP_PORT = 3000;
        public const int UDP_PORT = 3001;
        public static Dictionary<int, PlayerCharacter> ActivePlayersByTicketID;
        public static Dictionary<byte[], PlayerConnection> ActivePlayersByNetworID;

        public static Stopwatch GlobalTimer;

        public static ILogger Logger;

        public enum PacketCode
        {
            None = 0,
            MoveFromClient = 1, //joystick press data
            MoveFromServer = 2,
            GetClientUDPEndpoint = 3,
            InitialPlayerData = 4,
            OperativePlayerData = 5
        }

        public static void InitServerGlobals()
        {
            ActivePlayersByTicketID = new Dictionary<int, PlayerCharacter>();
            ActivePlayersByNetworID = new Dictionary<byte[], PlayerConnection>(new ByteArrayComparer());

            GlobalTimer = new Stopwatch();
            GlobalTimer.Start();

            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("GameServer.log")
                .CreateLogger();
        }
    }

    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null)
            {
                return left == right;
            }
            return left.SequenceEqual(right);
        }
        public int GetHashCode(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return key.Sum(b => b);
        }
    }
}
