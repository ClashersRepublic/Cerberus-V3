using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Data : Message
    {
        internal Clan Clan;
        internal long ClanID = 0;

        public Alliance_Data(Device device) : base(device)
        {
            Identifier = 24301;
        }

        public Alliance_Data(Device Device, Clan clan) : base(Device)
        {
            Identifier = 24301;
            Clan = clan;
        }

        public override void Encode()
        {
            if (Clan == null)
                Clan = ObjectManager.GetAlliance(ClanID == 0 ? Device.Player.Avatar.ClanId : ClanID);


            if (Clan != null)
            {
                Data.AddRange(Clan.ToBytes);

                Data.AddString(Clan.Description);
                Data.AddInt(6);
                Data.AddBool(false);
                Data.AddInt(0);
                Data.AddByte(0);
                Data.AddRange(Clan.Members.ToBytes);
                Data.AddInt(0);
                Data.AddInt(0);
                Data.AddInt(32);
            }
            else
            {
                new Out_Of_Sync(Device).Send();
            }
        }
    }
}