using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 24318
    internal class AllianceStreamEntryRemovedMessage : Message
    {
        public AllianceStreamEntryRemovedMessage(PacketProcessing.Client client, int i) : base(client)
        {
            SetMessageType(24318);
            m_vId = i;
        }

        public int m_vId;

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(0);
            pack.AddInt32(m_vId);
            Encrypt(pack.ToArray());
        }
    }
}
