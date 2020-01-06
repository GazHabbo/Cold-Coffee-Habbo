using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Hotel.Habbos.Club;
using MySql.Data.MySqlClient;

namespace Reflection.Hotel.Habbos
{
    public class Habbo
    {
        internal int ID;
        internal string Username;
        internal string Password;
        internal string Email;
        internal string DOB;
        internal int Coins;
        internal int Tickets;
        internal int Film;
        internal int Rank;
        internal string Figure;
        internal string Motto;
        internal string Gender;
        internal string ConsoleMotto;
        internal int ExpiredClubDays;

        public HabboClub Club
        {
            get;
            set;
        }

        internal void LoadRest()
        {
            var row = Program.SqlManager.GetRow("SELECT * FROM habbos_club WHERE userid = @uid;", new MySqlParameter("uid", this.ID));

            this.Club = new HabboClub();

            if (row != null)
            {
                this.Club.HasClub = true;
                this.Club.Started = (DateTime)row["started"];
                this.Club.Expire = (DateTime)row["expire"];
            }
            else
            {
                this.Club.HasClub = false;
            }
        }
    }
}
