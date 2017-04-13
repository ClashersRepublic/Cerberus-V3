﻿using System.Collections.Generic;
using UCS.Logic.StreamEntry;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 24312
    internal class AllianceStreamEntryMessage : Message
    {
        StreamEntry m_vStreamEntry;

        public AllianceStreamEntryMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24312);
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddRange(m_vStreamEntry.Encode());
            Encrypt(pack.ToArray());
        }

        public void SetStreamEntry(StreamEntry entry)
        {
            m_vStreamEntry = entry;
        }
    }
}