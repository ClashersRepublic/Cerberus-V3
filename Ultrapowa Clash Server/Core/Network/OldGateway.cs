using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UCS.Core.Settings;
using UCS.Core.Threading;
using UCS.Helpers;
using UCS.PacketProcessing;
using static UCS.Core.Logger;
using static UCS.Core.Settings.UCSControl;

namespace UCS.Core.Network
{
    internal class OldGateway
    {
        internal SocketAsyncEventArgsPool ReadPool;
        internal SocketAsyncEventArgsPool WritePool;
        internal Socket Listener;
        internal Mutex Mutex;
        internal int ConnectedSockets;

        static Thread T { get; set; }
        public static ManualResetEvent AllDone = new ManualResetEvent(false);

        internal OldGateway()
        {
            this.ReadPool = new SocketAsyncEventArgsPool();
            this.WritePool = new SocketAsyncEventArgsPool();
            this.Initialize();
            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveBufferSize = 2048,
                SendBufferSize = 2048,
                Blocking = false,
                NoDelay = true
            };
            this.Listener.Bind(new IPEndPoint(IPAddress.Any, Utils.ParseConfigInt("ServerPort")));
            this.Listener.Listen(200);
            Logger.Say();
            Logger.Say("UCS has been started on " + (object) Listener.LocalEndPoint);
            SocketAsyncEventArgs AcceptEvent = new SocketAsyncEventArgs();
            AcceptEvent.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            this.StartAccept(AcceptEvent);
        }

        internal void Initialize()
        {
            for (int index = 0; index < Constants.MaxOnlinePlayers + 1; ++index)
            {
                SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
                recvArgs.SetBuffer(new byte[2048], 0, 2048);
                recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
                this.ReadPool.Push(recvArgs);

                SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnSendCompleted);
                this.WritePool.Push(sendArgs);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs AcceptEvent)
        {
            AcceptEvent.AcceptSocket = (Socket)null;
            if (this.Listener.AcceptAsync(AcceptEvent))
                return;
            this.ProcessAccept(AcceptEvent);
        }

        internal void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            Socket Socket = AsyncEvent.AcceptSocket;
            if (Socket.Connected && AsyncEvent.SocketError == SocketError.Success)
            {
                // IP Blocking code.
                //if (ConnectionBlocker.IsAddressBanned(Socket.RemoteEndPoint.ToString().Split(':')[0]))
                //{
                //    Socket.Close(5);
                //    this.StartAccept(AsyncEvent);
                //    return;
                //}

                Logger.Write("New client connected -> " + Socket.RemoteEndPoint.ToString().Split(':')[0]);
                SocketAsyncEventArgs ReadEvent = this.ReadPool.Pop();
                if (ReadEvent != null)
                {
                    Client client = new Client(Socket)
                    {
                        CIPAddress = ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString()
                    };
                    Token token = new Token(ReadEvent, client);
                    Interlocked.Increment(ref this.ConnectedSockets);
                    ResourcesManager.AddClient(client);
                    Task.Run((Action)(() =>
                    {
                        try
                        {
                            if (Socket.ReceiveAsync(ReadEvent))
                                return;
                            this.ProcessReceive(ReadEvent);
                        }
                        catch (Exception ex)
                        {
                            this.Disconnect(ReadEvent);
                        }
                    }));
                }
            }
            else
            {
                Logger.Write("Failed to Receive and Process the Data.");
                Socket.Close(5);
            }
            this.StartAccept(AsyncEvent);
        }

        internal void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessAccept(AsyncEvent);
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                Token userToken = AsyncEvent.UserToken as Token;
                userToken.SetData();
                try
                {
                    if (userToken.Client.Socket.Available == 0)
                    {
                        userToken.Process();
                        if (userToken.Aborting || userToken.Client.Socket.ReceiveAsync(AsyncEvent))
                            return;
                        this.ProcessReceive(AsyncEvent);
                    }
                    //else
                    //{
                    //    if (userToken.Aborting || userToken.Client.Socket.ReceiveAsync(AsyncEvent))
                    //        return;
                    //    this.ProcessReceive(AsyncEvent);
                    //}
                }
                catch (Exception ex)
                {
                    this.Disconnect(AsyncEvent);
                }
            }
            else
                this.Disconnect(AsyncEvent);
        }

        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.ProcessReceive(AsyncEvent);
        }

        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            ResourcesManager.DropClient((AsyncEvent.UserToken as Token).Client.Socket.Handle.ToInt64());
        }

        internal void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            this.WritePool.Push(AsyncEvent);
        }
    }
}
