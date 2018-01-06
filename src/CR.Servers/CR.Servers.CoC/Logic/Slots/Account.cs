namespace CR.Servers.CoC.Logic
{
    using System;

    internal class Account
    {
        internal bool InBattle;

        internal int HighId;
        internal int LowId;

        internal Home Home;
        internal Device Device;
        internal Player Player;
        internal Account DefenseAccount;

        internal DateTime StartBattleTime;

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