namespace Magic.Royale.Logic.Structure.Game.Items
{
    internal class CardStack
    {
        internal readonly Card Card;
        internal readonly int Count;

        public CardStack(Card card, int count)
        {
            Card = card;
            Count = count;
        }
    }
}
