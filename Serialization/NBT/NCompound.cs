﻿using System.Collections;
using System.Collections.Generic;

namespace MineLW.Serialization.NBT
{
    public class NCompound : NTag<NCompound>, IDictionary<string, INTag>, IReadOnlyDictionary<string, INTag>
    {
        public int Count => _dictionary.Count;
        public ICollection<string> Keys => _dictionary.Keys;
        public ICollection<INTag> Values => _dictionary.Values;

        IEnumerable<INTag> IReadOnlyDictionary<string, INTag>.Values => Values;
        IEnumerable<string> IReadOnlyDictionary<string, INTag>.Keys => Keys;

        bool ICollection<KeyValuePair<string, INTag>>.IsReadOnly => false;

        private readonly Dictionary<string, INTag> _dictionary;

        public NCompound() : base(null)
        {
            _dictionary = new Dictionary<string, INTag>();
        }

        public IEnumerator<KeyValuePair<string, INTag>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<KeyValuePair<string, INTag>>.Add(KeyValuePair<string, INTag> item)
        {
            ((ICollection<KeyValuePair<string, INTag>>) _dictionary).Add(item);
        }

        bool ICollection<KeyValuePair<string, INTag>>.Contains(KeyValuePair<string, INTag> item)
        {
            return ((ICollection<KeyValuePair<string, INTag>>) _dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<string, INTag>>.CopyTo(KeyValuePair<string, INTag>[] array,
            int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, INTag>>) _dictionary).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, INTag>>.Remove(KeyValuePair<string, INTag> item)
        {
            return ((ICollection<KeyValuePair<string, INTag>>) _dictionary).Remove(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void Add(string key, INTag tag)
        {
            _dictionary[key] = tag;
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out INTag tag)
        {
            return _dictionary.TryGetValue(key, out tag);
        }

        public INTag this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
    }
}