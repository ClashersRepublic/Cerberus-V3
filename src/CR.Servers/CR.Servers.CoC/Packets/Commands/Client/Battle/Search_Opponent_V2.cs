namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Battle.Manager;
    using CR.Servers.CoC.Logic.Battle.Slots;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class Search_Opponent_V2 : Command
    {
        internal int UnknownByte;

        internal int UnknownInt;

        public Search_Opponent_V2(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override int Type => 601;

        internal override void Decode()
        {
            this.UnknownInt = this.Reader.ReadInt32();
            this.UnknownByte = this.Reader.ReadByte();
            base.Decode();
        }

        internal override void Execute()
        {
            Level level = this.Device.GameMode.Level;
            if (Resources.BattlesV2.Waiting.Count == 0)
            {
                Resources.BattlesV2.Enqueue(level);

                this.Device.State = State.SEARCH_BATTLE;
            }
            else
            {
                Level enemy = Resources.BattlesV2.Dequeue();

                enemy.Player.BattleIdV2 = Resources.BattlesV2.Seed;
                level.Player.BattleIdV2 = Resources.BattlesV2.Seed;

                Battles_V2 battle = new Battles_V2(level, enemy);

                Resources.BattlesV2.TryAdd(Resources.BattlesV2.Seed++, battle);

                level.BattleManager = new BattleManager(level, Resources.BattlesV2.GetPlayer(this.Device.GameMode.Level.Player.BattleIdV2, this.Device.GameMode.Level.Player.UserId));
                enemy.BattleManager = new BattleManager(enemy, Resources.BattlesV2.GetEnemy(this.Device.GameMode.Level.Player.BattleIdV2, this.Device.GameMode.Level.Player.UserId));

                new Pc_Battle_Data_V2(this.Device) {Enemy = enemy}.Send();
                new Pc_Battle_Data_V2(enemy.GameMode.Device) {Enemy = level}.Send();

                new V2_Battle_Info(enemy.GameMode.Device, level).Send();
                new V2_Battle_Info(this.Device, enemy).Send();
            }
        }
    }
}