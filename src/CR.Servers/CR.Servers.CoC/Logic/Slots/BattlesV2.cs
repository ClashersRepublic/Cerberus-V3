namespace CR.Servers.CoC.Logic.Slots
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using CR.Servers.CoC.Logic.Battle;
    using CR.Servers.CoC.Logic.Battle.Slots;

    internal class BattlesV2 : ConcurrentDictionary<long, Battles_V2>
    {
        internal int Seed = 1;

        internal List<Level> Waiting;

        public BattlesV2()
        {
            this.Waiting = new List<Level>();
        }

        internal void Remove(long BattleID)
        {
            if (this.ContainsKey(BattleID))
            {
            }
        }

        internal void Enqueue(Level Player)
        {
            lock (this.Waiting)
            {
                this.Waiting.Add(Player);
            }
        }

        internal void Dequeue(Level Player)
        {
            lock (this.Waiting)
            {
                this.Waiting.Remove(Player);
            }
        }

        internal Level Dequeue()
        {
            Level _Player = null;

            lock (this.Waiting)
            {
                _Player = this.Waiting[0];
                this.Waiting.RemoveAt(0);
            }

            return _Player;
        }

        internal Battle_V2 GetEnemy(long BattleID, long UserID)
        {
            if (this.ContainsKey(BattleID))
            {
                return this[BattleID].Player1.Player.UserId == UserID ? this[BattleID].Battle1 : this[BattleID].Battle;
            }
            return null;
        }

        internal Battle_V2 GetPlayer(long BattleID, long UserID)
        {
            if (this.ContainsKey(BattleID))
            {
                return this[BattleID].Player1.Player.UserId == UserID ? this[BattleID].Battle : this[BattleID].Battle1;
            }
            return null;
        }
    }
}