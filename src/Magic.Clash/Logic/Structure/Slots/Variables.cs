using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Logic.Structure.Slots
{
    internal class Variables : List<Slot>
    {
        internal Variables()
        {
            // Variables.
        }

        internal Variables(bool Initialize)
        {
            if (Initialize)
                this.Initialize();
        }

        internal bool IsBuilderVillage => Get(Variable.VillageToGoTo) == 1;

        internal int Get(int Gl_ID)
        {
            var i = FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Variable Variables)
        {
            return Get(37000000 + (int) Variables);
        }

        internal void Set(int Global, int Count)
        {
            var i = FindIndex(R => R.Data == Global);

            if (i > -1)
                this[i].Count = Count;
            else
                Add(new Slot(Global, Count));
        }

        internal void Set(Variable Variables, int Count)
        {
            Set(37000000 + (int) Variables, Count);
        }

        internal byte[] ToBytes
        {
            get
            {
                var Packet = new List<byte>();

                foreach (var Variable in ToArray())
                {
                    Packet.AddInt(Variable.Data);
                    Packet.AddInt(Variable.Count);
                }

                return Packet.ToArray();
            }
        }

        internal void Initialize()
        {
            Set(Variable.AccountBound, 0);
            Set(Variable.BeenInArrangedWar, 0);
            Set(Variable.ChallengeLayoutIsWar, 0);
            Set(Variable.ChallengeStarted, 0);
            Set(Variable.EventUseTroop, 0);
            Set(Variable.FILL_ME, 0);
            Set(Variable.FriendListLastOpened, 0);
            Set(Variable.LootLimitCooldown, 0);
            Set(Variable.LootLimitFreeSpeedUp, 0);
            Set(Variable.LootLimitTimerEndSubTick, 0);
            Set(Variable.LootLimitTimerEndTimestamp, 0);
            Set(Variable.LootLimitWinCount, 0);
            Set(Variable.SeenBuilderMenu, 0);
            Set(Variable.StarBonusCooldown, 0);
            Set(Variable.StarBonusCounter, 0);
            Set(Variable.StarBonusTimerEndSubTick, 0);
            Set(Variable.StarBonusTimerEndTimestep, 0);
            Set(Variable.StarBonusTimesCollected, 0);
            Set(Variable.VillageToGoTo, 0);
            Set(Variable.Village2BarrackLevel, 0);
        }
    }
}