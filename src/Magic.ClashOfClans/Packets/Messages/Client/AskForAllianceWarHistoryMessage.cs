using System;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AskForAllianceWarHistoryMessage : Message
    {
        private long AllianceID { get; set; }

        private long WarID { get; set; }

        public AskForAllianceWarHistoryMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                AllianceID = br.ReadInt64();
                WarID = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            AskForAllianceWarHistoryMessage warHistoryMessage = this;
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
                new AllianceWarHistoryMessage(Client, alliance).Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}