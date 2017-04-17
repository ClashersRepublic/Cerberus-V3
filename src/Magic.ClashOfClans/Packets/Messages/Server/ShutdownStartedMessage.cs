﻿using System.Collections.Generic;
using Magic.Helpers;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class ShutdownStartedMessage : Message
    {
        internal int m_vCode;

        public ShutdownStartedMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 20161;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vCode);
            Encrypt(data.ToArray());
        }

        public void SetCode(int code)
        {
            m_vCode = code;
        }
    }
}
