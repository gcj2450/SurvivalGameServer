using ProtoBuf;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SurvivalGameServer.connections;

namespace SurvivalGameServer
{
    internal class ReceivedDataHandler
    {
        private static Dictionary<int, Encryption> TemporaryEncryptionConnection = new Dictionary<int, Encryption>();
        private static Dictionary<byte[], byte[]> Fortest = new Dictionary<byte[], byte[]>(new ByteArrayComparer());

        public static void HandleData(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
        {
            //Console.WriteLine(string.Join('=', data.ToArray()));
            byte[] networkID = Array.Empty<byte>();
            Globals.PacketCode packetCode = Globals.PacketCode.None;
            byte[] packet = Array.Empty<byte>();
            if (data.Length > 0 && data.Length > 5)
            {
                networkID = data.Slice(0, 4).ToArray();
                packetCode = (Globals.PacketCode)data.Slice(4, 1).ToArray()[0];
                packet = data.Slice(5, data.Length - 5).ToArray();
            }

            //Movement packet, type 1
            if (data.Length > 0 && Fortest.ContainsKey(networkID) && packetCode == Globals.PacketCode.Move)
            {                
                Encryption.Decode(ref packet, Fortest[networkID]);

                MovementPacket movementPacket = ProtobufSchemes.DeserializeProtoBuf<MovementPacket>(packet, endpoint);
                Console.WriteLine(movementPacket.Horizontal + " = " + movementPacket.Vertical);
            }

            //00
            if (data.Length > 0 && data[0] == 0 && data[1] == 0) 
            {
                HandleRequestForSecretKey(data, id, endpoint);
                return;
            }

            //01
            if (data.Length > 0 && data[0] == 0 && data[1] == 1)
            {
                HandleSecretKeyAndNetworkID(data, id, endpoint);
                return;
            }
        }

        // 00 - get TCP request for public key
        private static void HandleRequestForSecretKey(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
        {
            try
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"received secret key exchange request from {endpoint}");

                Encryption encryption = new Encryption();
                int key = encryption.GetHashCode();
                RSAExchange exchange = new RSAExchange(key, encryption.publicKeyInString);

                byte[] packet = Array.Empty<byte>();
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, exchange);
                    packet = stream.ToArray();
                }

                Servers.SendTCP(packet, id);

                if (!TemporaryEncryptionConnection.ContainsKey(key))
                {
                    TemporaryEncryptionConnection.Add(key, encryption);
                    KillAfterTimeoutTemporaryEncryptionConnection(key);
                }
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error receiving 00 code from {endpoint}");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }
        }

        // 01 - Get public key, generate secret key and NetworkID
        private static void HandleSecretKeyAndNetworkID(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
        {
            try
            {
                RSAExchange exchange = new RSAExchange();
                Encryption encryption = null;

                using (Stream stream = new MemoryStream(Encryption.TakeSomeToArrayFromNumber(data, 3)))
                {
                    exchange = Serializer.Deserialize<RSAExchange>(stream);
                }

                if (TemporaryEncryptionConnection.ContainsKey(exchange.TemporaryKeyCode))
                {
                    encryption = TemporaryEncryptionConnection[exchange.TemporaryKeyCode];
                    Fortest.Add(
                        BitConverter.GetBytes(exchange.TemporaryKeyCode),
                        encryption.GetSecretKey(exchange.PublicKey));
                    encryption.Dispose();

                    TemporaryEncryptionConnection.Remove(exchange.TemporaryKeyCode);

                    Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"successfully exchanged secret key and network ID with {endpoint}");
                }
                else
                {
                    Globals.Logger.Write(Serilog.Events.LogEventLevel.Warning,
                        $"no such TemporaryEncryptionConnection from {endpoint}");
                }
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error receiving 01 code from {endpoint}");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }
        }

        private static async void KillAfterTimeoutTemporaryEncryptionConnection(int key)
        {
            if (!TemporaryEncryptionConnection.ContainsKey(key))
            {
                return;
            }

            await Task.Delay(60000);
            TemporaryEncryptionConnection.Remove(key);
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