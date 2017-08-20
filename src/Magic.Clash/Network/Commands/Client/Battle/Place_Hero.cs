using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    internal class Place_Hero : Command
    {
        internal int GlobalId;
        internal int X;
        internal int Y;
        internal int Tick;

        internal Heroes Hero;

        public Place_Hero(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            X = Reader.ReadInt32();
            Y = Reader.ReadInt32();
            GlobalId = Reader.ReadInt32();
            Hero = CSV.Tables.Get(Gamefile.Heroes).GetDataWithID(GlobalId) as Heroes;

            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.State == State.IN_PC_BATTLE)
            {
                if (!Device.Player.Avatar.Variables.IsBuilderVillage &&
                    !Device.Player.Avatar.Modes.IsAttackingOwnBase)
                {
                }
            }
            else if (Device.State == State.IN_1VS1_BATTLE)
            {
            }
        }
    }
}