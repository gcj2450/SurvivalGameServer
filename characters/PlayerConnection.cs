using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class PlayerConnection
    {
        public readonly int TicketID;
        public byte[] NetworkID { get; private set; }
        public byte[] SecretKey { get; private set; }
        public PlayerCharacter PlayerCharacter { get; private set; }

        public PlayerConnection(int ticket, PlayerCharacter playerCharacter)
        {
            TicketID = ticket;
            PlayerCharacter = playerCharacter;
        }

        public void SetSecretKey(byte[] secretKey)
        {
            SecretKey = secretKey;
        }
    }
}
