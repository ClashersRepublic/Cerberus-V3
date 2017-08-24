using System.Collections.Generic;
using Magic.Royale.Network.Messages.Client;

namespace Magic.Royale.Network.Messages.Server
{
    // Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(Device device) : base(device)
        {
            Identifier = 20108;
        }
    }
}