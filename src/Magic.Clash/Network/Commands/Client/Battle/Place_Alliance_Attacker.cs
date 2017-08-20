using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    internal class Place_Alliance_Attacker : Command
    {
        internal int GlobalId;
        internal int X;
        internal int Y;
        internal int Tick;

        public Place_Alliance_Attacker(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            X = Reader.ReadInt32();
            Y = Reader.ReadInt32();
            GlobalId = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.State == State.IN_PC_BATTLE)
            {
                if (!Device.Player.Avatar.Modes.IsAttackingOwnBase)
                {
                }
                Device.Player.Avatar.Castle_Units.Clear();
                Device.Player.Avatar.Castle_Used = 0;
            }
        }
    }
}
