using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Royale.Core;
using Magic.Royale.Files;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Logic.Structure.Game.Items;

namespace Magic.Royale.Logic.Structure.Game
{
    internal static class Rarities
    {
        private static bool _initialized = false;
        internal static List<Rarity> Rarities_List = new List<Rarity>();

        public static void Initialize()
        {
            if (_initialized)
                return;

            var csv = CSV.Tables.Get(Gamefile.Rarities);
            foreach (var data in csv.Datas)
            {
                var info = (Files.CSV_Logic.Rarities)csv.GetData(data.Row.Name);

                var Rarity = new Rarity
                {
                    Name = info.Name,
                    LevelCount = info.LevelCount,
                    DonateCapacity = info.DonateCapacity,
                    SortCapacity = info.SortCapacity,
                    DonateReward = info.DonateReward,
                    DonateXp = info.DonateXP,
                    GoldConversionValue = info.GoldConversionValue,
                    ChanceWeight = info.ChanceWeight,
                    UpgradeExp = info.UpgradeExp,
                    UpgradeMaterialCount = info.UpgradeMaterialCount,
                    UpgradeCost = info.UpgradeCost,
                    PowerLevelMultiplier = info.PowerLevelMultiplier
                };
                Rarities_List.Add(Rarity);
            }
            Logger.SayInfo(Rarities_List.Count + " Rarities, loaded and stored in memory.");
            _initialized = true;

        }

        public static Rarity Get(string name)
        {
            return Rarities_List.FirstOrDefault(rarity => rarity.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}