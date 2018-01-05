using CR.Servers.CoC.Packets;
using CR.Servers.Extensions;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Core.Network
{
    internal static class Processor
    {
        internal static void Send(this Message Message)
        {
            if (Message.Device.State != State.DISCONNECTED)
            {
                if (Message.Device.Connected)
                {
                    Message.Encode();
                    Message.Encrypt();
                    Message.Process();

                    if (Message.Device.UseRC4)
                    {
                        
                    }

                    Logging.Info(typeof(Processor), "Packet " + ConsolePad.Padding(Message.GetType().Name) + "    sent to    " + Message.Device.Token.Socket.RemoteEndPoint + ".");

                    Resources.Gateway.Send(Message.ToBytes, Message.Device.Token.Socket);
                }
            }
        }
    }
}