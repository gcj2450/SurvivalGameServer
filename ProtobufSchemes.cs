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
                    Serializer.Serialize<T>(stream, data);
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
            
            T result = default(T);
            return result;
        }
    }

    [ProtoContract] //for exchange of RSA secret key between client-server
    public struct RSAExchange
    {
        [ProtoMember(1)]
        public int TemporaryKeyCode {get;set;}
        [ProtoMember(2)]
        public string PublicKey {get;set;}

        public RSAExchange(int TemporaryKeyCode, string PublicKey)
        {
            this.TemporaryKeyCode = TemporaryKeyCode;
            this.PublicKey = PublicKey;
        }
    }

    [ProtoContract]//data from client: what controls are pressed
    public struct MovementPacket
    {
        [ProtoMember(1)]
        public float Horizontal { get; set; }
        [ProtoMember(2)]
        public float Vertical { get; set; }

        public MovementPacket(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}
