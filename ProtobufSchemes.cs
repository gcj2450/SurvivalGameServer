using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class ProtobufSchemes
    {

    }

    [ProtoContract]
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
}
