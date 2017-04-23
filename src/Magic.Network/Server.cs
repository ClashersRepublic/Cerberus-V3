using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Magic.Network
{
    public partial class Server : IDisposable
    {
        public Server()
        {
            _context = new ServerContext(IOCompleted, 4096);
            _clients = new BlockingCollection<Client>(256);

            _handler = new MessageHandler(this);
        }

        private bool _started;
        private bool _disposed;

        private Socket _listener;
        private readonly BlockingCollection<Client> _clients;
        private readonly MessageHandler _handler;
        internal readonly ServerContext _context;

        public void Start()
        {
            if (_started)
                throw new InvalidOperationException("Server already started.");

            var endPoint = new IPEndPoint(IPAddress.Any, 9339);
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(endPoint);
            _listener.Listen(100);

            // Start accepting new connections.
            var args = _context.GetArgs();
            StartAccept(args);

            _started = true;
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
                try { _listener.Dispose(); }
                catch { /* Swallow */ }

                // Dispose connected client.
                foreach (var client in _clients)
                {
                    try { client.Dispose(); }
                    catch { /* Swallow */ }
                }

                try { _context.Dispose(); }
                catch { /* Swallow */ }
            }

            _disposed = true;
        }

        private void IOCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Accept:
                        ProcessAccept(e, true);
                        break;

                    case SocketAsyncOperation.Receive:
                        ProcessReceive(e, true);
                        break;

                    case SocketAsyncOperation.Send:
                        ProcessSend(e);
                        break;

                    default:
                        Console.WriteLine("Unexpected IO operation.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: Exception while IOCompleted event was executing. " + ex.ToString());
                // Potentially critical.
            }
        }

        private static void KillSocket(Socket sock)
        {
            if (sock != null)
            {
                try { sock.Shutdown(SocketShutdown.Both); }
                catch { /* Swallow */ }
                try { sock.Dispose(); }
                catch { /* Swallow */ }
            }
        }
    }
}
