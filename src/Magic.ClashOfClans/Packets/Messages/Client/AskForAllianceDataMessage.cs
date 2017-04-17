using System;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AskForAllianceDataMessage : Message
    {
        private long m_vAllianceId;

        public AskForAllianceDataMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            m_vAllianceId = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance == null)
                return;

            new AllianceDataMessage(Client, alliance).Send();
        }
    }
}
