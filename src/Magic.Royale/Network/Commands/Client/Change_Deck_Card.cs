using System;
using Magic.Royale.Core;
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

            if (Slot1 > 0 || Slot1 <= 8)
                if (CardIndex == 63)
                {
                    avatar.Decks[avatar.Active_Deck].SwapCard(Slot1, Slot2);
                }
                else if (Slot2 == 63)
                {
                    var card = avatar.Decks[avatar.Active_Deck].Cards[Slot1].Index;
                    var card1 = CardIndex + 8;

                    var temp = avatar.Cards[avatar.Active_Deck][card1];

                    var _Index = avatar.Cards[avatar.Active_Deck].FindIndex(Card => Card.Index == card);
                    avatar.Cards[avatar.Active_Deck][card1] = avatar.Decks[avatar.Active_Deck].SwapCard(Slot1, temp);
                    avatar.Cards[avatar.Active_Deck][_Index] = temp;
                }
                else
                {
                    ExceptionLogger.Log(new InvalidOperationException(),
                        $"Shit when wrong at Change Deck Card as the neither card index {CardIndex} and Slot2 {Slot2} is 63");
                }
            else
                ExceptionLogger.Log(new InvalidOperationException(),
                    $"Shit when wrong at Change Deck Card as slot1 {Slot1} is higher than 8 or lower than 0");
        }
    }
}
