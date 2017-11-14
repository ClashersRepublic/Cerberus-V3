using System;
using System.Collections.Generic;
using System.Net.Sockets;
using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Core.Network
{
    internal class Token : IDisposable
    {
        internal Device Device;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;

        internal byte[] Buffer;

        internal bool Aborting;
        internal bool Disposed;

        internal int Tries;

        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device = Device;
            this.Device.Token = this;

            this.Args = Args;
            this.Args.UserToken = this;

            this.Buffer = new byte[Constants.ReceiveBuffer];
            this.Packet = new List<byte>(Constants.ReceiveBuffer);
        }

        internal void SetData()
        {
            if (!this.Disposed)
            {
                byte[] Data = new byte[this.Args.BytesTransferred];
                Array.Copy(this.Args.Buffer, 0, Data, 0, this.Args.BytesTransferred);
                this.Packet.AddRange(Data);
            }

            this.Tries += 1;
        }

        internal void Process()
        {
            if (this.Tries > 10)
            {
                Resources.Gateway.Disconnect(this.Args);
            }
            else
            {
                this.Tries = 0;

                byte[] Data = this.Packet.ToArray();
                this.Device.Process(Data);
            }
        }

        public void Dispose()
        {
            this.Buffer = null;
            this.Packet = null;
            this.Device = null;

            this.Tries = 0;

            this.Disposed = true;
        }

        ~Token()
        {
            if (!this.Aborting)
            {
                Resources.Gateway.Disconnect(this.Args);
            }

            if (!this.Disposed)
            {
                this.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
