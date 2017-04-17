using System.Collections.Generic;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 20105
    internal class FriendListMessage : Message
    {
        public FriendListMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 20105;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddDataSlots(new List<DataSlot>());
            Encrypt(pack.ToArray());
        }
    }
}