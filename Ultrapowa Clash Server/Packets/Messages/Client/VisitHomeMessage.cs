using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class VisitHomeMessage : Message
    {
        internal long AvatarId;

        public VisitHomeMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                AvatarId = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            VisitHomeMessage visitHomeMessage = this;
            try
            {
                Level player = ResourcesManager.GetPlayer(visitHomeMessage.AvatarId, false);
                player.Tick();
                new VisitedHomeDataMessage(visitHomeMessage.Client, player, level).Send();
                if (level.GetPlayerAvatar().GetAllianceId() <= 0L)
                    return;
                    Alliance alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
                if (alliance == null)
                    return;
                    new AllianceStreamMessage(Client, alliance).Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
