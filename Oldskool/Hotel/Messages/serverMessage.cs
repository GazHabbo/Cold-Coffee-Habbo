using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Utils;

namespace Reflection.Hotel.Messages
{
    public class serverMessage
    {
        private StringBuilder packet;
        private string header;

        public serverMessage(string header)
        {
            this.packet = new StringBuilder(header);

            this.header = header;
        }

        public void AddWiredInt(int i)
        {
            this.packet.Append(HabboEncoding.encodeVL64(i));
        }

        public void AddWiredBoolean(bool b)
        {
            this.packet.Append((b) ? 'I' : 'H');
        }

        public void AddString(string s)
        {
            this.AddString(s, 2);
        }

        public void AddString(string s, int c)
        {
            this.packet.Append(s);
            this.packet.Append((char)c);
        }

        public void AddRawInt(int i)
        {
            this.AddString(i.ToString());
        }

        public void Raw(object obj)
        {
            this.packet.Append(obj);
        }

        public override string ToString()
        {
            this.packet.Append((char)1);
            return this.packet.ToString();
        }
    }
}
