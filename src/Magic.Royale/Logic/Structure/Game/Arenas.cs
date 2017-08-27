using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Core;
using Magic.Royale.Files;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Logic.Structure.Game.Items;

namespace Magic.Royale.Logic.Structure.Game
{
    internal static class Arenas
    {
        public const int SCID_HIGH = 54;
        private static bool _initialized;

        internal static List<Arena> _Arena = new List<Arena>();

        internal static void Initialize()
        {
            if (_initialized)
                return;

            var indexCounter = 0;

            var csv = CSV.Tables.Get(Gamefile.Arenas);
            foreach (var data in csv.Datas)
            {
                var info = (Files.CSV_Logic.Arenas) csv.GetData(data.Row.Name);
                var Arena = new Arena
                {
                    Index = indexCounter,
                    Name = info.Name,
                    Scid = new SCID(SCID_HIGH, indexCounter),
                    _Arena = info.Arena,
                    ChestArena = info.ChestArena,
                    IsInUse = info.IsInUse,
                    TrophyLimit = info.TrophyLimit,
                    DemoteTrophyLimit = info.DemoteTrophyLimit,
                    SeasonTrophyRequest = info.SeasonTrophyReset,
                    ChestRewardMultiplier = info.ChestRewardMultiplier,
                    ChestShopPriceMultiplier = info.ChestShopPriceMultiplier,
                    RequestSize = info.RequestSize,
                    MaxDonationCountCommon = info.MaxDonationCountCommon,
                    MaxDonationCountEpic = info.MaxDonationCountEpic,
                    MaxDonationCountRare = info.MaxDonationCountEpic,
                    DailyDonationCapacityLimit = info.DailyDonationCapacityLimit,
                    BattleRewardGold = info.BattleRewardGold,
                    ReleaseDate = info.ReleaseDate
                };
                _Arena.Add(Arena);
                indexCounter++;
            }

            Logger.SayInfo(indexCounter + " Arenas, loaded and stored in memory.");
            _initialized = true;
        }

        public static Arena Get(string name)
        {
            return _Arena.FirstOrDefault(arena => arena.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
