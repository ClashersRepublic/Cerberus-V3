using System;
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

        public new void Add(Card _Card)
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
            var _Card = new Card(_Type, _ID, _Count, _Level, _isNew);

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

        public Card Get(int _ID)
        { 
            var _Index = FindIndex(Card => Card.Index == _ID);
            if (_Index > -1)
                return this[_Index];

            return null;
        }

        public byte[] ToBytes()
        {
            var _Packet = new List<byte>();
            var first8 = Player.Decks[Player.Active_Deck].Cards.ToList();
            foreach (var _Card in this.ToList().Where(x => first8.All(e => e.Index != x.Index)))
            {
                //Console.WriteLine("Crash");
                _Packet.AddVInt(_Card.Index); // Card ID
                _Packet.AddVInt(_Card.Level - 1); // Card Level
                _Packet.AddVInt(0); // Bought time
                _Packet.AddVInt(_Card.Count); // Card Count
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(_Card.Status); // New Card = 2
                _Packet.AddVInt(0); // Unknown
            }

            return _Packet.ToArray();
        }
    }
}
