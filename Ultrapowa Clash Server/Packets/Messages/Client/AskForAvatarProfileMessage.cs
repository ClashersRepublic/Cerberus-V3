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

        public AskForAvatarProfileMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                m_vAvatarId = br.ReadInt64();
                if (!br.ReadBoolean())
                  return;
                m_vCurrentHomeId = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            AskForAvatarProfileMessage avatarProfileMessage = this;
            try
            {
                Level player = ResourcesManager.GetPlayer(avatarProfileMessage.m_vAvatarId, false);
                if (player == null)
                    return;
                player.Tick();
                new AvatarProfileMessage(avatarProfileMessage.Client)
                {
                    m_vLevel = player
                }.Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
