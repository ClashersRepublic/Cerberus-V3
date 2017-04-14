using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UCS.PacketProcessing;
using UCS.Packets;

namespace UCS.Core.Network
{
    [Obsolete]
    internal class Token
    {
        internal Client Client;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;
        internal byte[] Buffer;
        internal int Offset;
        internal bool Aborting;

        internal Token(SocketAsyncEventArgs Args, Client Device)
        {
            this.Client = Device;
            this.Client.Token = this;
            this.Args = Args;
            this.Args.UserToken = (object)this;
            this.Buffer = new byte[2048];
            this.Packet = new List<byte>(2048);
        }

        internal void SetData()
        {
            byte[] numArray = new byte[this.Args.BytesTransferred];
            Array.Copy((Array)this.Args.Buffer, 0, (Array)numArray, 0, this.Args.BytesTransferred);
            this.Packet.AddRange((IEnumerable<byte>)numArray);
        }

        internal void Process()
        {
            this.Client.Process(this.Packet.ToArray());
        }

        internal void Reset()
        {
            this.Offset = 0;
            this.Packet.Clear();
        }
    }
}
