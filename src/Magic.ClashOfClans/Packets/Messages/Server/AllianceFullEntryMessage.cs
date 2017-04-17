using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class AllianceFullEntryMessage : Message
    {
       public readonly Alliance m_vAlliance;

        public AllianceFullEntryMessage(PacketProcessing.Client client, Alliance alliance) : base(client)
        {
            MessageType = 24324;
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
