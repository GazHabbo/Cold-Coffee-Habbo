using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Lightningbolt_Server.Collections
{
    public class Dict<Key, Value> : IEnumerable<Pair<Key, Value>>
    {
        private Key[] _keys;
        private Value[] _values;

        public Dict()
        {
            this._keys = new Key[0];
            this._values = new Value[0];
        }

        public int Length
        {
            get
            {
                return this._keys.Length;
            }
        }

        public int Count
        {
            get
            {
                if (this._keys.Length != this._values.Length)
                {
                    throw new Exception("You don't have the same amount of Keys than values. Hax?");
                }
                else
                {
                    return this._keys.Length;
                }
            }
        }

        public Key[] Keys
        {
            get
            {
                return this._keys;
            }
        }

        public Value[] Values
        {
            get
            {
                return this._values;
            }
        }

        public Value this[Key key]
        {
            get
            {
                for (int i = 0; i < this._keys.Length && i < this._values.Length; i++)
                {
                    if (this._keys[i].Equals(key))
                    {
                        return this._values[i];
                    }
                }

                return default(Value);
            }
            set
            {
                if (this.ContainsKey(key))
                {
                    for (int i = 0; i < this._keys.Length && i < this._values.Length; i++)
                    {
                        if (this._keys[i].Equals(key))
                        {
                            this._values[i] = value;
                        }
                    }
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        public bool Add(Key _key, Value _value)
        {
            if (this.ContainsKey(_key))
            {
                return false;
            }

            if (!StepHigher())
            {
                return false;
            }

            this._keys[Length - 1] = _key;
            this._values[Length - 1] = _value;

            return true;
        }

        private bool StepHigher()
        {
            try
            {
                Key[] newKeys = new Key[Count + 1];
                Value[] newVals = new Value[Count + 1];

                this.CopyKeys(this._keys, newKeys);
                this.CopyValues(this._values, newVals);

                this._keys = newKeys;
                this._values = newVals;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CopyKeys(Key[] old, Key[] newKeys)
        {
            for (int i = 0; i < old.Length && i < newKeys.Length; i++)
            {
                newKeys[i] = old[i];
            }
        }

        private void CopyValues(Value[] old, Value[] newVals)
        {
            for (int i = 0; i < old.Length && i < newVals.Length; i++)
            {
                newVals[i] = old[i];
            }
        }

        public bool ContainsKey(Key key)
        {
            for (int i = 0; i < this._keys.Length; i++)
            {
                if (this._keys[i].Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<Pair<Key, Value>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<Pair<Key, Value>>, IEnumerator
        {
            Dict<Key, Value> dictionary;
            int next;

            internal Pair<Key, Value> current;

            internal Enumerator(Dict<Key, Value> dictionary)
                : this()
            {
                this.dictionary = dictionary;
            }

            public bool MoveNext()
            {
                VerifyState();

                if (next < 0)
                    return false;

                while (next < dictionary.Length)
                {
                    int cur = next++;

                    current = new Pair<Key, Value>(
                        dictionary._keys[cur],
                        dictionary._values[cur]
                        );
                    return true;
                }

                next = -1;
                return false;
            }

            // No error checking happens.  Usually, Current is immediately preceded by a MoveNext(), so it's wasteful to check again
            public Pair<Key, Value> Current
            {
                get { return current; }
            }

            internal Key CurrentKey
            {
                get
                {
                    VerifyCurrent();
                    return current.Key;
                }
            }

            internal Value CurrentValue
            {
                get
                {
                    VerifyCurrent();
                    return current.Value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    VerifyCurrent();
                    return current;
                }
            }

            void IEnumerator.Reset()
            {
                Reset();
            }

            internal void Reset()
            {
                VerifyState();
                next = 0;
            }

            void VerifyState()
            {
                if (dictionary == null)
                    throw new ObjectDisposedException(null);
            }

            void VerifyCurrent()
            {
                VerifyState();
                if (next <= 0)
                    throw new InvalidOperationException("Current is not valid");
            }

            public void Dispose()
            {
                dictionary = null;
            }
        }

        public void Remove(Key key)
        {
            Key[] newKeys = new Key[Count - 1];
            Value[] newVals = new Value[Count - 1];

            for (int i = 0; i < this._keys.Length && i < this._values.Length; i++)
            {
                if (this._keys[i].Equals(key))
                {
                    continue;
                }

                newKeys[i] = this._keys[i];
                newVals[i] = this._values[i];
            }

            this._keys = newKeys;
            this._values = newVals;
        }
    }
}
