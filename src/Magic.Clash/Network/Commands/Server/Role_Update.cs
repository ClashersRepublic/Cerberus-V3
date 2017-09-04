using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using System;
using System.Diagnostics;

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

        public Role_Update(Reader Reader, Device _Client, int Identifier) : base(Reader, _Client, Identifier)
        {
        }

        public override void Encode()
        {
            Data.AddLong(Clan.Clan_ID);
            Data.AddInt(Role);
            Data.AddInt(Role);
            Data.AddInt(Device.Player.Avatar.Tick);
        }

        public override void Decode()
        {
            Reader.ReadInt64();
            Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();

        }
    }
}