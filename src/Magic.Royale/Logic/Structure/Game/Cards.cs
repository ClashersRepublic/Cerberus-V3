using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Core;
using Magic.Royale.Files;
using Magic.Royale.Files.CSV_Logic;
using Magic.Royale.Logic.Structure.Game.Items;

namespace Magic.Royale.Logic.Structure.Game
{
    internal static class Cards
    {
        internal static int TYPE_CHARACTER = 26;
        internal static int TYPE_BUILDING = 27;
        internal static int TYPE_SPELL = 28;
        private static bool _initialized;
        internal static List<Card> Card_List = new List<Card>();

        public static void Initialize()
        {
            if (_initialized)
                return;

            var indexCounter = 1;
            LoadCards(TYPE_CHARACTER, ref indexCounter);
            LoadCards(TYPE_BUILDING, ref indexCounter);
            LoadCards(TYPE_SPELL, ref indexCounter);

            Logger.SayInfo(indexCounter + " Cards, loaded and stored in memory.");
            _initialized = true;
        }

        internal static void LoadCards(int type, ref int indexCounter)
        {
            var cardIndex = 0;
            var csv = CSV.Tables.Get(type);
            foreach (var data in csv.Datas)
            {
                var card = new Card
                {
                    Type = type,
                    Index = indexCounter,
                    Scid = new SCID(type, cardIndex++)
                };

                if (type == TYPE_BUILDING)
                {
                    var info = (Spells_Buildings) csv.GetData(data.Row.Name);
                    card.Name = info.Name;
                    card.Rarity = Rarities.Get(info.Rarity);
                    card.ElixirCost = info.ManaCost;
                    card.UnlockArena = Arenas.Get(info.UnlockArena);
                    card.NotInUse = info.NotInUse;
                }
                else if (type == TYPE_CHARACTER)
                {
                    var info = (Spells_Characters) csv.GetData(data.Row.Name);
                    card.Name = info.Name;
                    card.Rarity = Rarities.Get(info.Rarity);
                    card.ElixirCost = info.ManaCost;
                    card.UnlockArena = Arenas.Get(info.UnlockArena);
                    card.NotInUse = info.NotInUse;
                }
                else if (type == TYPE_SPELL)
                {
                    var info = (Spells_Other) csv.GetData(data.Row.Name);
                    card.Name = info.Name;
                    card.Rarity = Rarities.Get(info.Rarity);
                    card.ElixirCost = info.ManaCost;
                    card.UnlockArena = Arenas.Get(info.UnlockArena);
                    card.NotInUse = info.NotInUse;
                }


                Card_List.Add(card);
                ++indexCounter;
            }
        }

        public static Card Get(string name)
        {
            return Card_List.FirstOrDefault(card => card.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public static Card Get(SCID scid)
        {
            return Card_List.FirstOrDefault(card => card.Scid.Value == scid.Value);
        }

        public static Dictionary<Rarity, List<Card>> Select(Arena arena)
        {
            var candidates = Rarities.Rarities_List.ToDictionary(rarity => rarity, rarity => new List<Card>());

            foreach (var card in Card_List)
                if (!card.NotInUse && card.UnlockArena._Arena <= arena._Arena)
                    candidates[card.Rarity].Add(card);

            return candidates;
        }
    }
}
