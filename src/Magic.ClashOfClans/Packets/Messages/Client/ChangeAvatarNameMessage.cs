using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class ChangeAvatarNameMessage : Message
    {
        private string PlayerName { get; set; }

        public ChangeAvatarNameMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                PlayerName = br.ReadString();
            }
        }

        public override void Process(Level level)
        {
            if (string.IsNullOrEmpty(PlayerName) || PlayerName.Length > 15)
            {
                ResourcesManager.DisconnectClient(Client);
            }
            else
            {
                level.GetPlayerAvatar().SetName(PlayerName);
                new AvatarNameChangeOkMessage(Client)
                {
                    AvatarName = level.GetPlayerAvatar().GetAvatarName()
            }.Send();
            }
        }
    }
}