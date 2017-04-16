using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class AskForAvatarProfileMessage : Message
    {
        private long m_vAvatarId;
        private long m_vCurrentHomeId;

        public AskForAvatarProfileMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            m_vAvatarId = Reader.ReadInt64();
            if (Reader.ReadBoolean())
                m_vCurrentHomeId = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            var player = ResourcesManager.GetPlayer(m_vAvatarId, false);
            if (player == null)
                return;

            player.Tick();
            new AvatarProfileMessage(Client)
            {
                Level = player
            }.Send();
        }
    }
}
