namespace CR.Servers.CoC.Logic.Battle.Slots.Items
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Replay_Stats
    {
        [JsonProperty] internal int AllianceBadge = -1;

        [JsonProperty] internal int AllianceBadge2 = -1;

        [JsonProperty] internal string AllianceName = string.Empty;

        [JsonProperty] internal bool AllianceUsed;

        [JsonProperty] internal int ArmyDeploymentPercentage;

        [JsonProperty] internal int AttackerScore;

        [JsonProperty] internal int AttackerStars;

        [JsonProperty] internal bool BattleEnded;

        [JsonProperty] internal int BattleTime;

        [JsonProperty] internal int DefenderScore;

        [JsonProperty] internal int DeployedHousingSpace;

        [JsonProperty] internal int DestructionPercentage;

        [JsonProperty] internal int[] HomeId = {0, 0};

        [JsonProperty] internal int OriginalAttackerScore;

        [JsonProperty] internal int OriginalDefenderScore;
        [JsonProperty] internal bool TownHallDestroyed;

        internal JObject Save()
        {
            JObject Json = new JObject
            {
                {"townhallDestroyed", this.TownHallDestroyed},
                {"battleEnded", this.BattleEnded},
                {"allianceUsed", this.AllianceUsed},
                {"destructionPercentage", this.DestructionPercentage},
                {"battleTime", this.BattleTime},
                {"attackerScore", this.AttackerScore},
                {"defenderScore", this.DefenderScore},
                {"originalAttackerScore", this.OriginalAttackerScore},
                {"originalDefenderScore", this.OriginalAttackerScore},
                {"allianceName", this.AllianceName},
                {"attackerStars", this.AttackerStars},
                {"homeID", JArray.FromObject(this.HomeId)},
                {"allianceBadge", this.AllianceBadge},
                {"allianceBadge2", this.AllianceBadge2},
                {"deployedHousingSpace", this.DeployedHousingSpace},
                {"armyDeploymentPercentage", this.ArmyDeploymentPercentage}
            };

            return Json;
        }
    }
}