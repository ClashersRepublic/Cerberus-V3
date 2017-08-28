using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_All_Stream_Entry : Message
    {
        internal Clan Alliance;

        internal Alliance_All_Stream_Entry(Device _Device) : base(_Device)
        {
            Identifier = 24311;
        }

        internal Alliance_All_Stream_Entry(Device _Device, Clan clan) : base(_Device)
        {
            Identifier = 24311;
            Alliance = clan;
        }

        public override void Encode()
        {
            Data.AddInt(0);
            Data.AddRange(Alliance != null
                ? Alliance.Chats.ToBytes
                : ObjectManager.GetAlliance(Device.Player.Avatar.ClanId).Chats.ToBytes);
        }
    }
}
