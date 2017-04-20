using Magic.ClashOfClans;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Network;
using Magic.Packets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Magic.ClashOfClans.Core.Network
{
    internal static class Gateway
    {
        private static Socket s_listener;
        private static Pool<SocketAsyncEventArgs> s_argsPool;
        private static Pool<byte[]> s_bufferPool;

        public static void Initialize()
        {
            s_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_argsPool = new Pool<SocketAsyncEventArgs>();

            const int PRE_ALLOC_SEA = 128;
            for (int i = 0; i < PRE_ALLOC_SEA; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                s_argsPool.Push(args);
            }

            s_bufferPool = new Pool<byte[]>();
        }

        public static void Listen()
        {
            const int PORT = 9339;
            const int BACK_LOG = 100;

            var endPoint = new IPEndPoint(IPAddress.Any, PORT);

            s_listener.Bind(endPoint);
            s_listener.Listen(BACK_LOG);

            var args = GetArgs();
            StartAccept(args);

            Logger.Say($"Listening on {endPoint}...");
        }

        public static void Send(this Message message)
        {
            var buffer = default(byte[]);
            var client = message.Client;

            try { message.Encode(); }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while encoding message {message.GetType()}");
                return;
            }

            try { buffer = message.GetRawData(); }
            catch (Exception ex)
            {
                // Exit early since buffer will be null, we can't send a null buffer to the client.
                ExceptionLogger.Log(ex, $"Exception while constructing message {message.GetType()}");
                return;
            }

            var socket = client.Socket;

            var args = GetArgs();
            args.SetBuffer(buffer, 0, buffer.Length);
            args.UserToken = client;

            try { message.Process(client.Level); }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while processing outgoing message {message.GetType()}");
            }

            StartSend(args);
        }

        private static void StartSend(SocketAsyncEventArgs e)
        {
            var client = (Client)e.UserToken;
            var socket = client.Socket;

            if (Thread.VolatileRead(ref client._dropped) == 1)
            {
                Recycle(e);
            }
            else
            {
                try
                {
                    while (true)
                    {
                        if (!socket.SendAsync(e))
                            ProcessSend(e);
                        else break;
                    }
                }
                catch (ObjectDisposedException)
                {
                    Recycle(e);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while starting receive");
                }
            }
        }

        private static void ProcessSend(SocketAsyncEventArgs e)
        {
            var client = (Client)e.UserToken;
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                ResourcesManagerOld.DropClient(client.GetSocketHandle());
                Recycle(e);
            }
            else
            {
                try
                {
                    var count = e.Count;
                    if (transferred < count)
                    {
                        e.SetBuffer(transferred, count - transferred);
                        StartSend(e);
                    }
                    else
                    {
                        // We done with sending can recycle EventArgs.
                        Recycle(e);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while processing send");
                }
            }
        }

        private static void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                // Avoid StackOverflowExceptions cause we can.
                while (true)
                {
                    if (!s_listener.AcceptAsync(e))
                        ProcessAccept(e, false);
                    else break;
                }
            }
            catch (Exception ex)
            {
                // If this is reached, we won't start listening again, therefore no more clients.
                ExceptionLogger.Log(ex, "Exception while starting to accept(critical)");
                // Could try to resurrect listeners or something here.
            }
        }

        private static void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            var acceptSocket = e.AcceptSocket;
            if (e.SocketError != SocketError.Success)
            {
                KillSocket(acceptSocket);
                Logger.Say($"Failed to accept new socket: {e.SocketError}.");
            }
            else
            {
                try
                {
                    Logger.Say($"Accepted connection at {acceptSocket.RemoteEndPoint}.");

                    var client = new Client(acceptSocket);

                    // Register the client in the ResourceManager.
                    ResourcesManagerOld.AddClient(client);
                    ResourcesManager.Instance.RegisterClient(client);

                    var args = GetArgs();
                    var buffer = GetBuffer();
                    args.UserToken = client;
                    args.SetBuffer(buffer, 0, buffer.Length);

                    StartReceive(args);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while processing accept");
                }
            }

            // Clean up for reuse.
            e.AcceptSocket = null;
            if (startNew)
                StartAccept(e);
        }

        private static void StartReceive(SocketAsyncEventArgs e)
        {
            var client = (Client)e.UserToken;
            var socket = client.Socket;

            if (Thread.VolatileRead(ref client._dropped) == 1)
            {
                Recycle(e);
            }
            else
            {
                try
                {
                    while (true)
                    {
                        if (!socket.ReceiveAsync(e))
                            ProcessReceive(e, false);
                        else break;
                    }
                }
                catch (ObjectDisposedException)
                {
                    Recycle(e);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while start receive: ");
                }
            }
        }

        private static void ProcessReceive(SocketAsyncEventArgs e, bool startNew)
        {
            var client = (Client)e.UserToken;
            var transferred = e.BytesTransferred;
            if (transferred == 0 || e.SocketError != SocketError.Success)
            {
                // Drop the client, which disposes connected socket.
                ResourcesManagerOld.DropClient(client.GetSocketHandle());
                Recycle(e);
            }
            else
            {
                try
                {
                    var buffer = e.Buffer;
                    var offset = e.Offset; // 0 anyways.
                    for (int i = 0; i < transferred; i++)
                        client.DataStream.Add(buffer[offset + i]);

                    var level = client.Level;
                    var message = default(Message);
                    while (client.TryGetPacket(out message))
                    {
                        try { message.Process(level); }
                        catch (Exception ex)
                        {
                            ExceptionLogger.Log(ex, $"Exception while processing incoming message {message.GetType()}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex, "Exception while processing receive");
                }

                if (startNew)
                    StartReceive(e);
            }
        }

        private static void AsyncOperationCompleted(object sender, SocketAsyncEventArgs e)
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
                        throw new Exception("Unexpected operation.");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception occurred while processing async operation(potentially critical)");
            }
        }

        private static void Recycle(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;

            var buffer = e.Buffer;
            e.UserToken = null;
            e.SetBuffer(null, 0, 0);
            e.AcceptSocket = null;

            s_argsPool.Push(e);

            Recycle(buffer);
        }

        private static void Recycle(byte[] buffer)
        {
            if (buffer == null)
                return;

            if (buffer.Length == Constants.BufferSize)
                s_bufferPool.Push(buffer);
        }

        private static void KillSocket(Socket socket)
        {
            if (socket == null)
                return;

            try { socket.Shutdown(SocketShutdown.Both); }
            catch { /* Swallow */ }
            try { socket.Dispose(); }
            catch { /* SWallow */ }
        }

        private static SocketAsyncEventArgs GetArgs()
        {
            var args = s_argsPool.Pop();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
            }
            return args;
        }

        private static byte[] GetBuffer() => s_bufferPool.Pop() ?? new byte[Constants.BufferSize];
    }
}
