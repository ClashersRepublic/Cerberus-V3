using System;

namespace Magic.Network.Messages.Components
{
    public class DeckMessageComponent : MessageComponent
    {
        public byte Unknown1;

        public CardMessageComponent[] BattleCards;

        public override void ReadMessageComponent(MessageReader reader)
        {
            // Part of DeckMessageComponent?
            Unknown1 = reader.ReadByte(); // 255

            // Active cards.
            BattleCards = new CardMessageComponent[8];
            for (int i = 0; i < 8; i++)
            {
                var card = new CardMessageComponent();
                card.ReadMessageComponent(reader);

                BattleCards[i] = card;
            }
        }

        public override void WriteMessageComponent(MessageWriter writer)
        {
            writer.Write(Unknown1);

            for (int i = 0; i < BattleCards.Length; i++)
            {
                var card = BattleCards[i];

                card.WriteMessageComponent(writer);
            }
        }
    }
}
