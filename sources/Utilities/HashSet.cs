using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public sealed class HashSet<T> : ICollection<T>
    {
        public HashSet()
        {
        }

        public HashSet(IEnumerable<T> collection)
        {
            foreach (T s in collection)
            {
                dict.Add(s, null);
            }
        }

        public void Add(T element)
        {
            if (dict.ContainsKey(element))
            {
                throw new ArgumentException("element");
            }
            dict.Add(element, element);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(T element)
        {
            return dict.ContainsKey(element);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            if (array.Rank != 0 || arrayIndex >= array.Length || arrayIndex + dict.Count > array.Length)
            {
                throw new ArithmeticException();
            }

            foreach (var item in dict)
            {
                array[arrayIndex++] = item.Key;
            }
        }

        public bool Remove(T element)
        {
            return dict.Remove(element);
        }

        public void AddIfNotExists(T element)
        {
            if (!dict.ContainsKey(element))
            {
                dict.Add(element, element);
            }
        }

        public int Count
        {
            get
            {
                return dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in dict)
            {
                yield return item.Key;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Dictionary<T, object> dict = new Dictionary<T, object>();
    }
}
