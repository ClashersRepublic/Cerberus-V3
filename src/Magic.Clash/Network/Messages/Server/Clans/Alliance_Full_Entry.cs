using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Full_Entry : Message
    {
        internal Clan Clan;
        internal int ClanID = 0;

        public Alliance_Full_Entry(Device device) : base(device)
        {
            Identifier = 24324;
        }

        public Alliance_Full_Entry(Device Device, Clan clan) : base(Device)
        {
            Identifier = 24324;
            Clan = clan;
        }

        public override void Encode()
        {
            if (Clan == null)
                Clan = ObjectManager.GetAlliance(ClanID == 0 ? Device.Player.Avatar.ClanId : ClanID);

            Data.AddString(Clan.Description);
            Data.AddInt((int) WarState.NONE); //War state:

            Data.AddInt(0);

            Data.Add(0);
            //pack.AddLong(WarID);

            Data.Add(0);
            Data.AddInt(0);
            Data.AddRange(Clan.ToBytes);
        }
    }
}
