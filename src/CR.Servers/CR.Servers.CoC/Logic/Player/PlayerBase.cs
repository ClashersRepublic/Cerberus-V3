using System;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic
{
    internal class PlayerBase
    {
        internal Level Level;

        [JsonProperty] internal int AllianceHighId;
        [JsonProperty] internal int AllianceLowId;

        [JsonProperty] internal DataSlots ResourceCaps;
        [JsonProperty] internal ResourceSlots Resources;

        [JsonProperty] internal DataSlots AchievementProgress;
        [JsonProperty] internal UnitSlots Units;
        [JsonProperty] internal UnitSlots Units2;
        [JsonProperty] internal UnitSlots Spells;
        [JsonProperty] internal DataSlots UnitUpgrades;
        [JsonProperty] internal AllianceUnitSlots AllianceUnits;
        [JsonProperty] internal DataSlots SpellUpgrades;
        [JsonProperty] internal DataSlots HeroUpgrades;

        [JsonProperty] internal DataSlots HeroStates;
        [JsonProperty] internal DataSlots HeroHealth;
        [JsonProperty] internal DataSlots HeroModes;

        [JsonProperty] internal NpcMapSlots NpcMapProgress;
        [JsonProperty] internal DataSlots NpcLootedGold;
        [JsonProperty] internal DataSlots NpcLootedElixir;
        [JsonProperty] internal VariableSlots Variables;

        [JsonProperty] internal int TownHallLevel;
        [JsonProperty] internal int TownHallLevel2;

        [JsonProperty] internal int CastleLevel = -1;
        [JsonProperty] internal int CastleTotalCapacity;
        [JsonProperty] internal int CastleUsedCapacity;
        [JsonProperty] internal int CastleTotalSpellCapacity;
        [JsonProperty] internal int CastleUsedSpellCapacity;

        [JsonProperty] internal int WallGroupId;

        internal long AllianceId=> (long)this.AllianceHighId << 32 | (uint)this.AllianceLowId;

        internal bool InAlliance => this.AllianceId > 0;

        //Testing
        internal bool Village2
        {
            get => this.Variables.GetCountByGlobalId(37000012) == 1;
            set => this.Variables.Set(37000012, Convert.ToInt32(value));
        }

        internal int Map
        {
            get => this.Variables.GetCountByGlobalId(37000012);
            set => this.Variables.Set(37000012, value);
        }

        internal int Gold
        {
            get => this.Resources.GetCountByGlobalId(3000001);
            set => this.Resources.Set(3000001, value);
        }

        internal int Elixir
        {
            get => this.Resources.GetCountByGlobalId(3000002);
            set => this.Resources.Set(3000002, value);
        }

        internal int DarkElixir
        {
            get => this.Resources.GetCountByGlobalId(3000003);
            set => this.Resources.Set(3000003, value);
        }

        internal int Gold2
        {
            get => this.Resources.GetCountByGlobalId(3000007);
            set => this.Resources.Set(3000007, value);
        }

        internal int Elixir2
        {
            get => this.Resources.GetCountByGlobalId(3000008);
            set => this.Resources.Set(3000008, value);
        }

        internal TownhallLevelData TownhallLevelData => (TownhallLevelData)CSV.Tables.Get(Gamefile.Townhall_Levels).GetDataWithInstanceID(this.TownHallLevel);

        internal TownhallLevelData TownhallLevel2Data => (TownhallLevelData)CSV.Tables.Get(Gamefile.Townhall_Levels).GetDataWithInstanceID(this.TownHallLevel2);

        internal int Checksum => this.Resources.Checksum
                                 + this.ResourceCaps.Checksum // OutOfSync
                                 + this.Units.Checksum
                                 + this.Spells.Checksum
                                 + this.AllianceUnits.Checksum
                                 + this.UnitUpgrades.Checksum
                                 + this.SpellUpgrades.Checksum
                                 + this.Units2.Checksum
                                 + this.TownHallLevel 
                                 + this.TownHallLevel2;

        internal virtual bool ClientPlayer => false;

        internal virtual bool NpcPlayer => false;

        public PlayerBase()
        {
            this.AchievementProgress = new DataSlots();
            this.ResourceCaps = new DataSlots();
            this.Resources = new ResourceSlots();
            this.Units = new UnitSlots();
            this.Units2 = new UnitSlots();
            this.Spells = new UnitSlots();
            this.UnitUpgrades = new DataSlots();
            this.SpellUpgrades = new DataSlots();
            this.HeroUpgrades = new DataSlots();
            this.AllianceUnits = new AllianceUnitSlots();

            this.HeroHealth = new DataSlots();
            this.HeroStates = new DataSlots();
            this.HeroModes = new DataSlots();

            this.NpcMapProgress = new NpcMapSlots();
            this.NpcLootedGold = new DataSlots();
            this.NpcLootedElixir = new DataSlots();

            this.Variables = new VariableSlots();
        }

        internal int GetUnitUpgradeLevel(Data Data)
        {
            return Data.GetDataType() == 4 ? this.UnitUpgrades.GetCountByData(Data) : this.SpellUpgrades.GetCountByData(Data);
        }

        internal void IncreaseUnitUpgradeLevel(Data Data)
        {
            if (Data.GetDataType() == 4)
            {
                this.UnitUpgrades.Add(Data, 1);
            }
            else
                this.SpellUpgrades.Add(Data, 1);
        }

        internal int GetAvailableResourceStorage(Data Resource)
        {
            return this.ResourceCaps.GetCountByData(Resource) - this.Resources.GetCountByData(Resource);
        }
    }
}
