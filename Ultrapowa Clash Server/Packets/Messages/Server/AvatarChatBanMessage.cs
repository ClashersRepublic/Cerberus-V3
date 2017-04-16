using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic.StreamEntry;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AvatarChatBanMessage : Message
    {
        public int m_vCode = 86400;

        public AvatarChatBanMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(20118);
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vCode);

            Encrypt(data.ToArray());
        }

        public void SetBanPeriod(int code)
        {
            m_vCode = code;
        }
    }
}