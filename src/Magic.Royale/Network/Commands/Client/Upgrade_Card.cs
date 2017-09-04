using System;
using System.Linq;
using Discord;
using Magic.Royale.Extensions.Binary;
using Magic.Royale.Files;
using Magic.Royale.Files.CSV_Logic;
using Magic.Royale.Logic;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Logic.Structure;
using Magic.Royale.Logic.Structure.Game.Items;
using Cards = Magic.Royale.Logic.Structure.Game.Cards;
using Rarities = Magic.Royale.Logic.Structure.Game.Rarities;

namespace Magic.Royale.Network.Commands.Client
{
    internal class Upgrade_Card : Command
    {
        internal int Tick_Start;
        internal int Tick_End;
        internal long AccountID;
        internal SCID SCID;

        public Upgrade_Card(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Upgrade_Card.
        }

        public override void Decode()
        {
            Tick_Start = Reader.ReadVInt();
            Tick_End = Reader.ReadVInt();
            AccountID = Reader.ReadVLong();
            SCID = Reader.ReadSCID();
            ShowValues();
        }

        public override void Process()
        {
            foreach (var cardlist in Device.Player.Cards.ToList())
            {
                int Index = cardlist.FindIndex(_Card => _Card.Scid.Value == SCID.Value);

                if (Index > -1)
                {
                    var card = cardlist[Index];
                    var CardData = Cards.Get(SCID);
                    var rarity = CardData.Rarity;

                    int gold = rarity.UpgradeCost[card.Level - 1];
                    int cardstack = rarity.UpgradeMaterialCount[card.Level - 1];
                    int exp = rarity.UpgradeExp[card.Level - 1];

                    card.Level++;
                    card.Count -= cardstack;
                }
            }
        }
    }
}
