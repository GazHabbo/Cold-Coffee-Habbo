using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflection.Hotel.Messages
{
    public partial class HabboMessages
    {
        protected void INFO_RETRIEVE()
        {
        }

        protected void GET_BALANCE()
        {
        }

        protected void RegisterUser()
        {
            Packets["@G"] = new Packet(INFO_RETRIEVE);
            Packets["@H"] = new Packet(GET_BALANCE);
        }
    }
}
