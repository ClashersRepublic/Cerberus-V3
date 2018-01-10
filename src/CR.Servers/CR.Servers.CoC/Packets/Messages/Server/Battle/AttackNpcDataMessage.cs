namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;
    using CR.Servers.Logic.Enums;

    internal class AttackNpcDataMessage : Message
    {
        internal int Npc_ID;

        internal Home NpcHome;

        internal bool Village2;

        public AttackNpcDataMessage(Device Device) : base(Device)
        {
            Device.State = State.IN_NPC_BATTLE;
        }

        internal override short Type => 24133;

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.NpcHome.Encode(this.Data);
            this.Device.GameMode.Level.Player.Encode(this.Data);

            this.Data.AddInt(this.Npc_ID);

            this.Data.AddBool(this.Village2);
        }
    }
}