using System;
using System.Net;
using System.Net.Sockets;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Core.Network
{
    internal class Gateway
    {
        internal Pool<SocketAsyncEventArgs> ReadPool;
        internal Pool<SocketAsyncEventArgs> WritePool;

        internal Socket Listener;

        internal Gateway()
        {
            this.ReadPool = new Pool<SocketAsyncEventArgs>();
            this.WritePool = new Pool<SocketAsyncEventArgs>();

            this.Initialize();

            this.Listener =
                new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveBufferSize = Constants.ReceiveBuffer,
                    SendBufferSize = Constants.SendBuffer,
                    Blocking = false,
                    NoDelay = true
                };



            this.Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            this.Listener.Listen(300);

            Console.WriteLine("Server is listening on " + this.Listener.LocalEndPoint + ".");

            SocketAsyncEventArgs AcceptEvent = new SocketAsyncEventArgs();
            AcceptEvent.Completed += this.OnAcceptCompleted;

            this.StartAccept(AcceptEvent);
        }

        internal void Initialize()
        {
            for (int Index = 0; Index < Constants.MaxPlayers; Index++)
            {
                SocketAsyncEventArgs ReadEvent = new SocketAsyncEventArgs();

                ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);

                ReadEvent.Completed += this.OnReceiveCompleted;
                ReadEvent.DisconnectReuseSocket = true;

                this.ReadPool.Push(ReadEvent);

            }

            for (int Index = 0; Index < Constants.MaxSends; Index++)
            {
                SocketAsyncEventArgs WriteEvent = new SocketAsyncEventArgs();

                WriteEvent.Completed += this.OnSendCompleted;
                WriteEvent.DisconnectReuseSocket = true;

                this.WritePool.Push(WriteEvent);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs AcceptEvent)
        {
            AcceptEvent.AcceptSocket = null;
            AcceptEvent.RemoteEndPoint = null;

            if (!this.Listener.AcceptAsync(AcceptEvent))
            {
                this.OnAcceptCompleted(null, AcceptEvent);
            }
        }

        internal void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                this.ProcessAccept(AsyncEvent);
            }
            else
            {
                AsyncEvent.AcceptSocket.Close();

                Logging.Error(this.GetType(), "Something happened when accepting a new connection, aborting.");

                this.StartAccept(AsyncEvent);
            }
        }
        internal void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            Socket Socket = AsyncEvent.AcceptSocket;

#if CHRONO
            Performance Chrono  = new Performance();
#endif

            if (Socket.Connected)
            {
                SocketAsyncEventArgs ReadEvent = this.ReadPool.Pop();

                if (ReadEvent == null)
                {
                    ReadEvent = new SocketAsyncEventArgs();

                    ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                    ReadEvent.Completed += this.OnReceiveCompleted;

                    ReadEvent.DisconnectReuseSocket = false;
                }

                Console.WriteLine("A nigger just connected to the emulator");
                Device Device = new Device(Socket);
                Token Token = new Token(ReadEvent, Device);

                Device.State = State.SESSION;

                if (!Socket.ReceiveAsync(ReadEvent))
                {
                    this.ProcessReceive(ReadEvent);
                }
            }
            else
            {
                Socket.Close();
            }

#if CHRONO
            TimeSpan TimeTaken = Chrono.Stop();

            if (TimeTaken.TotalSeconds > 1.0)
            {
                Logging.Error(this.GetType(), "Took " + TimeTaken.TotalSeconds + " seconds to accept a client.");
            }
#endif

            this.StartAccept(AsyncEvent);
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                Token Token = (Token)AsyncEvent.UserToken;

                if (!Token.Aborting)
                {
                    Token.SetData();

                    try
                    {
                        if (Token.Device.Socket.Available == 0)
                        {
                            Token.Process();

                            if (!Token.Aborting)
                            {
                                if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                                {
                                    this.ProcessReceive(AsyncEvent);
                                }
                            }
                        }
                        else
                        {
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                            {
                                this.ProcessReceive(AsyncEvent);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.Disconnect(AsyncEvent);
                    }
                }
            }
            else
            {
                this.Disconnect(AsyncEvent);
            }
        }

        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.SocketError == SocketError.Success)
            {
                this.ProcessReceive(AsyncEvent);
            }
            else
            {
                this.Disconnect(AsyncEvent);
            }
        }

        internal void Send(Message Message)
        {
            if (Message.Device.Connected)
            {
                SocketAsyncEventArgs WriteEvent = this.WritePool.Pop();

                if (WriteEvent == null)
                {
                    WriteEvent = new SocketAsyncEventArgs
                    {
                        DisconnectReuseSocket = false
                    };
                }

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;
                WriteEvent.UserToken = Message.Device.Token;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                {
                    this.ProcessSend(Message, WriteEvent);
                }
            }
            else
            {
                this.Disconnect(Message.Device?.Token?.Args);
            }
        }

        internal void ProcessSend(Message Message, SocketAsyncEventArgs Args)
        {
            if (Args.SocketError == SocketError.Success)
            {
                Message.Offset += Args.BytesTransferred;

                if (Message.Length + 7 > Message.Offset)
                {
                    if (Message.Device.Connected)
                    {
                        Args.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                        if (!Message.Device.Socket.SendAsync(Args))
                        {
                            this.ProcessSend(Message, Args);
                        }
                    }
                    else
                    {
                        this.OnSendCompleted(null, Args);
                        this.Disconnect(Message.Device.Token.Args);
                    }
                }
                else
                {
                    this.OnSendCompleted(null, Args);
                }
            }
            else
            {
                this.OnSendCompleted(null, Args);
                this.Disconnect(Message.Device.Token.Args);
            }
        }

        internal void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.DisconnectReuseSocket)
            {
                this.WritePool.Push(AsyncEvent);
            }
            else
            {
                AsyncEvent.Dispose();
                AsyncEvent = null;
            }
        }
        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent == null) return;

            Token Token = (Token)AsyncEvent.UserToken;

            if (Token.Aborting)
            {
                return;
            }

            Token.Aborting = true;

            Token.Device.Dispose();
            Token.Dispose();

            if (AsyncEvent.DisconnectReuseSocket)
            {
                this.ReadPool.Push(AsyncEvent);
            }
            else
            {
                AsyncEvent.Dispose();
                AsyncEvent = null;
            }
        }
    }
}
