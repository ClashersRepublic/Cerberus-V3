using System.Collections.Generic;

namespace UCS.Core.Network
{
    public class Pool<T>
    {
        private readonly object _sync = new object();
        private readonly Queue<T> _stack;

        internal Pool()
        {
            _stack = new Queue<T>(128);
        }

        internal T Pop()
        {
            lock (_sync)
            {
                if (_stack.Count > 0)
                    return _stack.Dequeue();

                return default(T);
            }
        }

        internal void Push(T item)
        {
            lock (_sync)
            {
                _stack.Enqueue(item);
            }
        }
    }
}
