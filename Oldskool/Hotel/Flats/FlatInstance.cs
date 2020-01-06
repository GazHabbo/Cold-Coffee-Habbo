using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace Reflection.Hotel.Flats
{
    public class FlatInstance
    {
        public Dictionary<int, FlatUser> FlatUsers
        {
            get;
            private set;
        }

        public FlatInfo Info
        {
            get;
            private set;
        }

        public FlatInstance(int RoomID)
        {
            this.FlatUsers = new Dictionary<int, FlatUser>();

            DataRow row = null;

            if (!Program.SqlManager.HasRows("SELECT * FROM flats WHERE id = @id;", out row, new MySqlParameter("id", RoomID)))
            {
                return;
            }
            else
            {
                this.Info = new FlatInfo()
                {
                    ID = (int)row["id"],
                    Name = (string)row["name"],
                    Owner = (string)row["owner"],
                    State = (int)row["state"],
                    MaxUsers = (int)row["maxusers"],
                    Description = (string)row["description"],
                    Category = (int)row["category"],
                    ShowOwner = (bool)row["showowner"],
                    Wallpaper = (string)row["wallpaper"],
                    Floor = (string)row["floor"],
                    Model = (string)row["model"]
                };
            }
        }

        internal void AddUser(Network.Game.GameConnection connection)
        {
            
        }
    }
}
