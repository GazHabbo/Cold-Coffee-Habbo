using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Kernel.IO;
using Reflection.Network.Game;
using Reflection.Hotel.Messages;
using Reflection.Kernel.Database;
using Reflection.Hotel;

namespace Reflection
{
    class Program
    {
        public static ConsoleWriter Writer
        {
            get
            {
                return new ConsoleWriter();
            }
        }

        public static GameListener Gamelistener
        {
            get;
            private set;
        }

        public static DatabaseManager SqlManager
        {
            get;
            private set;
        }

        public static Engine Game
        {
            get;
            private set;
        }

        public static DateTime DateTimeByTimestamp(double timestamp)
        {
            var time = new DateTime(1970, 1, 1, 0, 0, 0);
            return time.AddSeconds(timestamp);
        }

        public static double UnixTimestamp
        {
            get
            {
                return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            }
        }

        static void Main(string[] args)
        {
            Writer.SetTitle("Reflection - V7 Emulator coded by Tha");

            SqlManager = new DatabaseManager();
            SqlManager.Open("localhost", "root", "HabboDeveloper1", "reflection");

            Game = new Engine();

            Gamelistener = new GameListener("127.0.0.1", 3090, 10);


            do
            {
                Console.In.ReadLine();
            }
            while (true);
        }
    }
}
