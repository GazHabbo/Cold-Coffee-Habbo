using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Reflection.Hotel.Messages;
using Reflection.Utils;

namespace Reflection.Network.Game
{
    public class GameConnection
    {
        public int Id { get; set; }
        public GameListener Manager { get; private set; }
        public Socket Socket { get; set; }
        public HabboMessages HabboMessages { get; private set; }

        public GameConnection(int id, GameListener manager)
        {
            this.Id = id;
            this.Manager = manager;

            this.HabboMessages = new HabboMessages(this);
        }

        internal string IPAddress()
        {
            return this.Socket.RemoteEndPoint.ToString().Split(':')[0];
        }

        internal void OnReceiveData(char[] data)
        {
            //if (Encrypted)
            //{
            //    data = new Utils.HabboHexRC4(this.Key).Decipher(new string(data)).ToCharArray();
            //}

            int pos = 0;

            while (pos < data.Length)
            {
                int length = HabboEncoding.decodeB64(new string(new char[] { data[pos++], data[pos++], data[pos++] }));
                string header = new string(new char[] { data[pos++], data[pos++] });

                char[] content = new char[length - 2];

                for (int i = 0; i < content.Length && pos < data.Length; i++)
                {
                    content[i] = data[pos++];
                }

                Program.Writer.PrintLine("RECV", string.Concat(@"Client ", Id, @" received [ID:", header, "]: ", new string(content)));

                HabboMessages.TryHandle(header, new clientMessage(content), this);
            }
        }

        private bool DisconnectedCalled = false;
        private Hotel.Habbos.Habbo mHabbo;

        internal void Disconnect()
        {
            if (!this.DisconnectedCalled)
            {
                this.DisconnectedCalled = true;
                this.Socket.Disconnect(true);
            }
        }

        internal void SendData(byte[] data)
        {
            this.Manager.SendData(this.Socket, data);
        }

        internal void SendData(string Data)
        {
            SendData(Encoding.UTF8.GetBytes(Data));
        }

        internal void SendMessage(serverMessage message)
        {
            var data = message.ToString();
            Program.Writer.PrintLine("SEND", string.Concat("Client ", Id, " sent: ", data).Replace(((char)13).ToString(), "{{13}}"));
            SendData(data);
        }

        internal void SetHabbo(Hotel.Habbos.Habbo habbo)
        {
            this.mHabbo = habbo; 
        }

        internal Hotel.Habbos.Habbo GetHabbo()
        {
            return this.mHabbo;
        }

        public Hotel.Flats.FlatInstance Flat { get; set; }

        public bool Encrypted { get; set; }

        public string Key { get; set; }
    }
}
