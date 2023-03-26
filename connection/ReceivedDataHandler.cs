using ProtoBuf;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;

namespace SurvivalGameServer
{
    internal class ReceivedDataHandler
    {
        private Dictionary<int, Encryption> TemporaryEncryptionConnection = new Dictionary<int, Encryption>();

        public void HandleData(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
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

            //packets with codes
            if (data.Length > 0 && Globals.ActivePlayersByNetworID.ContainsKey(networkID))
            {
                switch (packetCode)
                {
                    case Globals.PacketCode.MoveFromClient:
                        
                        Encryption.Decode(ref packet, Globals.ActivePlayersByNetworID[networkID].SecretKey);

                        Globals.ActivePlayersByNetworID[networkID]
                            .AddMovementPacket(ProtobufSchemes.DeserializeProtoBuf<MovementPacketFromClient>(packet, endpoint));
                        break;

                    case Globals.PacketCode.GetClientUDPEndpoint:
                        Globals.ActivePlayersByNetworID[networkID].SetEndpointForUDP(endpoint);
                        Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"received UDP endpoint from {endpoint}");
                        break;
                }


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
        private void HandleRequestForSecretKey(Guid id, EndPoint endpoint)
        {
            try
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"received secret key exchange request from {endpoint}");

                Encryption encryption = new Encryption();
                int key = encryption.GetHashCode();
                RSAExchange exchange = new RSAExchange(key, encryption.publicKeyInString, 0);

                byte[] packet = ProtobufSchemes.SerializeProtoBuf(exchange);
                Servers.GetInstance().SendTCP(packet, id);

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
        private void HandleSecretKeyAndNetworkID(ReadOnlySpan<byte> data, Guid id, EndPoint endpoint)
        {
            try
            {
                RSAExchange exchange = ProtobufSchemes.DeserializeProtoBuf<RSAExchange>(Encryption.TakeSomeToArrayFromNumber(data, 3), endpoint);

                if (TemporaryEncryptionConnection.ContainsKey(exchange.TemporaryKeyCode))
                {
                    Encryption encryption = TemporaryEncryptionConnection[exchange.TemporaryKeyCode];
                    int ticketID = exchange.TicketID;

                    if (!Globals.ActivePlayersByTicketID.ContainsKey(ticketID))
                    {
                        Globals.Logger.Write(Serilog.Events.LogEventLevel.Error,
                        $"no such player on server from {endpoint} with ticket {ticketID}");
                        encryption.Dispose();
                        return;
                    }

                    PlayerConnection playerConnection = Globals.ActivePlayersByTicketID[ticketID].Connection;
                    playerConnection.SetConnectionData(
                        encryption.GetSecretKey(exchange.PublicKey), id);


                    Globals.ActivePlayersByNetworID.Add(
                        BitConverter.GetBytes(exchange.TemporaryKeyCode),
                        playerConnection);


                    TemporaryEncryptionConnection.Remove(exchange.TemporaryKeyCode);
                    encryption.Dispose();

                    GameServer.GetGameServerInstance().AddPlayerCharacter(playerConnection.CurrentPlayerCharacter);
                    playerConnection.SendMainPlayerData();


                    Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"successfully exchanged secret key and network ID with {endpoint}");
                    Globals.Logger.Write(Serilog.Events.LogEventLevel.Information,
                        $"player from {endpoint} with ticket {ticketID} entered the world");
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

        private async void KillAfterTimeoutTemporaryEncryptionConnection(int key)
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