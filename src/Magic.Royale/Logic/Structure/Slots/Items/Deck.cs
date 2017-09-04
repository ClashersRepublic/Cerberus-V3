using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Extensions;
using Magic.Royale.Extensions.List;
using Newtonsoft.Json;

namespace Magic.Royale.Logic.Structure.Slots.Items
{
    internal class Deck
    {
        [JsonIgnore] public Avatar Player;
        public List<Deck_Card> Cards = new List<Deck_Card>(8);

        public const int DECK_CARDS_COUNT = 8;

        public Deck(Avatar avatar)
        {
            Player = avatar;
        }

        public void Add(int ID, SCID scid)
        {
            var _Card = new Deck_Card {Index = ID, Scid = scid};

            if (!Cards.Contains(_Card))
                Cards.Add(_Card);
        }

        public byte[] Get8FirstCard()
        {
            var _Packet = new List<byte>();

            foreach (var _Card in Cards.ToList().GetRange(0, 8))
            {
                var card = Player.Cards[Player.Active_Deck].Get(_Card.Index);
                _Packet.AddVInt(card.Index); // Card ID
                _Packet.AddVInt(card.Level - 1); // Card Level
                _Packet.AddVInt(0); // Bought time
                _Packet.AddVInt(card.Count); // Card Count
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(card.Status); // New Card = 2
                _Packet.AddVInt(0); // Unknown
            }

            return _Packet.ToArray();
        }

        public byte[] ToBytes()
        {
            var _Packet = new List<byte>();

            foreach (var _Card in Cards.ToList())
            {
                var card = Player.Cards[Player.Active_Deck].Get(_Card.Index);
                _Packet.AddVInt(_Card.Index); // Card ID
                _Packet.AddVInt(card.Level - 1); // Card Level
                _Packet.AddVInt(0); // Bought time
                _Packet.AddVInt(card.Count); // Card Count
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(card.Status); // New Card = 2
            }

            return _Packet.ToArray();
        }

        public byte[] Hand()
        {
            var _Packet = new List<byte>();

            var data = Cards.ToList().GetRange(0, 8);
            data.Shuffle();
            foreach (var _Card in data)
            {
                var card = Player.Cards[Player.Active_Deck].Get(_Card.Index);
                _Packet.AddVInt(card.Index); // Card ID
                _Packet.AddVInt(card.Level); // Card Level
            }

            return _Packet.ToArray();
        }

        public void SwapCard(int slot1, int slot2)
        {
            if (slot1 < 0 || slot1 >= 8)
                throw new NullReferenceException("slot1");
            if (slot2 < 0 || slot2 >= DECK_CARDS_COUNT)
                throw new NullReferenceException("slot2");

            var temp = Cards[slot1];
            Cards[slot1] = Cards[slot2];
            Cards[slot2] = temp;
        }

        public Card_Item SwapCard(int slot, Card_Item targetCard)
        {
            if (slot < 0 || slot >= DECK_CARDS_COUNT)
                throw new NullReferenceException("slot");

            var result = Cards[slot]; //Old card


            Cards[slot] = new Deck_Card {Index = targetCard.Index, Scid = targetCard.Scid}; //New card
            Console.WriteLine($"Cards Index {result.Index}");
            return Player.Cards[Player.Active_Deck].Get(result.Index); //Return old card
        }
    }
}
