using System.Collections.Generic;
using Magic.Logic.AvatarStreamEntry;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 24412
    internal class AvatarStreamEntryMessage : Message
    {
        AvatarStreamEntry m_vAvatarStreamEntry;

        public AvatarStreamEntryMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24412;
        }

        public override void Encode()
        {
            var pack = new List<byte>();   
            pack.AddRange(m_vAvatarStreamEntry.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetAvatarStreamEntry(AvatarStreamEntry entry)
        {
            m_vAvatarStreamEntry = entry;
        }
    }
}