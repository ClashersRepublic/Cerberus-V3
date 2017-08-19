using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Messages.Server.Battle
{
    internal class Npc_Data : Message
    {
        internal int Npc_ID = 0;
        internal Level Avatar;

        public Npc_Data(Device _Device) : base(_Device)
        {
            Identifier = 24133;
        }

        public override void Encode()
        {
            var Home = new Objects(Avatar = Device.Player, NPC.Levels[Npc_ID]);

            Data.AddInt(0);
            Data.AddRange(Home.ToBytes);
            Data.AddRange(Device.Player.Avatar.ToBytes);

            Data.AddInt(Npc_ID);
            Data.AddByte(0);
        }

        public override void Process()
        {
            Device.State = State.IN_NPC_BATTLE;
        }
    }
}