using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SpeedUp_Training : Command
    {
        internal int BuildingId;
        internal bool IsSpell;
        internal int Tick;

        public SpeedUp_Training(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            IsSpell = Reader.ReadBoolean();
            Tick = Reader.ReadInt32();
        }
    }

}
