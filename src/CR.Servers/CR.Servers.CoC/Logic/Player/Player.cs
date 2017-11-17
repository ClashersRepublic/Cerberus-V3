using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Player : PlayerBase
    {
        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;

        [JsonProperty] internal string Token;
        [JsonProperty] internal string Password;

        [JsonProperty] internal int LogSeed;

        [JsonProperty] internal bool RedPackageState;
        [JsonProperty] internal bool NameSetByUser;

        [JsonProperty] internal int ChangeNameCount;

        [JsonProperty] internal string Name = string.Empty;

        [JsonProperty] internal int ExpLevel = 1;
        [JsonProperty] internal int ExpPoints;
        [JsonProperty] internal int Diamonds;
        [JsonProperty] internal int FreeDiamonds;
        [JsonProperty] internal int League;
        [JsonProperty] internal int Score;
        [JsonProperty] internal int DuelScore;
        [JsonProperty] internal int Locale;
        [JsonProperty] internal int ClanWarPreference = 1;

        [JsonProperty] internal int Wins;
        [JsonProperty] internal int Loses;
        [JsonProperty] internal int Games;


        [JsonProperty] internal List<int> Tutorials = new List<int>();

        [JsonProperty] internal bool Locked;

        /*[JsonProperty] internal Rank Rank = Rank.Administrator;

        [JsonProperty] internal Facebook Facebook;
        [JsonProperty] internal Google Google;
        [JsonProperty] internal Gamecenter Gamecenter;*/

        [JsonProperty] internal DateTime LastTick = DateTime.UtcNow;
        [JsonProperty] internal DateTime Update = DateTime.UtcNow;
        [JsonProperty] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty] internal DateTime BanTime = DateTime.UtcNow;

        internal bool Connected => this.Level != null && this.Level.GameMode.Connected;

        internal long UserId => (((long)this.HighID << 32) | (uint) this.LowID);

        internal bool Banned => this.BanTime > DateTime.UtcNow;

        internal new int Checksum => this.ExpPoints
                                     + this.ExpLevel
                                     + this.Diamonds
                                     + this.FreeDiamonds
                                     + this.Score
                                     + this.DuelScore
                                     + (this.AllianceLowId > 0 ? 13 : 0)
                                     + base.Checksum;

        internal Player()
        {
            /*this.Achievements = new List<Data>(60);
            this.Missions = new List<Data>(30);
            this.Logs = new List<AvatarStreamEntry>(50);

            this.Facebook = new Facebook(this);
            this.Google = new Google(this);
            this.Gamecenter = new Gamecenter(this);*/
        }

        internal Player(Level Level, int HighID, int LowID) : this()
        {
            this.Level = Level;
            this.HighID = HighID;
            this.LowID = LowID;

            if (Extension.ParseConfigBoolean("Game:StartingResources:FetchFromCSV"))
            {
                this.Diamonds = Globals.StartingDiamonds;
                this.FreeDiamonds = Globals.StartingDiamonds;
            }
            else
            {
                this.Diamonds = Extension.ParseConfigInt("Game:StartingResources:Diamonds");
                this.FreeDiamonds = Extension.ParseConfigInt("Game:StartingResources:Diamonds");
            }

            this.Resources.Initialize();
            this.Variables.Initialize();
        }

        internal void AddDiamonds(int Diamonds)
        {
            this.Diamonds += Diamonds;
            this.FreeDiamonds += Diamonds;
        }

        internal void AddExperience(int ExpPoints)
        {
            var ExperienceData =
                (ExperienceLevelData) CSV.Tables.Get(Gamefile.Experience_Levels).GetDataWithID(this.ExpLevel - 1);

            this.ExpPoints += ExpPoints;

            if (this.ExpPoints >= ExperienceData?.ExpPoints)
            {
                this.ExpLevel++;
                this.ExpPoints -= ExperienceData.ExpPoints;
            }
        }

        /*internal void SetAlliance(Alliance Alliance, Member Member)
        {
            this.AllianceHighID = Alliance.HighID;
            this.AllianceLowID = Alliance.LowID;
            this.Alliance = Alliance;
            this.AllianceMember = Member;
        }*/
        internal void Encode(List<byte> _Packet)
        {
            _Packet.AddLong(this.UserId);
            _Packet.AddLong(this.UserId);

            _Packet.AddBool(false); //Clan
            _Packet.AddInt(0); //Legendary_Trophies
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);


            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);

            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0); //Builder base Versus Battle Win
            _Packet.AddInt(0); //5
            _Packet.AddInt(0); //8

            _Packet.AddInt(this.League);
            _Packet.AddInt(this.CastleLevel);
            _Packet.AddInt(this.CastleTotalCapacity);
            _Packet.AddInt(this.CastleUsedCapacity);
            _Packet.AddInt(this.CastleTotalSpellCapacity);
            _Packet.AddInt(this.CastleUsedSpellCapacity);

            _Packet.AddInt(this.TownHallLevel);
            _Packet.AddInt(this.TownHallLevel2);


            _Packet.AddString(this.Name);
            _Packet.AddString(null);

            _Packet.AddInt(this.ExpLevel);
            _Packet.AddInt(this.ExpPoints);
            _Packet.AddInt(this.Diamonds);
            _Packet.AddInt(this.FreeDiamonds);

            _Packet.AddInt(0); // 1200
            _Packet.AddInt(0); // 60

            _Packet.AddInt(this.Score);
            _Packet.AddInt(this.DuelScore);

            _Packet.AddInt(this.Wins);
            _Packet.AddInt(this.Loses);
            _Packet.AddInt(0); // Def Wins
            _Packet.AddInt(0); // Def Loses

            _Packet.AddInt(0); //WarGold
            _Packet.AddInt(0); //WarElixir
            _Packet.AddInt(0); //WarDarkElixir

            _Packet.AddInt(0);

            _Packet.AddBool(true);
            _Packet.AddInt(220);
            _Packet.AddInt(1828055880);

            _Packet.AddBool(this.NameSetByUser); //Name changed count

            _Packet.AddInt(this.ChangeNameCount); //Name Changed

            _Packet.AddInt(6900); //6900
            _Packet.AddInt(0);
            _Packet.AddInt(this.ClanWarPreference);

            _Packet.AddInt(0);
            _Packet.AddInt(0); // Total Attack with shield

            _Packet.AddBool(false); //0

            this.ResourceCaps.Encode(_Packet);
            this.Resources.Encode(_Packet);
            this.Units.Encode(_Packet);
            this.Spells.Encode(_Packet);

            this.UnitUpgrades.Encode(_Packet);
            this.SpellUpgrades.Encode(_Packet);
            this.HeroUpgrades.Encode(_Packet);

            this.HeroHealth.Encode(_Packet);
            this.HeroStates.Encode(_Packet);

            this.AllianceUnits.Encode(_Packet);

             _Packet.AddInt(Tutorials.Count);
             foreach (var Tutorial in Tutorials)
                 _Packet.AddInt(Tutorial);

            _Packet.AddInt(0); //Achievements
            _Packet.AddInt(0); //AchievementProgress

            this.NpcMapProgress.Encode(_Packet);
            this.NpcLootedGold.Encode(_Packet);
            this.NpcLootedElixir.Encode(_Packet);

            _Packet.AddInt(0);
            _Packet.AddInt(0);
            _Packet.AddInt(0);

            this.Variables.Encode(_Packet);

            _Packet.AddInt(0); // Hero Modes
            _Packet.AddInt(0); // UnitPreset1
            _Packet.AddInt(0); // UnitPreset2
            _Packet.AddInt(0); // UnitPreset3
            _Packet.AddInt(0); // PreviousArmySize
            _Packet.AddInt(0); // UnitCounterForEvent
            _Packet.AddInt(0); // Units Village2
            _Packet.AddInt(0); // Units Village2 new
            //_Packet.AddInt(0);


        }

        internal void Save(JObject Json)
        {
            Json.Add("avatar_id_high", this.HighID);
            Json.Add("avatar_id_low", this.LowID);
            Json.Add("name", this.Name);

            if (this.InAlliance)
            {
                //Json.Add("alliance_name", this.Alliance.Header.Name);
                //Json.Add("badge_id", this.Alliance.Header.Badge);
                //Json.Add("alliance_exp_level", this.Alliance.Header.ExpLevel);
            }
            else
                Json.Add("alliance_name", string.Empty);

            Json.Add("league_type", this.League);

            Json.Add("units", this.Units.Save());
            Json.Add("spells", this.Spells.Save());
            Json.Add("unit_upgrades", this.UnitUpgrades.Save());
            Json.Add("spell_upgrades", this.SpellUpgrades.Save());
            Json.Add("resources", this.Resources.Save());

            Json.Add("alliance_units", this.AllianceUnits.Save());

            Json.Add("hero_states", this.HeroStates.Save());
            Json.Add("hero_health", this.HeroHealth.Save());
            Json.Add("hero_modes", this.HeroModes.Save());
            Json.Add("variables", this.Variables.Save());
            Json.Add("units2", this.Units2.Save());

            Json.Add("castle_lvl", this.CastleLevel);

            Json.Add("castle_total", this.CastleTotalCapacity);
            Json.Add("castle_used", this.CastleUsedCapacity);
            Json.Add("castle_total_sp", this.CastleTotalSpellCapacity);
            Json.Add("castle_used_sp", this.CastleUsedSpellCapacity);

            Json.Add("town_hall_lvl", this.TownHallLevel);
            Json.Add("th_v2_lvl", this.TownHallLevel2);

            Json.Add("score", this.Score);
            Json.Add("duel_score", this.DuelScore);

            if (this.RedPackageState)
            {
                Json.Add("red_package_state", this.RedPackageState);
            }
        }

        internal bool HasEnoughDiamonds(int Count)
        {
            return this.Diamonds >= Count;
        }

        internal void UseDiamonds(int Count)
        {
            this.Diamonds -= Count;
            this.FreeDiamonds -= Count;

            if (this.FreeDiamonds < 0)
            {
                this.FreeDiamonds = 0;
            }
        }

        public override string ToString()
        {
            return this.HighID + "-" + this.LowID;
        }
    }
}
