using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class ReportPlayerMessage : Message
    {
        public long ReportedPlayerID { get; set; }

        public int Tick { get; set; }

        public ReportPlayerMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                br.ReadInt32();
                ReportedPlayerID = br.ReadInt64();
                br.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            ReportPlayerMessage reportPlayerMessage = this;
            try
            {
                Level player = ResourcesManager.GetPlayer(ReportedPlayerID, false);
                ++player.GetPlayerAvatar().ReportedTimes;
                if (player.GetPlayerAvatar().ReportedTimes < 3)
                    return;
                AvatarChatBanMessage c = new AvatarChatBanMessage(Client);
                int code = 1800;
                c.SetBanPeriod(code);
                c.Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
