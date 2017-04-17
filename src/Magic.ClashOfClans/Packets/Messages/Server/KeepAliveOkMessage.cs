using System.Collections.Generic;
using Magic.PacketProcessing.Messages.Client;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 20108;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            Encrypt(data.ToArray());
        }
    }
}