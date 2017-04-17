using System;
using System.Collections.Generic;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class OwnHomeDataMessage : Message
    {
        public Level Player;

        public OwnHomeDataMessage(PacketProcessing.Client client, Level level) : base(client)
        {
            MessageType = 24101;
            Player = level;
        }

        public override void Encode()
        {
            var avatar = Player.GetPlayerAvatar();
            var data = new List<byte>();
            var home = new ClientHome(avatar.GetId());

            home.SetShieldTime(avatar.RemainingShieldTime);
            home.SetHomeJson(Player.SaveToJson());

            data.AddInt32(0);
            data.AddInt32(-1);
            data.AddInt32((int)Player.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            data.AddRange(home.Encode());
            data.AddRange(avatar.Encode());

            data.AddInt32(avatar.State == ClientAvatar.UserState.Editmode ? 1 : 0);

            data.AddInt32(0);
            data.AddInt64(0);
            data.AddInt64(0);
            data.AddInt64(0);

            Encrypt(data.ToArray());
        }
    }
}
