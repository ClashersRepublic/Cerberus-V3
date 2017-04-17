using Magic.Network;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Magic
{
    // Code that handles sending process.
    public partial class Server
    {
        internal void Send(byte[] data, Client client)
        {
            var args = _context.GetArgs();
            args.UserToken = client;
            args.SetBuffer(data, 0, data.Length);

            StartSend(args, client.Connection);
        }

        private void StartSend(SocketAsyncEventArgs e, Connection conn)
        {
            Debug.Assert(e != null);

            if (!conn.Socket.SendAsync(e))
                ProcessSend(e);
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            Debug.Assert(e != null);

            while (true)
            {
                var client = (Client)e.UserToken;
                var transferred = e.BytesTransferred;
                if (transferred == 0 || e.SocketError != SocketError.Success)
                {
                    // TODO: Handle disconnect & Recycle.
                    Console.WriteLine("Socket has been disconnected.");
                    _context.Recycle(e);
                    return;
                }

                if (transferred < e.Count)
                {
                    e.SetBuffer(transferred, e.Count - transferred);
                    if (client.Connection.Socket.SendAsync(e))
                        break;
                }
                else
                {
                    _context.Recycle(e);
                    break;
                }
            }
        }
    }
}
