using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;

namespace Magic.Packets.Messages.Client
{
    internal class AddClashFriendMessage : Message
    {
        public long FriendId;

        public AddClashFriendMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            FriendId = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            // Space
        }
    }
}
