﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MineLW.Serialization.Json
{
    public class JObject : JElement, IDictionary<string, JElement>, IReadOnlyDictionary<string, JElement>
    {
        public int Count => _dictionary.Count;
        public ICollection<string> Keys => _dictionary.Keys;
        public ICollection<JElement> Values => _dictionary.Values;

        IEnumerable<JElement> IReadOnlyDictionary<string, JElement>.Values => Values;
        IEnumerable<string> IReadOnlyDictionary<string, JElement>.Keys => Keys;

        bool ICollection<KeyValuePair<string, JElement>>.IsReadOnly => false;

        private readonly Dictionary<string, JElement> _dictionary;

        public JObject(Dictionary<string, JElement> dictionary = null)
        {
            _dictionary = dictionary ?? new Dictionary<string, JElement>();
        }

        public T ToObject<T>(string key, Func<JElement, T> func)
        {
            return func(_dictionary[key]);
        }

        public T Optional<T>(string key, T defaultValue) where T : JElement
        {
            return _dictionary.ContainsKey(key) ? _dictionary[key].Get<T>() : defaultValue;
        }

        public IEnumerator<KeyValuePair<string, JElement>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<KeyValuePair<string, JElement>>.Add(KeyValuePair<string, JElement> item)
        {
            ((ICollection<KeyValuePair<string, JElement>>) _dictionary).Add(item);
        }

        bool ICollection<KeyValuePair<string, JElement>>.Contains(KeyValuePair<string, JElement> item)
        {
            return ((ICollection<KeyValuePair<string, JElement>>) _dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<string, JElement>>.CopyTo(KeyValuePair<string, JElement>[] array,
            int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, JElement>>) _dictionary).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, JElement>>.Remove(KeyValuePair<string, JElement> item)
        {
            return ((ICollection<KeyValuePair<string, JElement>>) _dictionary).Remove(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void Add(string key, JElement tag)
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

        public bool TryGetValue(string key, out JElement tag)
        {
            return _dictionary.TryGetValue(key, out tag);
        }

        public JElement this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        public override string ToString()
        {
            return _dictionary.ToString();
        }
    }
}