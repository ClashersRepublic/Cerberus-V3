namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;
    using CR.Servers.Logic.Enums;

    internal class Pc_Battle_Data_V2 : Message
    {
        internal Level Enemy;

        public Pc_Battle_Data_V2(Device Device) : base(Device)
        {
            this.Device.State = State.IN_1VS1_BATTLE;
        }

        internal override short Type => 25023;

        internal override void Encode()
        {
            this.Device.GameMode.Level.Player.Encode(this.Data);
            this.Data.AddLong(this.Enemy.Player.UserId);

            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);

            this.Data.AddCompressed(this.Enemy.BattleV2().ToString());
            this.Data.AddCompressed(GameEvents.Events_Json);
            this.Data.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");

            this.Data.AddLong(this.Enemy.Player.UserId);
            this.Data.AddHexa("592FA598".Replace(" ", ""));
        }
    }
}