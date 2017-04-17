using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeVisitMessage : Message
    {
        public long AvatarID;

        public ChallengeVisitMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            AvatarID = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, level).Send();
        }
    }
}
