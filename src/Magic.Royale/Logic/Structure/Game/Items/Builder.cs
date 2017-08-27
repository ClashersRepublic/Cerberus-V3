using System;
using System.Collections.Generic;

namespace Magic.Royale.Logic.Structure.Game.Items
{
    internal class Builder
    {
        internal readonly int Size;
        internal int Gold = 0;
        internal int Gems = 0;

        internal readonly List<CardStack[]> Cards = new List<CardStack[]>();

        internal Builder(int optionSize)
        {
            Size = optionSize;
        }

        public void Add(CardStack[] option)
        {
            if (option.Length != Size)
                throw new Exception("Option length is not right.");

            Cards.Add(option);
        }

        public OpeningChest Build()
        {
            Cards.Sort(
                (a, b) => (a[0].Card.Rarity.SortCapacity * a[0].Count).CompareTo(
                    b[0].Card.Rarity.SortCapacity * a[0].Count));
            return new OpeningChest(Cards, Gold, Gems);
        }
    }
}
