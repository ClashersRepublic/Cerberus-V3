using System;
using System.Net;
using System.Net.Sockets;
using CR.Servers.CoC.Logic;
using CR.Servers.Logic.Enums;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace CR.Servers.CoC.Core.Network
{
    internal class Gateway
    {
        private readonly Socket _listener;
        private readonly ConcurrentDictionary<IntPtr, Device> _connectedDevices;

        internal Gateway()
        {
            this._connectedDevices = new ConcurrentDictionary<IntPtr, Device>();
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
                SocketAsyncEventArgs readEvent = new SocketAsyncEventArgs();
                readEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                readEvent.Completed += this.OnReceiveCompleted;
                readEvent.DisconnectReuseSocket = false;

                Program.Connected();

                Device device = new Device();
                Token token = new Token(readEvent, device, socket);

                _connectedDevices.TryAdd(socket.Handle, device);

                device.State = State.SESSION;

                try
                {
                    if (!socket.ReceiveAsync(readEvent))
                    {
                        this.OnReceiveCompleted(null, readEvent);
                    }
                }
                catch (Exception Exception)
                {
                    Logging.Error(this.GetType(), "Exception while starting to receive in ProcessAccept. trace: " + Exception);
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
            try
            {
                if (args.BytesTransferred > 0)
                {
                    Token token = (Token)args.UserToken;

                    if (token != null && !token.Aborting)
                    {
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
                                    try
                                    {
                                        if (!token.Socket.ReceiveAsync(args))
                                        {
                                            this.ProcessReceive(args);
                                        }
                                    }
                                    catch (Exception Exception)
                                    {
                                        Logging.Error(this.GetType(), "Exception while starting to receive in ProcessReceive. trace: " + Exception);
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
                else
                {
                    this.Disconnect(args);
                }
            }
            catch (Exception Exception)
            {
                Logging.Error(this.GetType(), "Exception in ProcessReceive. trace: " + Exception);
            }
        }

        internal void Send(byte[] packet, Token token)
        {
            if (!token.Aborting)
            {
                if (token.Socket.Connected)
                {
                    SocketAsyncEventArgs writeEvent = new SocketAsyncEventArgs();

                    writeEvent.Completed += this.OnSendCompleted;
                    writeEvent.UserToken = token;
                    writeEvent.SetBuffer(packet, 0, packet.Length);


                    if (!token.Socket.SendAsync(writeEvent))
                    {
                        this.OnSendCompleted(token.Socket, writeEvent);
                    }
                }
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            var socket = (Socket)sender;
            if (args.BytesTransferred < args.Count)
            {
                var offset = args.Offset + args.BytesTransferred;
                var count = args.Count - offset;

                args.SetBuffer(offset, count);
                socket.SendAsync(args);
            }
            args.Dispose();
        }

        internal void Disconnect(SocketAsyncEventArgs rcvArgs)
        {
            Program.Disconnected();

            if (rcvArgs.UserToken != null)
            {
                Token token = (Token)rcvArgs.UserToken;

                if (token != null)
                {
                    Device _;
                    _connectedDevices.TryRemove(token.Socket.Handle, out _);

                    if (!token.Aborting)
                    {
                        token.Dispose();
                    }

                    rcvArgs.UserToken = null;
                    rcvArgs.Dispose();
                }
            }
        }
    }
}