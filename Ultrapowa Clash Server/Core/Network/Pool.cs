using System.Collections.Generic;

namespace UCS.Core.Network
{
    public class Pool<T>
    {
        private readonly object _sync = new object();
        private readonly Stack<T> _stack;

        internal Pool()
        {
            _stack = new Stack<T>(128);
        }

        internal T Pop()
        {
            lock (_sync)
            {
                if (_stack.Count > 0)
                    return _stack.Pop();

                return default(T);
            }
        }

        internal void Push(T Args)
        {
            lock (_sync)
            {
                _stack.Push(Args);
            }
        }
    }
}
