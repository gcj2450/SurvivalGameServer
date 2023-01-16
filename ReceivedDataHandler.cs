using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class ReceivedDataHandler
    {
        public static void HandleData(ReadOnlySpan<byte> data)
        {
            //00
            if (data.Length > 0 && data[0] == 0 && data[1] == 0) 
            {

            }
        }
    }
}

//packet codes:
// 00 - request for public key
// 01 - receive key back, secret key ready


