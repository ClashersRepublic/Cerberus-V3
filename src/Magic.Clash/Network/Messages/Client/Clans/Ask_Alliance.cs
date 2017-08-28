using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server.Clans;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Ask_Alliance : Message
    {
        internal long ClanID;

        public Ask_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            ClanID = Reader.ReadInt64();
        }

        public override void Process()
        {
            new Alliance_Data(Device) {Clan = ObjectManager.GetAlliance(ClanID)}.Send();
        }
    }
}