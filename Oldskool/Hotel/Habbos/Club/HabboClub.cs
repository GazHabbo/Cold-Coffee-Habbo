using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflection.Hotel.Habbos.Club
{
    public class HabboClub
    {
        public DateTime Started;
        public DateTime Expire;
        
        public int CheckDaysLeft
        {
            get
            {
                return Convert.ToInt32((Expire - Started).TotalDays);
            }
        }

        public int CheckMonthsLeft
        {
            get
            {
                return (CheckDaysLeft / 31);
            }
        }

        public bool HasClub
        {
            get;
            set;
        }
    }
}
