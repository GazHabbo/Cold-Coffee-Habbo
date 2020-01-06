using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Network.Game;
using MySql.Data.MySqlClient;
using Reflection.Hotel.Habbos;

namespace Reflection.Hotel.Messages
{
    public partial class HabboMessages
    {
        protected void CHECK_VERSION()
        {
            var key = Utils.HabboHexRC4.GeneratePublicKeyString();
            
            this.fuseResponse = new serverMessage("@A");
            this.fuseResponse.AddString(key);
            this.SendResponse();

            this.Session.Encrypted = true;
            this.Session.Key = key; 
        }

        protected void GDATE()
        {

        }

        protected void RegisterHandshake()
        {
            this.Packets["@E"] = CHECK_VERSION;
            this.Packets["@q"] = GDATE;
        }
    }
}
