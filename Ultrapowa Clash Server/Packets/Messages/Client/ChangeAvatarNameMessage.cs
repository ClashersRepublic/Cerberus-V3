using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class ChangeAvatarNameMessage : Message
    {
        private string PlayerName { get; set; }

        public ChangeAvatarNameMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
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