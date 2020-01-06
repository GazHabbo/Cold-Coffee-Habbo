using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Network.Game;

namespace Reflection.Hotel.Flats
{
    public class FlatUser
    {
        internal GameConnection Session;
        internal int VirtualID;
        internal int X;
        internal int Y;
        internal double Z;
        internal int Rotation;
        internal Dictionary<string, string> Statusses;

        public FlatUser(GameConnection Connection)
        {
            this.Session = Connection;
        }
    }
}
