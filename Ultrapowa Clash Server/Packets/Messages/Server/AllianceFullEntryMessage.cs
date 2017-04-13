using System.Collections.Generic;
using System.Threading.Tasks;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AllianceFullEntryMessage : Message
    {
       public readonly Alliance m_vAlliance;

        public AllianceFullEntryMessage(PacketProcessing.Client client, Alliance alliance) : base(client)
        {
            SetMessageType(24324);
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            var allianceMembers = m_vAlliance.GetAllianceMembers();
            pack.AddString(m_vAlliance.GetAllianceDescription());
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.Add((byte) 0);
            pack.Add((byte) 0);
            pack.AddInt32(0);
            pack.AddRange((IEnumerable<byte>) m_vAlliance.EncodeFullEntry());

            Encrypt(pack.ToArray());
        }
    }
}
