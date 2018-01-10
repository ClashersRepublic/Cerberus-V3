namespace CR.Servers.CoC.Logic.Battles
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Packets;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;

    internal class DuelBattle
    {
        internal Battle Battle1;
        internal Battle Battle2;

        internal DuelBattle(Battle battle1, Battle battle2)
        {
            this.Battle1 = battle1;
            this.Battle2 = battle2;
        }

        internal Battle GetBattle(Level level)
        {
            if (level == this.Battle1.Attacker)
            {
                return this.Battle1;
            }

            return this.Battle2;
        }

        internal void HandleCommands(int subTick, List<Command> commands, Device device)
        {
            Battle battle = this.GetBattle(device.GameMode.Level);

            if (battle != null)
            {
                battle.HandleCommands(subTick, commands);
            }

            this.SendDuelBattleInfoMessage();
        }

        internal void SendDuelBattleInfoMessage()
        {
        }
    }
}