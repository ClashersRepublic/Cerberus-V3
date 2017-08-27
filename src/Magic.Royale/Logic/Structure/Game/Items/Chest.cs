using System;
using System.Reflection;
using Magic.Royale.Core;
using Magic.Royale.Extensions;

namespace Magic.Royale.Logic.Structure.Game.Items
{
    internal class Chest : ICloneable
    {

        internal int Index;
        internal SCID Scid;

        internal Arena Arena;
        internal bool InShop, InArenaInfo, DraftChest;
        internal int TimeTakenDays, TimeTakesHours, TimeTakenMinutes, TimeTakenSeconds, RandomSpells, DifferentSpells, RareChance, EpicChance, LegendaryChance, MinGoldPerCard, MaxGoldPerCard;
        internal Card[] GuaranteedSpells;
        internal string Name;

        internal int Ticks => (((TimeTakenDays * 24 + TimeTakesHours) * 60 + TimeTakenMinutes) * 60 + TimeTakenSeconds) * 20;

        internal Chest Clone()
        {
            return this.MemberwiseClone() as Chest;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        internal void ShowValues()
        {
            Console.WriteLine(Environment.NewLine);

            foreach (var Field in GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                if (Field != null)
                    Logger.SayInfo(Utils.Padding(GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " +
                                   Utils.Padding(
                                       !string.IsNullOrEmpty(Field.Name)
                                           ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)")
                                           : "(null)", 40));
        }
    }
}
