using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic.Battle.Slots.Items
{
    internal class Replay_Info
    {
        [JsonProperty] internal List<int[]> Loot = new List<int[]>();

        [JsonProperty] internal List<int[]> AvailableLoot = new List<int[]>();

        [JsonProperty] internal List<int[]> Units = new List<int[]>();

        [JsonProperty] internal List<int[]> Spells = new List<int[]>();

        [JsonProperty] internal List<int[]> CastleUnit = new List<int[]>();

        [JsonProperty] internal List<int[]> Levels = new List<int[]>();

        [JsonProperty] internal Replay_Stats Stats = new Replay_Stats();

        internal void Add_Unit(int Data, int Count)
        {
            int Index = this.Units.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.Units[Index][1] += Count;
            else
                this.Units.Add(new[] { Data, Count });
        }

        internal void Add_Spell(int Data, int Count)
        {
            int Index = this.Spells.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.Spells[Index][1] += Count;
            else
                this.Spells.Add(new[] { Data, Count });
        }

        internal void Add_Level(int Data, int Count)
        {
            int Index = this.Levels.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.Levels[Index][1] += Count;
            else
                this.Levels.Add(new[] { Data, Count });
        }

        internal void Add_Available_Loot(int Data, int Count)
        {
            int Index = this.AvailableLoot.FindIndex(U => U[0] == Data);

            if (Index > -1)
                this.AvailableLoot[Index][1] += Count;
            else
                this.AvailableLoot.Add(new[] { Data, Count });
        }

        internal JObject Save()
        {
            var Json = new JObject
            {
                {"loot", JArray.FromObject(this.Loot)},
                {"units", JArray.FromObject(this.Units)},
                {"cc_units", JArray.FromObject(this.CastleUnit)},
                {"spells", JArray.FromObject(this.Spells)},
                {"levels", JArray.FromObject(this.Levels)},
                {"stats", this.Stats.Save() }
            };

            return Json;
        }
    }
}
