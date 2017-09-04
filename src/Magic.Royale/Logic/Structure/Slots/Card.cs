using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Card : List<Card_Item>
    {
        public Avatar Player;

        public Card()
        {
        }

        public Card(Avatar avatar)
        {
            Player = avatar;
        }

        public new void Add(Card_Item _Card)
        {
            if (Contains(_Card))
            {
                var _Index = FindIndex(Card => Card.Index == _Card.Index && Card.Type == _Card.Type);

                if (_Index > -1)
                    this[_Index].Count += _Card.Count;
                else
                    base.Add(_Card);
            }
            else
            {
                base.Add(_Card);
            }
        }

        public void Add(byte _Type, int _ID, int _Count, int _Level, byte _isNew)
        {
            var _Card = new Card_Item(_Type, _ID, _Count, _Level, _isNew);

            if (Contains(_Card))
            {
                var _Index = FindIndex(Card => Card.Index == _Card.Index && Card.Type == _Card.Type);

                if (_Index > -1)
                    this[_Index].Count += _Card.Count;
                else
                    base.Add(_Card);
            }
            else
            {
                base.Add(_Card);
            }
        }

        public Card_Item Get(int _ID)
        {
            var _Index = FindIndex(Card => Card.Index == _ID);
            if (_Index > -1)
                return this[_Index];

            return null;
        }

        public byte[] ToBytes()
        {
            var packet = new List<byte>();
            var cards = Player.Decks[Player.Active_Deck].Cards.ToArray();
            foreach (var card in ToArray().Where(t2 => cards.All(t1 => t2.Index != t1.Index)))
            {
                packet.AddVInt(card.Index); // Card ID
                packet.AddVInt(card.Level - 1); // Card Level
                packet.AddVInt(0); // Bought time
                packet.AddVInt(card.Count); // Card Count
                packet.AddVInt(0); // Unknown
                packet.AddVInt(0); // Unknown
                packet.AddVInt(card.Status); // New Card = 2
            }

            return packet.ToArray();
        }
    }
}
