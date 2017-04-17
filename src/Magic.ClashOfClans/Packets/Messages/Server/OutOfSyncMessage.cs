using System.Collections.Generic;
using Magic.Helpers;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 24104
    internal class OutOfSyncMessage : Message
    {
        public OutOfSyncMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24104;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            Encrypt(data.ToArray());
        }
    }
}
