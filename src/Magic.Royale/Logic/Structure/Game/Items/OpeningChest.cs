using System.Collections.Generic;

namespace Magic.Royale.Logic.Structure.Game.Items
{
    internal class OpeningChest
    {
        internal readonly List<CardStack[]> Cards;
        internal readonly List<CardStack> SelectedCard;
        internal readonly int Gold, Gems;
        internal int CurrentCard;

        public OpeningChest(List<CardStack[]> cards, int gold, int gems)
        {
            Cards = cards;
            SelectedCard = new List<CardStack>(cards.Count);
            Gold = gold;
            Gems = gems;
        }

        public int Size()
        {
            return Cards.Count == 0 ? 0 : Cards[0].Length;
        }

        public CardStack Next(int selection)
        {
            CardStack card = null;
            if (HasCards())
            {
                SelectedCard.Insert(CurrentCard, card = Cards[CurrentCard][selection >= Size() ? 0 : selection]);
                ++CurrentCard;
            }

            return card;
        }

        public bool HasCards()
        {
            return CurrentCard < Cards.Count;
        }

        public void End()
        {
            while (Next(0) != null)
            {
            }
        }

        public static Builder Builder(bool draft)
        {
            return new Builder(draft ? 2 : 1);
        }
    }
}