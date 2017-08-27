using System;
using Magic.Royale.Extensions.Binary;

namespace Magic.Royale.Network.Commands.Client
{
    internal class Change_Deck_Card : Command
    {
        internal int Tick_Start;
        internal int Tick_End;
        internal long AccountID;
        internal int CardIndex;
        internal int Slot1;
        internal int Slot2;

        public Change_Deck_Card(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Change_Deck_Card.
        }

        public override void Decode()
        {
            Tick_Start = Reader.ReadVInt();
            Tick_End = Reader.ReadVInt();
            AccountID = Reader.ReadVLong();
            CardIndex = Reader.ReadVInt();
            Slot1 = Reader.ReadVInt();
            Slot2 = Reader.ReadVInt();
            ShowValues();
        }

        public override void Process()
        {
            var avatar = Device.Player;

            //Code cause crash
           /* if (Slot1 > 0 || Slot1 <= 8)
                if (CardIndex != 63)
                {
                    var temp = avatar.Cards[CardIndex];
                    Console.WriteLine($"Deck index { avatar.Decks[avatar.Active_Deck].Cards[Slot1].Index}");
                    Console.WriteLine($"Temp index { temp.Index}");
                    var _Index = avatar.Cards.FindIndex(Card => Card.Index == avatar.Decks[avatar.Active_Deck].Cards[Slot1].Index);
                    Console.WriteLine($"Search valie {avatar.Cards[_Index].Index}");
                    avatar.Cards[CardIndex] = avatar.Decks[avatar.Active_Deck].SwapCard(Slot1 ,temp);
                    avatar.Cards[_Index] = temp; //Card thaat on move
                }
                else if (Slot2 != 0)
                    avatar.Decks[avatar.Active_Deck].SwapCard(Slot1, Slot2);*/
        }
    }
}
