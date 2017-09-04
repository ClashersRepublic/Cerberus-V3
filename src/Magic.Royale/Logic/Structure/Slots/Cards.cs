using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Cards : List<Card>
    {
        public Avatar Player;

        public Cards()
        {
        }

        public Cards(Avatar avatar)
        {
            Player = avatar;
        }

        public void Add(Card_Item _Card)
        {
            foreach (var card in this.ToList())
            {
                if (card.Contains(_Card))
                {
                    var _Index = card.FindIndex(Card => Card.Index == _Card.Index && Card.Type == _Card.Type);

                    if (_Index > -1)
                        card[_Index].Count += _Card.Count;
                    else
                        card.Add(_Card);
                }
                else
                {
                    card.Add(_Card);
                }
            }
        }

        public void Add(byte _Type, int _ID, int _Count, int _Level, byte _isNew)
        {
            var _Card = new Card_Item(_Type, _ID, _Count, _Level, _isNew);
            foreach (var card in this.ToList())
            {
                if (card.Contains(_Card))
                {
                    var _Index = card.FindIndex(Card => Card.Index == _Card.Index && Card.Type == _Card.Type);

                    if (_Index > -1)
                        card[_Index].Count += _Card.Count;
                    else
                        card.Add(_Card);
                }
                else
                {
                    card.Add(_Card);
                }
            }
        }

        public byte[] ToBytes()
        {
            var packet = new List<byte>();
            var unactive = this[Player.Active_Deck].ToArray();
            var active = Player.Decks[Player.Active_Deck].Cards.ToArray();

            packet.AddVInt(unactive.Length - active.Length);

            foreach (var card in unactive.Where(t2 => active.All(t1 => t2.Index != t1.Index)))
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
