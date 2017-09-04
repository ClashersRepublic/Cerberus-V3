using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Commands.Server
{
    internal class Leaved_Alliance : Command
    {
        internal long AllianceID;
        internal int Reason;

        public Leaved_Alliance(Device _Client) : base(_Client)
        {
            Identifier = 2;
        }

        public Leaved_Alliance(Reader Reader, Device _Client, int Identifier) : base(Reader, _Client, Identifier)
        {
        }

        public override void Encode()
        {
            Data.AddLong(AllianceID);
            Data.AddInt(Reason); //1 = leave, 2 = kick (Not worth it to make an enum for this)
            Data.AddInt(Device.Player.Avatar.Tick);
        }

        public override void Decode()
        {
            Reader.ReadInt64();
            Reader.ReadInt32();
            Reader.ReadInt32();
        }
    }
}