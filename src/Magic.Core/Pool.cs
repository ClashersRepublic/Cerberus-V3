using System;
using System.Diagnostics;
using System.Threading;

namespace Magic.Core
{
    [DebuggerDisplay("Count = {Count}")]
    public class Pool<T>
    {
        public Pool(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();

            _items = new T[capacity];
        }

        private T[] _items;
        private int _head;

        public T this[int index] => _items[index];

        public int Count => Thread.VolatileRead(ref _head);

        public void Push(T item)
        {
            lock (_items)
            {
                _items[_head++] = item;

                if (_head == _items.Length)
                    Array.Resize(ref _items, _items.Length * 2);
            }
        }

        public T Pop()
        {
            lock (_items)
            {
                // Avoid going out of bound.
                if (_head <= 0)
                    return default(T);

                var value = _items[--_head];

                // Reset buffer value.
                _items[_head] = default(T);
                return value;
            }
        }
    }
}
