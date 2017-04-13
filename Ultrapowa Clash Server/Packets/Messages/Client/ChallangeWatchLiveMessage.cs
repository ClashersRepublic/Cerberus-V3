using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    internal class ChallangeWatchLiveMessage : Message
    {
        public ChallangeWatchLiveMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, level).Send();
        }
    }
}
