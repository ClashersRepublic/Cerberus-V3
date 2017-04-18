using System.Collections.Generic;
using Magic.Helpers;
using Magic.Logic.StreamEntries;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class AvatarChatBanMessage : Message
    {
        public int m_vCode = 86400;

        public AvatarChatBanMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 20118;
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