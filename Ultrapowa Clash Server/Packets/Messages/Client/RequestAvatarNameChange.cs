using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class RequestAvatarNameChange : Message
    {
        public string PlayerName { get; set; }

        public byte Unknown1 { get; set; }

        public RequestAvatarNameChange(PacketProcessing.Client client, PacketReader br) : base(client, br)
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
            RequestAvatarNameChange avatarNameChange = this;
            try
            {
                Level player = ResourcesManager.GetPlayer(level.GetPlayerAvatar().GetId(), false);
                if (player == null)
                    return;
                if (avatarNameChange.PlayerName.Length > 15)
                {
                    ResourcesManager.DisconnectClient(avatarNameChange.Client);
                }
                else
                {
                    player.GetPlayerAvatar().SetName(avatarNameChange.PlayerName);
                    new AvatarNameChangeOkMessage(Client)
                    {
                        AvatarName = avatarNameChange.PlayerName
                    }.Send();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
