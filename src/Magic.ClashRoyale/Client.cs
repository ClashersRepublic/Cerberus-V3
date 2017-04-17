using Magic.Logic;
using Magic.Network;
using Magic.Network.Cryptography;
using System;
using System.IO;
using System.Net.Sockets;

namespace Magic
{
    public class Client : IDisposable
    {
        public Client(Server server, Socket socket)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            _server = server;
            _conn = new Connection(socket);
            _conn._crypto = new MessageCryptoNaCl.Server(MessageCryptoNaCl.StandardKeyPair);
        }

        internal Level _level;
        private bool _disposed;
        private readonly Server _server;
        private readonly Connection _conn;

        public Level Level => _level;
        public Connection Connection => _conn;

        public void SendMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var stream = new MemoryStream();
            using (var writer = new MessageWriter(stream))
            {
                message.WriteMessage(writer);

                var plaintext = stream.ToArray();
                var cipher = _conn._crypto.ProcessOutgoing(plaintext);
                var len = BitConverter.GetBytes(cipher.Length);
                Array.Reverse(len);

                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(message.Id);
                writer.Write(len, 1, 3);
                writer.Write(message.Version);

                writer.Write(cipher);

                var bytes = stream.ToArray();
                _server.Send(bytes, this);
            }
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
                _conn.Dispose();
            }

            _disposed = true;
        }
    }
}
