using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace Magic.Network
{
    // Code that handles receiving process.
    public partial class Server
    {
        private void StartReceive(Client client)
        {
            Debug.Assert(client != null);

            try
            {
                var buffer = _context.GetBuffer();
                var args = _context.GetArgs();
                var socket = client.Connection.Socket;

                args.UserToken = client;
                args.SetBuffer(buffer, 0, buffer.Length);

                while (true)
                {
                    if (!socket.ReceiveAsync(args))
                        ProcessReceive(args, false);
                    else break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: Exception while StartReceive: " + ex.ToString());
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e, bool startNew)
        {
            Debug.Assert(e != null);

            var client = (Client)e.UserToken;
            while (true)
            {
                try
                {
                    var transfered = e.BytesTransferred;
                    if (transfered == 0 || e.SocketError != SocketError.Success)
                    {
                        // TODO: Handle disconnect & Recycle.
                        Console.WriteLine("Socket has been disconnected.");
                        _context.Recycle(e);

                        // Remove client from the client list.
                        var tmpClient = default(Client);
                        _clients.TryTake(out tmpClient);
                        return;
                    }

                    var buffer = client.Connection._incomingBuffer;
                    for (int i = 0; i < transfered; i++)
                        buffer.Add(e.Buffer[i + e.Offset]);

                    // Process only if we've got the header.
                    if (buffer.Count >= 7)
                    {
                        var id = (ushort)((buffer[0] << 8) | (buffer[1]));
                        var len = (buffer[2] << 16) | (buffer[3] << 8) | (buffer[4]);
                        var version = (ushort)((buffer[5] << 8) | (buffer[6]));
                        // Continue processing if we've got enough data as well.
                        if (buffer.Count + 7 >= len)
                        {
                            var cipher = new byte[len];
                            for (int i = 0; i < len; i++)
                                cipher[i] = buffer[i + 7];

                            var crypto = client.Connection._crypto;
                            var message = MessageFactory.Create(id);
                            try
                            {
                                var plaintext = crypto.ProcessIncoming(cipher);
                                using (var reader = new MessageReader(new MemoryStream(plaintext)))
                                {
                                    message.Decode(reader);
#if DEBUG
                                    if (reader.BaseStream.Position != reader.BaseStream.Length)
                                        Console.WriteLine("PACKET: Did not fully read message {0}", id);
#endif
                                }

                                //_handler.HandleAsync(message, client);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("ERROR: Unable to process incoming message. " + ex.ToString());
                            }

                            // Clean up buffer.
                            buffer.RemoveRange(0, 7 + len);
                        }
                        else
                        {
                            // Just some slight resizing.
                            if (buffer.Capacity < len)
                                buffer.Capacity = len;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("ERROR: Exception while ProcessReceive: " + ex.ToString());
                }

                // Start receiving again when we done with the client.
                if (startNew && client.Connection.Socket.ReceiveAsync(e))
                    break;
            }
        }
    }
}
