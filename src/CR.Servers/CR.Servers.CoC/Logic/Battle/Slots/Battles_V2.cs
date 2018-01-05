namespace CR.Servers.CoC.Logic.Battle.Slots
{
    internal class Battles_V2
    {
        internal Battle_V2 Battle;
        internal Battle_V2 Battle1;
        internal int BattleID;
        internal int Checksum;

        internal Level Player1;
        internal Level Player2;
        internal bool Started;
        internal int Tick;

        internal Timer Timer = new Timer();


        public Battles_V2(Level player1, Level player2)
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.Battle = new Battle_V2(player1, player2);
            this.Battle1 = new Battle_V2(player2, player1);
        }

        internal Level GetPlayer(long userId)
        {
            return this.Battle1.Attacker.UserId == userId ? this.Player1 : this.Player2;
        }

        internal Level GetEnemy(long userId)
        {
            return this.Battle1.Attacker.UserId != userId ? this.Player1 : this.Player2;
        }

        internal Battle_V2 GetPlayerBattle(long userId)
        {
            return this.Battle.Attacker.UserId == userId ? this.Battle : this.Battle1;
        }

        internal Battle_V2 GetEnemyBattle(long UserID)
        {
            return this.Battle1.Attacker.UserId != UserID ? this.Battle1 : this.Battle;
        }
    }
}