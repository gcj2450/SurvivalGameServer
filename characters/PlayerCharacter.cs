using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class PlayerCharacter : BaseCharacter
    {
        public PlayerConnection Connection { get; private set; }
        public PlayerCharacter(int ticket) 
        {
            Connection = new PlayerConnection(ticket, this);
        }
    }
}
