using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    internal class Hero_Rage : Command
    {
        internal int GlobalId;
        internal int Tick;

        public Hero_Rage(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            GlobalId = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.State == State.IN_PC_BATTLE)
            {
                if (!Device.Player.Avatar.Variables.IsBuilderVillage && !Device.Player.Avatar.Modes.IsAttackingOwnBase)
                {
                }
            }
            else if (Device.State == State.IN_1VS1_BATTLE)
            {
            }
        }
    }
}