using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class AskForAllianceDataMessage : Message
    {
        private long m_vAllianceId;

        public AskForAllianceDataMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            m_vAllianceId = br.ReadInt64();
        }

        public override void Process(Level level)
        {
            AskForAllianceDataMessage allianceDataMessage = this;
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(allianceDataMessage.m_vAllianceId);
                if (alliance == null)
                    return;
                new AllianceDataMessage(allianceDataMessage.Client, alliance).Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
