using System.Collections.Generic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class AllianceJoinOkMessage : Message
    {
        public AllianceJoinOkMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24303;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            Encrypt(pack.ToArray());
        }
    }
}