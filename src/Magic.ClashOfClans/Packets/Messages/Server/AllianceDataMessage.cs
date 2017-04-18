using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class AllianceDataMessage : Message
    {
        private readonly Alliance m_vAlliance;

        public AllianceDataMessage(PacketProcessing.Client client, Alliance alliance) : base(client)
        {
            MessageType = 24301;
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var allianceMembers = m_vAlliance.AllianceMembers;
            data.AddRange(m_vAlliance.EncodeFullEntry());
            data.AddString(m_vAlliance.AllianceDescription);
            data.AddInt32(0);
            data.Add((byte)0);
            data.AddInt32(0);
            data.Add((byte)0);
            data.AddInt32(allianceMembers.Count);
            foreach (AllianceMemberEntry allianceMemberEntry in allianceMembers)
            {
                data.AddRange(allianceMemberEntry.Encode());
            }

            data.AddInt32(0);
            data.AddInt32(32);
            Encrypt(data.ToArray());
        }
    }
}