namespace CR.Servers.CoC.Core.Network
{
    using System.Collections.Concurrent;
    using System.Net.Sockets;

    internal class SocketAsyncEventArgsPool : ConcurrentStack<SocketAsyncEventArgs>
    {
        internal SocketAsyncEventArgsPool(bool sendPool)
        {
            for (int i = 0; i < (sendPool ? Constants.MaxSends : Constants.MaxPlayers); i++)
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
            if (this.TryPop(out SocketAsyncEventArgs asyncEvent))
            {
                return asyncEvent;
            }

            return null;
        }

        internal void Enqueue(SocketAsyncEventArgs asyncEvent)
        {
            asyncEvent.AcceptSocket = null;
            asyncEvent.RemoteEndPoint = null;
            asyncEvent.UserToken = null;

            if (asyncEvent.DisconnectReuseSocket)
            {
                this.Push(asyncEvent);
            }
        }
    }
}