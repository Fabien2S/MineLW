﻿using System.Collections;
using System.Collections.Generic;

namespace MineLW.Serialization.Json
{
    public class JArray : JElement, IList<JElement>
    {
        public int Count => _list.Count;
        public bool IsReadOnly => false;

        private readonly List<JElement> _list;

        public JArray(List<JElement> list = null)
        {
            _list = list ?? new List<JElement>();
        }

        public IEnumerator<JElement> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(JElement item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(JElement item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(JElement[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(JElement item)
        {
            return _list.Remove(item);
        }

        public int IndexOf(JElement item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, JElement item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public JElement this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public override string ToString()
        {
            return '[' + string.Join(", ", _list) + ']';
        }
    }
}