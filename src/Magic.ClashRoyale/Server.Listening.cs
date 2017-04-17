using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Magic
{
    // Code that handles listening process.
    public partial class Server
    {
        private void StartAccept(SocketAsyncEventArgs e)
        {
            Debug.Assert(e != null);
            try
            {
                // Keep accepting on the SAEA until operation is done
                // async.
                while (true)
                {
                    if (!_listener.AcceptAsync(e))
                    {
                        ProcessAccept(e, false);
                        e.AcceptSocket = null;
                    }
                    else break;
                }
            }
            catch (Exception ex)
            {
                //NOTE: Listener won't accept anymore potentially if this code reaches.
                Console.Error.WriteLine("ERROR: **CRITICAL** Exception while StartAccept: " + ex.ToString());
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            var socket = default(Socket);
            try
            {
                socket = e.AcceptSocket;
                Console.WriteLine("Accepted new Socket at {0}", socket.RemoteEndPoint.ToString());

                var client = new Client(this, socket);
                var conn = client.Connection;

                // Register client in client list.
                _clients.Add(client);

                StartReceive(client);
            }
            catch (Exception ex)
            {
                KillSocket(socket);
                Console.Error.WriteLine("ERROR: Exception while ProcessAccept: " + ex.ToString());
            }

            e.AcceptSocket = null;
            // Start accepting again when done processing.
            if (startNew)
                StartAccept(e);
        }
    }
}
