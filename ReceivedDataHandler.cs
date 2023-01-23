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
        //private static Dictionary<byte[], byte[]> Fortest = new Dictionary<byte[], byte[]>(new ByteArrayComparer());
        private static Servers connections = Servers.GetInstance();

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
            if (data.Length > 0 && Globals.ActivePlayersByNetworID.ContainsKey(networkID) && packetCode == Globals.PacketCode.Move)
            {                
                Encryption.Decode(ref packet, Globals.ActivePlayersByNetworID[networkID].SecretKey);
                MovementPacket movementPacket = ProtobufSchemes.DeserializeProtoBuf<MovementPacket>(packet, endpoint);
                Console.WriteLine(movementPacket.Horizontal + " = " + movementPacket.Vertical);
            }

            //01
            if (data.Length > 0 && data[0] == 0 && data[1] == 1) 
            {
                HandleRequestForSecretKey(id, endpoint);
                return;
            }

            //02
            if (data.Length > 0 && data[0] == 0 && data[1] == 2)
            {
                HandleSecretKeyAndNetworkID(data, id, endpoint);
                return;
            }
        }

        // 01 - get TCP request for public key
        private static void HandleRequestForSecretKey(Guid id, EndPoint endpoint)
        {
            try
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"received secret key exchange request from {endpoint}");

                Encryption encryption = new Encryption();
                int key = encryption.GetHashCode();
                RSAExchange exchange = new RSAExchange(key, encryption.publicKeyInString, 0);

                byte[] packet = ProtobufSchemes.SerializeProtoBuf(exchange);
                connections.SendTCP(packet, id);

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

        // 02 - Get public key, generate secret key and NetworkID
        private static void HandleSecretKeyAndNetworkID(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
        {
            try
            {
                RSAExchange exchange = ProtobufSchemes.DeserializeProtoBuf<RSAExchange>(Encryption.TakeSomeToArrayFromNumber(data, 3), endpoint);
                
                if (TemporaryEncryptionConnection.ContainsKey(exchange.TemporaryKeyCode))
                {
                    Encryption encryption = TemporaryEncryptionConnection[exchange.TemporaryKeyCode];
                    
                    /*
                    Fortest.Add(
                        BitConverter.GetBytes(exchange.TemporaryKeyCode),
                        encryption.GetSecretKey(exchange.PublicKey));
                    */
                    
                    Globals.ActivePlayersByNetworID.Add(
                        BitConverter.GetBytes(exchange.TemporaryKeyCode), 
                        Globals.ActivePlayersByTicketID[exchange.TicketID].Connection);

                    Globals.ActivePlayersByTicketID[exchange.TicketID].Connection.SetSecretKey(encryption.GetSecretKey(exchange.PublicKey));

                    TemporaryEncryptionConnection.Remove(exchange.TemporaryKeyCode);
                    encryption.Dispose();

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

    
}