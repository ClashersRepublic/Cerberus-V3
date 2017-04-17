using System.Collections.Generic;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class SetDeviceTokenMessage : Message
    {
        readonly Level level;

        public SetDeviceTokenMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 20113;
            level = client.Level;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(level.GetPlayerAvatar().GetUserToken());
            Encrypt(pack.ToArray());
        }
    }
}