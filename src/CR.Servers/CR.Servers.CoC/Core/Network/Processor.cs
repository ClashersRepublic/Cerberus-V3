using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Packets;
using CR.Servers.Extensions;

namespace CR.Servers.CoC.Core.Network
{
    internal static class Processor
    {
        internal static void Send(this Message Message)
        {
            if (Message.Device.Connected)
            {
                Message.Encode();
                Message.Encrypt();
                Message.Process();

                Logging.Info(typeof(Processor), "Packet " + ConsolePad.Padding(Message.GetType().Name) + "    sent to    " + Message.Device.Socket.RemoteEndPoint + ".");

                Resources.Gateway.Send(Message);
            }
        }
    }
}