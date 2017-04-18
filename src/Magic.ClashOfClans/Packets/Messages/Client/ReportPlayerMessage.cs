using System;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
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
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                br.ReadInt32();
                ReportedPlayerID = br.ReadInt64();
                br.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            var player = ResourcesManager.GetPlayer(ReportedPlayerID, false);
            ++player.Avatar.ReportedTimes;
            if (player.Avatar.ReportedTimes < 3)
                return;

            AvatarChatBanMessage c = new AvatarChatBanMessage(Client);
            int code = 1800;
            c.SetBanPeriod(code);
            c.Send();
        }
    }
}
