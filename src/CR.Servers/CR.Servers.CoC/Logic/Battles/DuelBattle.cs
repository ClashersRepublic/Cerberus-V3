namespace CR.Servers.CoC.Logic.Battles
{
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
    }
}