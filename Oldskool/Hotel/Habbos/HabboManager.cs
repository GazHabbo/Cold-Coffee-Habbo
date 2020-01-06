using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Reflection.Hotel.Habbos
{
    public class HabboManager
    {
        internal static Habbo GenerateHabbo(int ID)
        {
            return GenerateHabbo(Program.SqlManager.GetRow("SELECT * FROM habbos WHERE id = @id LIMIT 1;", new MySql.Data.MySqlClient.MySqlParameter("id", ID)));
        }

        internal static Habbo GenerateHabbo(DataRow row)
        {
            return new Habbo()
            {
                ID = (int)row["id"],
                Coins = (int)row["coins"],
                ConsoleMotto = (string)row["consolemotto"],
                DOB = (string)row["dob"],
                Email = (string)row["email"],
                Figure = (string)row["figure"],
                Film = (int)row["film"],
                Gender = (string)row["gender"],
                Motto = (string)row["motto"],
                Password = (string)row["password"],
                Rank = (int)row["rank"],
                Tickets = (int)row["tickets"],
                Username = (string)row["username"],
                ExpiredClubDays = (int)row["expired_clubdays"],
                Club = new Club.HabboClub()
            };
        }
    }
}
