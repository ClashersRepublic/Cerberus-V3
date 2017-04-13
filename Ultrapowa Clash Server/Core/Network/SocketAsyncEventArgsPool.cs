using System.Collections.Generic;
using System.Net.Sockets;
using UCS.Core.Settings;

namespace UCS.Core.Network
{
    internal class SocketAsyncEventArgsPool
    {
        private readonly object _sync = new object();
        private readonly Stack<SocketAsyncEventArgs> _stack;

        internal SocketAsyncEventArgsPool()
        {
            _stack = new Stack<SocketAsyncEventArgs>(128);
        }

        internal SocketAsyncEventArgs Pop()
        {
            lock (_sync)
            {
                if (_stack.Count > 0)
                    return _stack.Pop();

                return null;
            }
        }

        internal void Push(SocketAsyncEventArgs Args)
        {
            lock (this._sync)
            {
                _stack.Push(Args);
            }
        }
    }
}
