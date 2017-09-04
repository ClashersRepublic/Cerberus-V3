using System;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Extensions.List;
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

        public Joined_Alliance(Reader Reader, Device _Client, int Identifier) : base(Reader, _Client, Identifier)
        {
        }

        public override void Encode()
        {
            Data.AddRange(Clan.ToBytesHeader());
            Data.AddInt(Device.Player.Avatar.Tick);
        }

        public override void Decode()
        {
            Reader.ReadInt64();
            Reader.ReadString();
            Reader.ReadInt32();
            Reader.ReadByte();
            Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();
        }
    }
}