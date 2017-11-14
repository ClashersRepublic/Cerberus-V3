namespace CR.Servers.CoC.Logic
{
    internal class Account
    {
        internal int HighId;
        internal int LowId;

        internal Player Player;
        internal Home Home;

        public Account(int HighID, int LowID, Player Player, Home Home)
        {
            this.HighId = HighID;
            this.LowId = LowID;
            this.Player = Player;
            this.Home = Home;
        }

        internal long AccountId => (((long)this.HighId << 32) | (uint)this.LowId);
    }
}