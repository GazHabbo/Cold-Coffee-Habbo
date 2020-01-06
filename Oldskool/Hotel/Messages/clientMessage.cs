using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reflection.Utils;

namespace Reflection.Hotel.Messages
{
    public class clientMessage
    {
        private char[] content;
        private int pos;

        public int Length
        {
            get
            {
                return content.Length - pos;
            }
        }

        public clientMessage(char[] content)
        {
            this.content = content;
            this.pos = 0;
        }

        private char[] ReadChars(int amount)
        {
            var data = new char[amount];

            for (int i = 0; i < data.Length && pos < this.content.Length; i++)
            {
                data[i] = this.content[pos++];
            }

            return data;
        }

        internal string ReadString()
        {
            int strlen = HabboEncoding.decodeB64(new string(ReadChars(2)));
            return new string(ReadChars(strlen));
        }

        internal void Skip(int amount)
        {
            this.ReadChars(amount);
        }

        internal int ReadWiredInteger()
        {
            int result = HabboEncoding.decodeVL64(this.content);
            pos += HabboEncoding.encodeVL64(result).Length;
            return result;
        }

        public override string ToString()
        {
            return string.Join("", this.content);
        }

        internal int ReadB64Int()
        {
            return HabboEncoding.decodeB64(new string(this.ReadChars(2)));
        }
    }
}
