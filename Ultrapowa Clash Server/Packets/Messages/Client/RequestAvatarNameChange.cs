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
        public string PlayerName;
        public byte Unknown1;

        public RequestAvatarNameChange(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            PlayerName = Reader.ReadString();
        }

        public override void Process(Level level)
        {
            Level player = ResourcesManager.GetPlayer(level.GetPlayerAvatar().GetId(), false);
            if (player == null)
                return;

            if (PlayerName.Length > 15)
            {
                ResourcesManager.DisconnectClient(Client);
            }
            else
            {
                player.GetPlayerAvatar().SetName(PlayerName);

                new AvatarNameChangeOkMessage(Client)
                {
                    AvatarName = PlayerName
                }.Send();
            }
        }
    }
}
