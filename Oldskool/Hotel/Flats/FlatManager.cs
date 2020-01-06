using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace Reflection.Hotel.Flats
{
    public class FlatManager
    {
        public Dictionary<int, FlatInstance> CachedFlats
        {
            get;
            private set;
        }

        public FlatManager()
        {
            this.CachedFlats = new Dictionary<int, FlatInstance>();
        }

        public FlatInstance GetFlat(int RoomID)
        {
            if (this.CachedFlats.ContainsKey(RoomID))
            {
                return this.CachedFlats[RoomID];
            }
            else
            {
                FlatInstance Flat = new FlatInstance(RoomID);
                this.CachedFlats.Add(Flat.Info.ID, Flat);
                return Flat;
            }
        }

        public List<FlatInstance> GetFlatsByOwner(string Owner)
        {
            List<FlatInstance> flats = new List<FlatInstance>();
            DataTable data = Program.SqlManager.GetTable("SELECT * FROM flats WHERE owner = @owner;", new MySqlParameter("owner", Owner));

            foreach (DataRow row in data.Rows)
            {
                flats.Add(GetFlat((int)row["id"]));
            }

            return flats;
        }

        // Thank you Josh
        public string GetRoomStateString(int state)
        {
            switch (state)
            {
                case 0:
                    return "open";
                case 1:
                    return "closed";
                case 2:
                    return "password";
                default:
                    return "unknown";
            }
        }
    }
}
