using System;
using System.Collections.Generic;
using UCS.Logic;
using UCS.PacketProcessing;

namespace  UCS.PacketProcessing.Commands.Server
{
    internal class JoinedAllianceCommand : Command
    {
        private Alliance m_vAlliance;

        public JoinedAllianceCommand()
        {
        }

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddRange(m_vAlliance.EncodeHeader());
            return data.ToArray();
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

    }
}