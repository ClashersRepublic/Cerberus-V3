using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Core.Network
{
	internal class Reader
	{
        internal delegate void IncomingReadHandler(Reader _Reader, byte[] _Buffer);

		internal readonly byte[] Buffer = new byte[1024];
        internal readonly IncomingReadHandler IReader;
        internal Socket Socket;

		public Reader(Socket socket, IncomingReadHandler readHandler)
		{
			this.Socket     = socket;
            this.IReader    = readHandler;
			this.Socket.BeginReceive(this.Buffer, 0, 1024, 0, OnReceive, this);
		}

        internal void OnReceive(IAsyncResult RAsync)
		{
			try
			{
				SocketError Error;
				int Received = Socket.EndReceive(RAsync, out Error);

				if (Error == SocketError.Success && Received > 0)
				{
					byte[] TBuffer = new byte[Received];
					Array.Copy(this.Buffer, 0, TBuffer, 0, Received);
					this.IReader(this, TBuffer);
					Socket.BeginReceive(this.Buffer, 0, 1024, 0, OnReceive, this);
				}
                else
                {
                    this.Socket.Close(5);
                }
			}
            catch (ObjectDisposedException)
            {
                // Alread disposed.
            }
			catch (Exception)
			{
                if (this.Socket != null)
                {
                    this.Socket.Close(5);
                }
			}
		}
	}
}
