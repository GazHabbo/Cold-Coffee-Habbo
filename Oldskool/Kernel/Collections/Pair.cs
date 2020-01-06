using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lightningbolt_Server.Collections
{
    public class Pair<TKey, TValue>
    {
        private TKey _key;
        private TValue _value;

        public Pair(TKey _key, TValue _value)
        {
            this._key = _key;
            this._value = _value;
        }

        public TKey Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public TValue Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
    }
}
