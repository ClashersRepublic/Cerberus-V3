using System;
using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class OwnHomeDataMessage : Message
    {
        public Level Player;

        public OwnHomeDataMessage(PacketProcessing.Client client, Level level) : base(client)
        {
            SetMessageType(24101);
            Player = level;
        }

        public override void Encode()
        {
            ClientAvatar avatar = Player.GetPlayerAvatar();
            var data = new List<byte>();
            ClientHome clientHome = new ClientHome(avatar.GetId());

            clientHome.SetShieldTime(avatar.RemainingShieldTime);
            clientHome.SetHomeJSON(Player.SaveToJSON());

            data.AddInt32(0);
            data.AddInt32(-1);
            data.AddInt32((int)Player.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            data.AddRange((IEnumerable<byte>)clientHome.Encode());
            data.AddRange((IEnumerable<byte>)avatar.Encode());
            if (avatar.State == ClientAvatar.UserState.Editmode)
            {
                data.AddInt32(1);
            }
            else
            {
                data.AddInt32(0);
            }
            data.AddInt32(0);
            data.AddInt64(0);
            data.AddInt64(0);
            data.AddInt64(0);

            Encrypt(data.ToArray());
        }
    }
}
