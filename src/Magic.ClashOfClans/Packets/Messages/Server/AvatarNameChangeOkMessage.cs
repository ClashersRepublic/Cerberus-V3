using System.Collections.Generic;
using Magic.Helpers;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class AvatarNameChangeOkMessage : Message
    {
        internal string AvatarName;

        public AvatarNameChangeOkMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24111;
            AvatarName = "NoNameYet";
        }

        public override void Encode()
        {
            var pack = new List<byte>();

            pack.AddInt32(3);
            pack.AddString(AvatarName);
            pack.AddInt32(1);
            pack.AddInt32(-1);

            Encrypt(pack.ToArray());
        }
    }
}