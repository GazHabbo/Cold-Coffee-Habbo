using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lightningbolt_Server.Collections;
using Reflection.Network.Game;
using MySql.Data.MySqlClient;
using Reflection.Hotel.Habbos;
using Reflection.Utils;

namespace Reflection.Hotel.Messages
{
    public partial class HabboMessages
    {
        protected delegate void Packet();
        protected GameConnection Session;
        protected Dict<string, Packet> Packets;
        protected serverMessage fuseResponse;
        protected clientMessage fuseRequest;

        public HabboMessages(GameConnection Session)
        {
            this.Session = Session;
            this.Packets = new Dict<string, Packet>();

            this.RegisterHandshake();
        }

        protected void SendResponse()
        {
            Session.SendMessage(this.fuseResponse);
        }

        internal void TryHandle(string header, clientMessage cMessage, GameConnection connection)
        {
            if (Packets.ContainsKey(header))
            {
                fuseRequest = cMessage;
                Packets[header]();
            }
        }
    }
}
