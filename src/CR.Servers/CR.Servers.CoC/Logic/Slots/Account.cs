namespace CR.Servers.CoC.Logic
{
    using CR.Servers.CoC.Logic.Battles;

    internal class Account
    {
        internal Battle Battle;
        internal Device Device;
        internal DuelBattle DuelBattle;
        internal int HighId;

        internal Home Home;
        internal int LowId;
        internal Player Player;

        public Account(int HighID, int LowID, Player Player, Home Home)
        {
            this.HighId = HighID;
            this.LowId = LowID;
            this.Player = Player;
            this.Home = Home;
        }

        internal long AccountId
        {
            get
            {
                return ((long) this.HighId << 32) | (uint) this.LowId;
            }
        }
    }
}