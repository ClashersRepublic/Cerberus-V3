using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UCS.PacketProcessing;

namespace UCS.Core.Network
{
    public static class Gateway
    {
        private static Socket s_listener;
        private static SocketAsyncEventArgsPool s_pool;

        public static void Initialize()
        {
            s_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_pool = new SocketAsyncEventArgsPool();

            const int PRE_ALLOC_SEA = 128;
            for (int i = 0; i < PRE_ALLOC_SEA; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                s_pool.Push(args);
            }
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

        private static void StartAccept(SocketAsyncEventArgs e)
        {
            try
            {
                // Avoid stackoverflows cause we can.
                while (true)
                {
                    if (!s_listener.AcceptAsync(e))
                        ProcessAccept(e, false);
                    else break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while starting to accept(critical): " + ex);
            }
        }

        private static void ProcessAccept(SocketAsyncEventArgs e, bool startNew)
        {
            try
            {
                var acceptSocket = e.AcceptSocket;
                Logger.Say($"Accepted connection at {acceptSocket.RemoteEndPoint}");

                // Clean up for reuse.
                e.AcceptSocket = null;

                var client = new Client(acceptSocket);
                // Let UCS know we've got a client.
                ResourcesManager.AddClient(client);

                var args = GetArgs();
                args.SetBuffer(new byte[4096], 0, 4096);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while processing accept: " + ex);
            }

            if (startNew)
                StartAccept(e);
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

                    default:
                        throw new Exception("Unexpected operation.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("We done fucked up: " + ex);
            }
        }

        private static SocketAsyncEventArgs GetArgs()
        {
            var args = s_pool.Pop();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
            }

            return args;
        }
    }
}
