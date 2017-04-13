using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
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
            using (var br = new PacketReader(new MemoryStream(GetData())))
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
                Alliance alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
                new AllianceWarHistoryMessage(Client, alliance).Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}