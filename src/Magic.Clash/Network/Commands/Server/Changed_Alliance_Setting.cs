using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Server
{
    internal class Changed_Alliance_Setting : Command
    {
        internal Clan Clan;

        public Changed_Alliance_Setting(Device client) : base(client)
        {
            Identifier = 6;
        }

        public Changed_Alliance_Setting(Reader Reader, Device _Client, int Identifier) : base(Reader, _Client,
            Identifier)
        {
        }

        public override void Encode()
        {
            Data.AddLong(Clan.Clan_ID);
            Data.AddInt(Clan.Badge);
            Data.AddInt(Clan.Level);
            Data.AddInt(-1);
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