using Magic.Network.Cryptography;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Magic.Network
{
    public class Connection : IDisposable
    {
        public Connection(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            _socket = socket;

            _incomingBuffer = new List<byte>(4096);
        }

        private bool _disposed;
        internal byte[] _id;

        // Shitty way of storing incoming data.
        // Might upgrade in the future.
        internal List<byte> _incomingBuffer;

        internal MessageCrypto _crypto;
        private readonly Socket _socket;

        public Socket Socket => _socket;

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
                _socket.Dispose();
            }

            _disposed = true;
        }
    }
}
