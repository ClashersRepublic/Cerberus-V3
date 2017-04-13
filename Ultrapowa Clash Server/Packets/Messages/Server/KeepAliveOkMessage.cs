using System.Collections.Generic;
using UCS.PacketProcessing.Messages.Client;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(PacketProcessing.Client client, KeepAliveMessage cka) : base(client)
        {
            SetMessageType(20108);
        }

        public override void Encode()
        {
            var data = new List<byte>();
            Encrypt(data.ToArray());
        }
    }
}