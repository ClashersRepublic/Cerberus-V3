using System.Collections.Generic;
using Magic.Helpers;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class ServerErrorMessage : Message
    {
        internal string m_vErrorMessage;

        public ServerErrorMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24115;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddString(m_vErrorMessage);
            Encrypt(data.ToArray());
        }

        public void SetErrorMessage(string message)
        {
            m_vErrorMessage = message;
        }
    }
}
