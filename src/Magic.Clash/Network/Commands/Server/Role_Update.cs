using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Server
{
    internal class Role_Update : Command
    {
        internal Clan Clan = null;
        internal int Role;

        public Role_Update(Device _Client) : base(_Client)
        {
            Identifier = 8;
        }

        public override void Encode()
        {
            Data.AddLong(Clan.Clan_ID);
            Data.AddInt(Role);
            Data.AddInt(Role);
            Data.AddInt(0);
        }

        public override void Decode()
        {
            Debug();
        }
    }
}