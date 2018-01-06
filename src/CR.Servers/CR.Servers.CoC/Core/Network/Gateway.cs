namespace CR.Servers.CoC.Core.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class Gateway
    {
        private readonly Socket _listener;
        private readonly SocketAsyncEventArgsPool _rcvPool;
        private readonly SocketAsyncEventArgsPool _sndPool;

        private readonly byte[] _emptyBuffer = new byte[0];

        internal Gateway()
        {
            this._rcvPool = new SocketAsyncEventArgsPool(false);
            this._sndPool = new SocketAsyncEventArgsPool(true);

            foreach (SocketAsyncEventArgs rcvArgs in this._rcvPool)
            {
                rcvArgs.Completed += this.OnReceiveCompleted;
            }

            foreach (SocketAsyncEventArgs sndArgs in this._sndPool)
            {
                sndArgs.Completed += this.OnSendCompleted;
            }

            this._listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            this._listener.Listen(500);

            Logging.Info(this.GetType(), "Gateway started :)");

            SocketAsyncEventArgs acceptEvent = new SocketAsyncEventArgs();
            acceptEvent.Completed += this.OnAcceptCompleted;
            this.Accept(acceptEvent);
        }

        internal void Accept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            if (!this._listener.AcceptAsync(args))
            {
                this.OnAcceptCompleted(null, args);
            }
        }

        internal void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                this.ProcessAccept(args);
            }

            this.Accept(args);
        }

        internal void ProcessAccept(SocketAsyncEventArgs args)
        {
            Socket socket = args.AcceptSocket;

            if (socket != null && socket.Connected)
            {
                SocketAsyncEventArgs readArgs = this._rcvPool.Dequeue();

                if (readArgs == null)
                {
                    readArgs = new SocketAsyncEventArgs();
                    readArgs.Completed += this.OnReceiveCompleted;
                    readArgs.DisconnectReuseSocket = false;
                    readArgs.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                }

                Device device = new Device();
                Token token = new Token(readArgs, device, socket);

                device.State = State.SESSION;
                Program.Connected();

                if (!socket.ReceiveAsync(readArgs))
                {
                    this.OnReceiveCompleted(null, readArgs);
                }
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                this.ProcessReceive(args);
            }
            else
            {
                this.Disconnect(args);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0)
            {
                Token token = (Token) args.UserToken;

                if (token.Socket.Connected)
                {
                    token.SetData();

                    try
                    {
                        if (token.Socket.Available == 0)
                        {
                            token.Process();
                        }

                        if (!token.Aborting)
                        {
                            if (!token.Socket.ReceiveAsync(args))
                            {
                                this.ProcessReceive(args);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.Error(this.GetType(), "An error has been throwed when the handle of data. trace: " + exception);
                        this.Disconnect(args);
                    }
                }
                else
                {
                    this.Disconnect(args);
                }
            }
            else
            {
                this.Disconnect(args);
            }
        }

        internal void Send(byte[] packet, Socket socket)
        {
            if (socket.Connected)
            {
                SocketAsyncEventArgs sendArgs = this._sndPool.Dequeue();

                if (sendArgs == null)
                {
                    sendArgs = new SocketAsyncEventArgs();
                    sendArgs.Completed += this.OnSendCompleted;
                    sendArgs.DisconnectReuseSocket = false;
                }

                sendArgs.SetBuffer(packet, 0, packet.Length);

                if (!socket.SendAsync(sendArgs))
                {
                    this.OnSendCompleted(null, sendArgs);
                }
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.DisconnectReuseSocket)
            {
                args.SetBuffer(this._emptyBuffer, 0, 0);
                this._sndPool.Enqueue(args);
            }
            else
            {
                args.Dispose();
            }
        }

        internal void Disconnect(SocketAsyncEventArgs rcvArgs)
        {
            Program.Disconnected();

            if (rcvArgs.UserToken != null)
            {
                Token token = (Token) rcvArgs.UserToken;

                if (!token.Aborting)
                {
                    token.Dispose();
                }

                rcvArgs.UserToken = null;

                if (rcvArgs.DisconnectReuseSocket)
                {
                    this._rcvPool.Enqueue(rcvArgs);
                }
                else
                {
                    rcvArgs.Dispose();
                }
            }
        }
    }
}