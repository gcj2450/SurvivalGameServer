using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class PlayerCharacter : Characters
    {
        public PlayerConnection Connection { get; private set; }
        public PlayerCharacter(long objectId, ushort appearanceId, string name, ushort currentHealth, 
            ushort maxHealth, ushort level, ushort armor, Vector3 position, Vector3 rotation, float speed) 

            : base(objectId, appearanceId, name, currentHealth, maxHealth, level, armor, position, rotation, speed)

        {
            
        }

        public void SetTicketConnection(int ticket)
        {
            Connection = new PlayerConnection(ticket, this);
        }
    }
}
