using System;
using System.Net.Sockets;

namespace Magic.Core
{
    public class ServerContext : IDisposable
    {
        internal ServerContext(EventHandler<SocketAsyncEventArgs> handler, int bufferSize)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _bufferSize = bufferSize;
            _handler = handler;
            _args = new Pool<SocketAsyncEventArgs>(128);
            _buffers = new Pool<byte[]>(128);
        }

        private bool _disposed;
        private readonly int _bufferSize;
        private readonly EventHandler<SocketAsyncEventArgs> _handler;
        private readonly Pool<SocketAsyncEventArgs> _args;
        private readonly Pool<byte[]> _buffers;

        public SocketAsyncEventArgs GetArgs()
        {
            var args = _args.Pop();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += _handler;
            }
            return args;
        }

        public byte[] GetBuffer() => _buffers.Pop() ?? new byte[_bufferSize];

        public void Recycle(SocketAsyncEventArgs e)
        {
            if (e != null)
            {
                var buffer = e.Buffer;
                Recycle(buffer);

                e.SetBuffer(null, 0, 0);
                e.AcceptSocket = null;
                e.UserToken = null;
                _args.Push(e);
            }
        }

        public void Recycle(byte[] buffer)
        {
            if (buffer != null)
                _buffers.Push(buffer);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                while (_args.Count != 0)
                {
                    var args = _args.Pop();
                    args.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
