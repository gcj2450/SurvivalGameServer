using ProtoBuf;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SurvivalGameServer
{
    internal class ReceivedDataHandler
    {
        private static Dictionary<int, Encryption> TemporaryEncryptionConnection = new Dictionary<int, Encryption>();
        private static Dictionary<byte[], byte[]> Fortest = new Dictionary<byte[], byte[]>(new ByteArrayComparer());
        public static void HandleData(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
        {            
            if (data.Length > 0 && data.Length > 4 && Fortest.ContainsKey(data.Slice(0,4).ToArray()))
            {                
                byte[] dataTest = data.Slice(4, data.Length - 4).ToArray();
                Encryption.Decode(ref dataTest, Fortest[data.Slice(0, 4).ToArray()]);
                Console.WriteLine(string.Join('=', dataTest));
            }

            //00
            if (data.Length > 0 && data[0] == 0 && data[1] == 0) 
            {
                try
                {
                    Program.Logger.Write(Serilog.Events.LogEventLevel.Information,
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
                    Program.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error receiving 00 code from {endpoint}");
                    Program.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
                }
                return;
            }

            //01
            if (data.Length > 0 && data[0] == 0 && data[1] == 1)
            {
                try
                {                    
                    RSAExchange exchange = new RSAExchange();
                    Encryption encryption = null;

                    using (Stream stream = new MemoryStream(Encryption.TakeSomeToArrayFromNumber(data, 2)))
                    {
                        exchange = Serializer.Deserialize<RSAExchange>(stream);
                    }

                    if (TemporaryEncryptionConnection.ContainsKey(exchange.TemporaryKeyCode))
                    {
                        encryption = TemporaryEncryptionConnection[exchange.TemporaryKeyCode];
                        byte[] secretKey = encryption.GetSecretKey(exchange.PublicKey);
                        byte[] ClientNetworkID = BitConverter.GetBytes(exchange.TemporaryKeyCode);
                        Fortest.Add(ClientNetworkID, secretKey);
                        encryption.Dispose();
                        TemporaryEncryptionConnection.Remove(exchange.TemporaryKeyCode);
                        Program.Logger.Write(Serilog.Events.LogEventLevel.Information, 
                            $"successfully exchanged secret key and network ID with {endpoint}");
                    }
                    else
                    {
                        Program.Logger.Write(Serilog.Events.LogEventLevel.Warning, 
                            $"no such TemporaryEncryptionConnection from {endpoint}");
                    }
                }
                catch (Exception ex)
                {
                    Program.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error receiving 01 code from {endpoint}");
                    Program.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
                }
                return;
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

//packet codes:
// 00 - get TCP request for public key
// 01 - Get public key and generate secret key


