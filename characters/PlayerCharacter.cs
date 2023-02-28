using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class PlayerCharacter : Characters
    {
        public PlayerConnection Connection { get; private set; }
        public PlayerCharacter() 
        {
            
        }

        public void SetTicketConnection(int ticket)
        {
            Connection = new PlayerConnection(ticket, this);
        }
    }
}
