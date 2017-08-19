using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Messages.Server.Battle;

namespace Magic.ClashOfClans.Network.Messages.Client.Battle
{
    internal class Attack_NPC : Message
    {
        internal int Npc_ID;

        public Attack_NPC(Device device, Reader reader) : base(device, reader)
        {
            // Attack_NPC.
        }

        public override void Decode()
        {
            Npc_ID = Reader.ReadInt32();
        }

        public override void Process()
        {
            new Npc_Data(Device) {Npc_ID = Npc_ID}.Send();
            var Index = Device.Player.Avatar.Npcs.FindIndex(N => N.NPC_Id == Npc_ID);

            if (Index < 0)
            {
                if (Npc_ID == 17000000)
                    Device.Player.Avatar.Mission_Finish(21000002);
                else if (Npc_ID == 17000001)
                    Device.Player.Avatar.Mission_Finish(21000009);

                Device.Player.Avatar.Npcs.Add(new Npc(Npc_ID));
            }
        }
    }
}
