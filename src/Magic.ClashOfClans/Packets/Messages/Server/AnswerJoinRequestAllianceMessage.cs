﻿using System.Collections.Generic;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 24317
    internal class AnswerJoinRequestAllianceMessage : Message
    {
        public AnswerJoinRequestAllianceMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24317;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            Encrypt(pack.ToArray());
        }
    }
}