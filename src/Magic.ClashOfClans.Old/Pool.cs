using Magic.ClashOfClans.Core;
using System.Collections.Concurrent;

namespace Magic.ClashOfClans.Network
{
    public class Pool<T>
    {
        private readonly ConcurrentBag<T> _stack;

        internal Pool()
        {
            _stack = new ConcurrentBag<T>();
        }

        public int Count => _stack.Count;

        public T Pop()
        {
            var ret = default(T);
            if (!_stack.TryTake(out ret))
                return default(T);
            
            return ret;
        }

        public void Push(T item)
        {
            _stack.Add(item);
        }
    }
}
