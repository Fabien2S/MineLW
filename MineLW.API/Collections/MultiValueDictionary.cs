using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MineLW.API.Collections
{
    public class MultiValueDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>>
    {
        public int Count => _dictionary.Count;

        public IEnumerable<TKey> Keys => _dictionary.Keys;
        public IEnumerable<IReadOnlyCollection<TValue>> Values => _dictionary.Values;

        private readonly Dictionary<TKey, Collection> _dictionary;

        public MultiValueDictionary()
        {
            _dictionary = new Dictionary<TKey, Collection>();
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/> associated with the given key.
        /// </summary>
        /// <param name="key">The key of the <see cref="ICollection{T}"/></param>
        /// <param name="value">The object to add to the <see cref="ICollection{T}"/></param>
        public void Add(TKey key, TValue value)
        {
            if (_dictionary.TryGetValue(key, out var collection))
            {
                collection.Add(value);
                return;
            }

            _dictionary[key] = new Collection(new HashSet<TValue>
            {
                value
            });
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/> of the given key.
        /// </summary>
        /// <param name="key">The key of the <see cref="ICollection{T}"/></param>
        /// <param name="value">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <returns>true if item was successfully removed from the <see cref="ICollection{T}"/>; otherwise, false. This method also returns false if item is not found in the original <see cref="ICollection{T}"/>.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public bool Remove(TKey key, TValue value)
        {
            if (!_dictionary.TryGetValue(key, out var collection))
                throw new KeyNotFoundException();
            return collection.Count == 1 ? _dictionary.Remove(key) : collection.Remove(value);
        }

        public bool Contains(TKey key, TValue value)
        {
            if (!_dictionary.TryGetValue(key, out var collection))
                throw new KeyNotFoundException();
            return collection.Contains(value);
        }

        public bool ContainsValue(TValue value)
        {
            return _dictionary.Any(entry => entry.Value.Contains(value));
        }

        public bool Remove(TValue value)
        {
            return _dictionary.Any(entry => entry.Value.Remove(value));
        }

        public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> GetEnumerator()
        {
            return (IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>) (IEnumerator) _dictionary
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out IReadOnlyCollection<TValue> value)
        {
            if (_dictionary.TryGetValue(key, out var set))
            {
                value = set;
                return true;
            }

            value = null;
            return false;
        }

        public IReadOnlyCollection<TValue> this[TKey key] => _dictionary[key];

        private class Collection : ICollection<TValue>, IReadOnlyCollection<TValue>
        {
            public int Count => _collection.Count;
            public bool IsReadOnly => _collection.IsReadOnly;

            private readonly ICollection<TValue> _collection;

            public Collection(ICollection<TValue> collection)
            {
                _collection = collection;
            }

            public IEnumerator<TValue> GetEnumerator() => _collection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(TValue item)
            {
                _collection.Add(item);
            }

            public void Clear()
            {
                _collection.Clear();
            }

            public bool Contains(TValue item)
            {
                return _collection.Contains(item);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                _collection.CopyTo(array, arrayIndex);
            }

            public bool Remove(TValue item)
            {
                return _collection.Remove(item);
            }
        }
    }
}