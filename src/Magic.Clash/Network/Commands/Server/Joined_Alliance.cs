using System;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Server
{
    internal class Joined_Alliance : Command
    {
        internal Clan Clan;

        public Joined_Alliance(Device _Client) : base(_Client)
        {
            Identifier = 1;
        }

        public Joined_Alliance(Device _Client, Clan Clan) : base(_Client)
        {
            Identifier = 1;
            this.Clan = Clan;
        }

        public override void Encode()
        {
            Data.AddRange(Clan.ToBytesHeader());
        }

        public override void Decode()
        {
            Debug();
        }
    }
}