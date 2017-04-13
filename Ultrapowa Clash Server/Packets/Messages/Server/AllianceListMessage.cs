using System.Collections.Generic;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AllianceListMessage : Message
    {
        private System.Collections.Generic.List<Alliance> m_vAlliances;
        private string m_vSearchString;

        public AllianceListMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24310);
            m_vAlliances = new List<Alliance>();
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(m_vSearchString);
            pack.AddInt32(m_vAlliances.Count);
            foreach (Alliance vAlliance in m_vAlliances)
            {
                if (vAlliance != null)
                    pack.AddRange((IEnumerable<byte>) vAlliance.EncodeFullEntry());
            };
            Encrypt(pack.ToArray());
        }

        public void SetAlliances(List<Alliance> alliances)
        {
            m_vAlliances = alliances;
        }

        public void SetSearchString(string searchString)
        {
            m_vSearchString = searchString;
        }
    }
}
