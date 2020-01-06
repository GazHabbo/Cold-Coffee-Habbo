using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflection.Hotel.Messages
{
    public partial class HabboMessages
    {
        protected void CATALOG_INDEX()
        {
        }

        protected void CATALOG_PAGE()
        {
        }

        protected void RegisterCatalogue()
        {
            Packets["Ae"] = new Packet(CATALOG_INDEX);
            Packets["Af"] = new Packet(CATALOG_PAGE);
        }
    }
}
