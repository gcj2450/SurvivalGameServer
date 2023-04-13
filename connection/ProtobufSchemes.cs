using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class ProtobufSchemes
    {
        public static byte[] SerializeProtoBuf<T>(T data)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, data);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error serializing packet to ... ");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }

            return Array.Empty<byte>();
        }

        public static T DeserializeProtoBuf<T>(byte[] data, EndPoint endPoint)
        {
            T result = default;

            try
            {
                using (var stream = new MemoryStream(data))
                {
                    

                    result = Serializer.Deserialize<T>(stream);
                }
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error deserializing packet from {endPoint}");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }

            
            return result;
        }
    }


    [ProtoContract] //for exchange of RSA secret key between client-server    
    public struct RSAExchange
    {
        [ProtoMember(1)]
        public int TemporaryKeyCode { get; set; }
        [ProtoMember(2)]
        public string PublicKey { get; set; }
        [ProtoMember(3)]
        public int TicketID { get; set; }

        public RSAExchange(int TemporaryKeyCode, string PublicKey, int ticket)
        {
            this.TemporaryKeyCode = TemporaryKeyCode;
            this.PublicKey = PublicKey;
            TicketID = ticket;
        }
    }

    [ProtoContract]//data from client: what controls are pressed
    public class MovementPacketFromClient
    {
        [ProtoMember(1)]
        public int PacketId { get; set; }
        [ProtoMember(2)]
        public float Horizontal { get; set; }
        [ProtoMember(3)]
        public float Vertical { get; set; }
        [ProtoMember(4)]
        public bool isActionButtonOnePressed { get; set; }

        public MovementPacketFromClient(float horizontal, float vertical, bool isOnePressed)
        {
            PacketId = 0;
            Horizontal = horizontal;
            Vertical = vertical;
            isActionButtonOnePressed = isOnePressed;
        }

        public MovementPacketFromClient()
        {
            PacketId = 0;
            Horizontal = 0;
            Vertical = 0;
            isActionButtonOnePressed = false;
        }

        public void Clear()
        {
            PacketId = 0;
            Horizontal = 0;
            Vertical = 0;
            isActionButtonOnePressed = false;
        }
    }

    [ProtoContract]
    public class MovementPacketFromServer
    {
        public MovementPacketFromServer(long objId)
        {
            ObjectId = objId;
            PacketOrder = 0;
            PositionX = 0;
            PositionY = 0;
            PositionZ = 0;
            RotationX = 0;
            RotationY = 0;
            RotationZ = 0;
        }

        public void Update(long objectId, uint packetId, float positionX, float positionY, float positionZ, float rotationY)
        {
            
            ObjectId = objectId;
            PacketOrder = packetId;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
            RotationX = 0;
            RotationY = rotationY;
            RotationZ = 0;

        }

        [ProtoMember(1)]
        public uint PacketOrder { get; set; }
        [ProtoMember(2)]
        public long ObjectId { get; set; }
        [ProtoMember(3)]
        public float PositionX { get; set; }
        [ProtoMember(4)]
        public float PositionY { get; set; }
        [ProtoMember(5)]
        public float PositionZ { get; set; }
        [ProtoMember(6)]
        public float RotationX { get; set; }
        [ProtoMember(7)]
        public float RotationY { get; set; }
        [ProtoMember(8)]
        public float RotationZ { get; set; }

    }

    [ProtoContract]
    public struct ListOfMovementPacketsFromServer
    {
        public ListOfMovementPacketsFromServer(int i)
        {
            ListOfPackets = new Dictionary<long, MovementPacketFromServer>();            
        }

        public void AddOrUpdate(MovementPacketFromServer packet)
        {            
            if (!ListOfPackets.ContainsKey(packet.ObjectId))
            {
                ListOfPackets.Add(packet.ObjectId, packet);
            }
            else
            {
                ListOfPackets[packet.ObjectId] = packet;
            }
        }

        public void Clear()
        {
            ListOfPackets.Clear();
        }


        [ProtoMember(1)]        
        public Dictionary<long, MovementPacketFromServer> ListOfPackets;
    }

    [ProtoContract]
    public struct PlayerDataInitial
    {
        public PlayerDataInitial(PlayerCharacter player)
        {
            ObjectId = player.ObjectId;
            AppearanceId = player.AppearanceId;
            Name = player.Name;
            PositionX = player.Position.X;
            PositionY = player.Position.Y;
            PositionZ = player.Position.Z;
            RotationX = player.Rotation.X;
            RotationY = player.Rotation.Y;
            RotationZ = player.Rotation.Z;
        }

        [ProtoMember(1)]
        public long ObjectId { get; set; }
        [ProtoMember(2)]
        public ushort AppearanceId { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public float PositionX { get; set; }
        [ProtoMember(5)]
        public float PositionY { get; set; }
        [ProtoMember(6)]
        public float PositionZ { get; set; }
        [ProtoMember(7)]
        public float RotationX { get; set; }
        [ProtoMember(8)]
        public float RotationY { get; set; }
        [ProtoMember(9)]
        public float RotationZ { get; set; }
    }
}
