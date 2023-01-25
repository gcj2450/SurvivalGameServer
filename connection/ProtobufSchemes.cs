using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer.connection
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
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(stream);
                }
            }
            catch (Exception ex)
            {
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, $"error deserializing packet from {endPoint}");
                Globals.Logger.Write(Serilog.Events.LogEventLevel.Error, ex.ToString());
            }

            T result = default;
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
    public struct MovementPacketFromClient
    {
        [ProtoMember(1)]
        public float Horizontal { get; set; }
        [ProtoMember(2)]
        public float Vertical { get; set; }
        [ProtoMember(3)]
        public bool isActionButtonOnePressed { get; set; }

        public MovementPacketFromClient(float horizontal, float vertical, bool isOnePressed)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            isActionButtonOnePressed = isOnePressed;
        }
    }

    [ProtoContract]
    public struct MovementPacketFromServer
    {
        public MovementPacketFromServer(float positionX, float positionY, float positionZ, float rotationX, float rotationY, float rotationZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;
        }

        [ProtoMember(1)]
        public float PositionX { get; set; }
        [ProtoMember(2)]
        public float PositionY { get; set; }
        [ProtoMember(3)]
        public float PositionZ { get; set; }
        [ProtoMember(4)]
        public float RotationX { get; set; }
        [ProtoMember(5)]
        public float RotationY { get; set; }
        [ProtoMember(6)]
        public float RotationZ { get; set; }

    }
}
