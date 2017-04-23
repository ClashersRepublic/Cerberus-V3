using System.Collections.Concurrent;

namespace Magic.ClashOfClans.Network
{
    public class Pool<T>
    {
        private readonly object _sync = new object();
        private readonly ConcurrentQueue<T> _stack;

        internal Pool()
        {
            _stack = new ConcurrentQueue<T>();
        }

        internal T Pop()
        {
            var ret = default(T);
            if (_stack.Count > 0)
                _stack.TryDequeue(out ret);

            return ret;
        }

        internal void Push(T item)
        {
            _stack.Enqueue(item);
        }
    }
}
