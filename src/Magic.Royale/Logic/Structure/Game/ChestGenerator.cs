using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Logic.Structure.Game.Items;

namespace Magic.Royale.Logic.Structure.Game
{
    internal class ChestGenerator
    {
        //Not working yet
        public OpeningChest GenerateChest(Avatar player, Chest chest, Random random)
        {
            var isDraft = chest.DraftChest;
            var builder = OpeningChest.Builder(isDraft);

            var common = Rarities.Get("Common");
            var rare = Rarities.Get("Rare");
            var epic = Rarities.Get("Epic");
            var legendary = Rarities.Get("Legendary");

            float rewardMultiplier = chest.Arena.ChestRewardMultiplier / 100f;
            int minimumSpellsCount = (int)(chest.RandomSpells * rewardMultiplier);
            int minimumDifferentSpells = (int)(chest.DifferentSpells * rewardMultiplier);
            Console.WriteLine($"minimumSpellsCount {minimumSpellsCount}");
            Console.WriteLine($"minimumDifferentSpells {minimumDifferentSpells}");

            Console.WriteLine($"chest.RareChance {chest.RareChance}");
            Console.WriteLine($"chest.EpicChance {chest.EpicChance}");

            Console.WriteLine($"chest.LegendaryChance {chest.LegendaryChance}");

            float minimumRare = (float)minimumSpellsCount / (float)chest.RareChance;
            float minimumEpic = (float)minimumSpellsCount / (float)chest.EpicChance;
            float minimumLegendary = (float)minimumSpellsCount / (float)chest.LegendaryChance;
            
            Console.WriteLine($"minimumRare {minimumRare}");
            Console.WriteLine($"minimumEpic {minimumEpic}");
            Console.WriteLine($"minimumLegendary {minimumLegendary}");    

            float rareCount = minimumRare + player.RareChance;
            float epicCount = minimumEpic + player.EpicChance;
            float legendaryCount = minimumLegendary + player.LegendaryChance;
            float commonCount = minimumSpellsCount - rareCount - epicCount - legendaryCount;
            Console.WriteLine($"rareCount {rareCount}");
            Console.WriteLine($"epicCount {epicCount}");
            Console.WriteLine($"legendaryCount {legendaryCount}");
            Console.WriteLine($"commonCount {commonCount}");

            int differentRare = CountDifferent(rareCount, minimumSpellsCount, minimumDifferentSpells);
            int differentEpic = CountDifferent(epicCount, minimumSpellsCount, minimumDifferentSpells);
            int differentLegendary = CountDifferent(legendaryCount, minimumSpellsCount, minimumDifferentSpells);
            int differentCommon = minimumDifferentSpells - differentRare - differentEpic - differentLegendary;

            Console.WriteLine($"differentRare {differentRare}");
            Console.WriteLine($"differentEpic {differentEpic}");
            Console.WriteLine($"differentLegendary {differentLegendary}");
            Console.WriteLine($"differentCommon {differentCommon}");

            int realSpellsCount = (int)(commonCount + rareCount + epicCount + legendaryCount);

            Dictionary<Rarity, List<Card>> candidates = Cards.Select(Arenas.Get("Arena9"));
            commonCount -= GenerateCards(builder, candidates[common], differentCommon, (int)commonCount, random);
            rareCount -= GenerateCards(builder, candidates[rare], differentRare, (int)rareCount, random);
            epicCount -= GenerateCards(builder, candidates[epic], differentEpic, (int)epicCount, random);
            legendaryCount -= GenerateCards(builder, candidates[legendary], differentLegendary, (int)legendaryCount, random);

            player.RareChance = rareCount;
            player.EpicChance = epicCount;
            player.LegendaryChance = legendaryCount;

            int minGold = chest.MinGoldPerCard * realSpellsCount;
            int maxGold = chest.MaxGoldPerCard * realSpellsCount;

            builder.Gold = minGold + random.Next(maxGold - minGold);
            builder.Gems = 0; // TODO:

            return builder.Build();
        }

        internal float GenerateCards(Builder builder, List<Card> candidates, int different, int count, Random random)
        {
            int first;
            if (different == 1)
            {
                first = count;
            }
            else
            {
                first = (int)Math.Ceiling((count / different) * 1f + random.NextDouble());
            }

            double current = first;
            double sum = 0;

            Console.WriteLine($"First {first}");

            Console.WriteLine($"Different {different}");
            Console.WriteLine($"Sum + current {sum + current}");
            Console.WriteLine($"Count {count}");


            while (different != 1 && sum + current <= count && candidates.Count > builder.Size * 2)
            {
                if (AddStack(builder, candidates, current, random))
                {
                    sum += current;
                    current *= 1f + random.NextDouble();
                    --different;
                }
            }

            if (sum < count)
            {
                current = count - sum;
                if (AddStack(builder, candidates, current, random))
                {
                    sum += current;
                    current *= 1f +  random.NextDouble();
                    --different;
                }
            }

            return (float)sum;
        }

        internal bool AddStack(Builder builder, List<Card> candidates, double count, Random random)
        {
            Console.WriteLine(candidates.Count);
            if (candidates.Count > builder.Size)
            {
                var stack = new CardStack[builder.Size];
                for (var i = 0; i < stack.Length; ++i)
                {
                    var index = random.Next(candidates.Count);
                    var candidate = candidates.ElementAt(index);

                    stack[i] = new CardStack(candidate, (int)count);
                }
                builder.Add(stack);

                return true;
            }

            return false;
        }

        internal int CountDifferent(float rarityCount, int count, int different)
        {
            return (int)Math.Ceiling(different * (rarityCount / count));
        }
    }
}
