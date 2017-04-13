using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AllianceDataMessage : Message
    {
        private readonly Alliance m_vAlliance;

        public AllianceDataMessage(PacketProcessing.Client client, Alliance alliance) : base(client)
        {
            SetMessageType(24301);
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            AllianceDataMessage allianceDataMessage = this;
            try
            {
                var pack = new List<byte>();
                var allianceMembers = allianceDataMessage.m_vAlliance.GetAllianceMembers();
                pack.AddRange((IEnumerable<byte>)allianceDataMessage.m_vAlliance.EncodeFullEntry());
                pack.AddString(allianceDataMessage.m_vAlliance.GetAllianceDescription());
                pack.AddInt32(0);
                pack.Add((byte)0);
                pack.AddInt32(0);
                pack.Add((byte)0);
                pack.AddInt32(allianceMembers.Count);
                foreach (AllianceMemberEntry allianceMemberEntry in allianceMembers)
                {
                    pack.AddRange(allianceMemberEntry.Encode());
                }

                pack.AddInt32(0);
                pack.AddInt32(32);
                Encrypt(pack.ToArray());
            }
            catch (Exception ex)
            {
            }
        }
    }
}