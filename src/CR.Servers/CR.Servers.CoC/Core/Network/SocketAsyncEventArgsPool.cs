using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CR.Servers.CoC.Core.Network
{
    internal class SocketAsyncEventArgsPool : ConcurrentStack<SocketAsyncEventArgs>
    {
        internal SocketAsyncEventArgsPool(bool sendPool)
        {
            for (int i = 0; i < Constants.MaxPlayers; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();

                if (!sendPool)
                {
                    args.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                }

                args.DisconnectReuseSocket = true;

                this.Enqueue(args);
            }
        }

        internal SocketAsyncEventArgs Dequeue()
        {
            this.TryPop(out SocketAsyncEventArgs args);
            return args;
        }

        internal void Enqueue(SocketAsyncEventArgs args)
        {
            this.Push(args);
        }
    }
}
