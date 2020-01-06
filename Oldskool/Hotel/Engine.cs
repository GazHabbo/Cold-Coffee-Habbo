using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Hotel.Roles;
using Reflection.Hotel.Flats;

namespace Reflection.Hotel
{
    public class Engine
    {
        public RoleManager RoleManager
        {
            get;
            private set;
        }

        public FlatManager FlatManager
        {
            get;
            private set;
        }

        public Engine()
        {
            this.RoleManager = new RoleManager();
            this.FlatManager = new FlatManager();
        }
    }
}
