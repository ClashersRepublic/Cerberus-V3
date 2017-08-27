using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Royale.Core;
using Magic.Royale.Files;
using Magic.Royale.Files.CSV_Logic;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Logic.Structure.Game.Items;

namespace Magic.Royale.Logic.Structure.Game
{
    internal class Chests
    {
        internal const int SCID_HIGH = 19;
        private static bool _initialized;
        internal static List<Chest> Chests_List = new List<Chest>();

        public static void Initialize()
        {
            if (_initialized)
                return;

            var chestIndex = 0;
            var csv = CSV.Tables.Get(Gamefile.Treasure_Chests);
            foreach (var data in csv.Datas)
            {
                Chest chest;
                var info = (Treasure_Chests) csv.GetData(data.Row.Name);

                if (string.IsNullOrEmpty(info.BaseChest))
                {
                    chest = new Chest();
                }
                else
                {
                    chest = Get(info.BaseChest);
                    if (chest == null)
                    {
                        chest = new Chest();
                        //Logger.SayInfo($"Base chest {info.BaseChest} is not found for chest {info.Name}'");
                    }
                    else
                    {
#if DEBUG
                        //Logger.SayInfo($"Base chest {info.BaseChest} is found for chest {info.Name}'");
#endif
                    }
                }

                chest.Index = chestIndex;
                chest.Scid = new SCID(SCID_HIGH, chestIndex++);
                chest.Name = info.Name;
                chest.Arena = Arenas.Get(string.IsNullOrEmpty(info.Arena) ? "" : info.Arena);
                chest.InShop = info.InShop;
                chest.InArenaInfo = info.InArenaInfo;
                chest.TimeTakenDays = info.TimeTakenDays;
                chest.TimeTakesHours = info.TimeTakenHours;
                chest.TimeTakenMinutes = info.TimeTakenMinutes;
                chest.TimeTakenSeconds = info.TimeTakenSeconds;
                chest.RandomSpells = info.RandomSpells;
                chest.DifferentSpells = info.RandomSpells;
                chest.RareChance = info.RareChance;
                chest.LegendaryChance = info.LegendaryChance;

                string[] GuaranteedSpells = info.GuaranteedSpells;

                chest.GuaranteedSpells = new Card[GuaranteedSpells.Length];

                for (int i = 0; i < GuaranteedSpells.Length; i++)
                {
                    chest.GuaranteedSpells[i] = Cards.Get(GuaranteedSpells[i]);
                }

                chest.MinGoldPerCard = info.MinGoldPerCard;
                chest.MaxGoldPerCard = info.MaxGoldPerCard;
                chest.DraftChest = info.DraftChest;

                Chests_List.Add(chest);
            }
            Logger.SayInfo(Chests_List.Count + " Chests, loaded and stored in memory.");
            _initialized = true;
        }

        public static Chest Get(string name)
        {
            return Chests_List.FirstOrDefault(chest => chest.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public static Chest Get(int index)
        {
            return Chests_List.FirstOrDefault(chest => chest.Index == index);
        }
    }
}
