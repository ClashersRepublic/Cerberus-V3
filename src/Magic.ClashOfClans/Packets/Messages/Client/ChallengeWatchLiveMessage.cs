using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeWatchLiveMessage : Message
    {
        public ChallengeWatchLiveMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            // Space
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, level).Send();
        }
    }
}
