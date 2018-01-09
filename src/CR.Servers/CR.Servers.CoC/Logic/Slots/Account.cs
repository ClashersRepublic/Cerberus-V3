namespace CR.Servers.CoC.Logic
{
    using System;
    using CR.Servers.CoC.Logic.Battles;

    internal class Account
    {
        internal int HighId;
        internal int LowId;

        internal Home Home;
        internal Device Device;
        internal Player Player;
        internal Battle Battle;
        internal DuelBattle DuelBattle;
        
        public Account(int HighID, int LowID, Player Player, Home Home)
        {
            this.HighId = HighID;
            this.LowId = LowID;
            this.Player = Player;
            this.Home = Home;
        }

        internal long AccountId => ((long) this.HighId << 32) | (uint) this.LowId;
    }
}