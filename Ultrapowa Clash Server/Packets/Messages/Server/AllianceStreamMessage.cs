using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AllianceStreamMessage : Message
    {
        private readonly Alliance m_vAlliance;

        public AllianceStreamMessage(PacketProcessing.Client client, Alliance alliance) : base(client)
        {
            SetMessageType(24311);
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            System.Collections.Generic.List<UCS.Logic.StreamEntry.StreamEntry> list = this.m_vAlliance.GetChatMessages().ToList<UCS.Logic.StreamEntry.StreamEntry>();
            var pack = new List<byte>();
            var chatMessages = m_vAlliance.GetChatMessages().ToList();
            pack.AddInt32(0);
            pack.AddInt32(list.Count);
            int num = 0;

            foreach (UCS.Logic.StreamEntry.StreamEntry streamEntry in list)
            {
                    pack.AddRange((IEnumerable<byte>) streamEntry.Encode());
                ++num;
                if (num >= 150)
                    break;
            }
        }
    }
}
