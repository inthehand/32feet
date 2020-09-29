// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.Headers.ObexHeaders
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace InTheHand.Net.Obex.Headers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ObexHeaders : IEnumerable<KeyValuePair<ObexHeader,object>>
    {
        private readonly Dictionary<ObexHeader, object> _values = new Dictionary<ObexHeader, object>();

        public void Add(ObexHeader key, string value)
        {
            _values.Add(key, value);
        }

        public void Add(ObexHeader key, byte[] value)
        {
            _values.Add(key, value);
        }

        public void Add(ObexHeader key, uint value)
        {
            _values.Add(key, value);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(ObexHeader key)
        {
            return _values.ContainsKey(key);
        }

        public object GetValue(ObexHeader key)
        {
            return _values[key];
        }

        public void Remove(ObexHeader key)
        {
            _values.Remove(key);
        }

        public IEnumerator<KeyValuePair<ObexHeader, object>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
    }
}
