using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.Packets.Messages.Server;

namespace UCS.Core.Network
{
    [Obsolete]
	internal static class PacketManager
	{
		public static void Receive(this Message p)
		{
            p.Decrypt();
			p.Decode();
			p.Process(p.Client.GetLevel());
		}

        //[Obsolete]
        //public static void Send(this Message p)
        //{
        //    try
        //    {
        //        p.Encode();
        //        if (p.GetMessageType() == 20000)
        //        {
        //            byte[] sessionKey = ((RC4SessionKey)p).Key;
        //            p.Client.UpdateKey(sessionKey);
        //        }

        //        var data = p.GetRawData();
        //        p.Process(p.Client.GetLevel());
        //        p.Client.Socket.BeginSend(data, 0, data.Length, 0, SendCallback, p);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        private static void SendCallback(IAsyncResult ar)
        {
            var message = (Message)ar.AsyncState;
            var socket = message.Client.Socket;
            try
            {               
                var error = SocketError.Success;
                var transferred = socket.EndReceive(ar, out error);
                if (transferred == 0 || error != SocketError.Success)
                {
                    ResourcesManager.DropClient(socket.Handle.ToInt64());
                    return;
                }

                var data = message.GetRawData();
                if (transferred < data.Length)
                    socket.BeginSend(data, transferred, data.Length - transferred, SocketFlags.None, SendCallback, message);
            }
            catch (Exception)
            {
                ResourcesManager.DropClient(socket.Handle.ToInt64());
                Logger.Error("Exception while handling send callback.");
            }
        }
    }
}
