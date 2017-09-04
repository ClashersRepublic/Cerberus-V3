using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using System;

namespace Magic.ClashOfClans.Network.Messages.Server.Authentication
{
    internal class Authentication_OK : Message
    {
        public Authentication_OK(Device client) : base(client)
        {
            Identifier = 20104;
            Device.State = State.LOGGED;
        }

        internal int ServerBuild;
        internal int ServerMajorVersion;
        internal int ContentVersion;

        public override void Encode()
        {
            var avatar = Device.Player.Avatar;
            Data.AddLong(avatar.UserId);
            Data.AddLong(avatar.UserId);

            Data.AddString(avatar.Token);

            Data.AddString(null);
            Data.AddString(null);

            Data.AddInt(ServerMajorVersion);
            Data.AddInt(ServerBuild);
            Data.AddInt(ContentVersion);

            Data.AddString("prod");

            Data.AddInt(avatar.Login_Count++); //Session Count
            Data.AddInt((int) avatar.PlayTime.TotalSeconds); //Playtime Second
            Data.AddInt(0);

            Data.AddString(null);

            Data.AddString(TimeUtils.ToJavaTimestamp(avatar.LastSave).ToString()); // 14 75 26 87 86 11 24 33
            Data.AddString(TimeUtils.ToJavaTimestamp(avatar.Created).ToString()); // 14 78 03 95 03 10 0

            Data.AddInt(0);
            Data.AddString(null);
            Data.AddString(avatar.Region);
            Data.AddString(null);
            Data.AddInt(1); //Unknown
            Data.AddString("https://www.clashersrepublic.com/events/");
            Data.AddString(
                "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/"); //Patch server?
            Data.AddString(null);
        }
    }
}
