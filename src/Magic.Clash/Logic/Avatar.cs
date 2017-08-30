using System;
using System.Collections.Generic;
using System.Reflection;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Newtonsoft.Json;
using Npcs = Magic.ClashOfClans.Logic.Structure.Slots.Npcs;
using Resource = Magic.ClashOfClans.Logic.Enums.Resource;
using Variables = Magic.ClashOfClans.Logic.Structure.Slots.Variables;

namespace Magic.ClashOfClans.Logic
{
    internal class Avatar
    {
        // Ids

        [JsonIgnore] internal int ObstacleClearCount;

        [JsonIgnore]
        internal long UserId
        {
            get => ((long) UserHighId << 32) | UserLowId;
            set
            {
                UserHighId = Convert.ToInt32(value >> 32);
                UserLowId = (int) value;
            }
        }

        [JsonIgnore]
        internal long ClanId
        {
            get => ((long) ClanHighID << 32) | ClanLowID;
            set
            {
                ClanHighID = Convert.ToInt32(value >> 32);
                ClanLowID = (int) value;
            }
        }

        [JsonProperty("avatar_id_high")] internal int UserHighId;
        [JsonProperty("avatar_id_low")] internal int UserLowId;

        [JsonProperty("alliance_id_high")] internal int ClanHighID;
        [JsonProperty("alliance_id_low")] internal int ClanLowID;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;

        [JsonProperty("name")] internal string Name = "NoNameYet";
        [JsonProperty("IpAddress")] internal string IpAddress;
        [JsonProperty("region")] internal string Region;
        [JsonProperty("alliance_name")] internal string Alliance_Name;

        [JsonProperty("xp_level")] internal int Level = 1;
        [JsonProperty("xp")] internal int Experience;

        [JsonProperty("wins")] internal int Wons;
        [JsonProperty("loses")] internal int Loses;
        [JsonProperty("games")] internal int Games;
        [JsonProperty("win_streak")] internal int Streak;
        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;

        [JsonProperty("shield")] internal int Shield;
        [JsonProperty("guard")] internal int Guard;
        [JsonProperty("score")] internal int Trophies;
        [JsonProperty("duel_score")] internal int Builder_Trophies = 1;
        [JsonProperty("legend_troph")] internal int Legendary_Trophies;
        [JsonProperty("league_type")] internal int League;

        [JsonProperty("war_state")] internal bool WarState = true;
        [JsonProperty("name_state")] internal byte NameState;

        [JsonProperty("rank")] internal Rank Rank = Rank.PLAYER;

        [JsonProperty("town_hall_level")] internal int TownHall_Level;
        [JsonProperty("th_v2_lvl")] internal int Builder_TownHall_Level;
        [JsonProperty("castle_lvl")] internal int Castle_Level = -1;
        [JsonProperty("castle_total")] internal int Castle_Total;
        [JsonProperty("castle_used")] internal int Castle_Used;
        [JsonProperty("castle_total_sp")] internal int Castle_Total_SP;
        [JsonProperty("castle_used_sp")] internal int Castle_Used_SP;
        [JsonProperty("castle_resource")] internal Resources Castle_Resources;

        [JsonProperty("bookmarks")] internal List<long> Bookmarks = new List<long>();
        [JsonProperty("tutorials")] internal List<int> Tutorials = new List<int>();
        [JsonProperty("last_search_enemy_id")] internal List<long> Last_Attack_Enemy_ID = new List<long>();
        [JsonProperty("account_locked")] internal bool Locked;

        [JsonProperty("badge_id")] internal int Badge_ID = -1;
        [JsonProperty("wall_group_id")] internal int Wall_Group_ID = -1;
        [JsonProperty("alliance_role")] internal int Alliance_Role = -1;
        [JsonProperty("alliance_level")] internal int Alliance_Level = -1;

        [JsonProperty("units")] internal Units Units;
        [JsonProperty("units2")] internal Units Units2;
        [JsonProperty("spells")] internal Units Spells;
        [JsonProperty("alliance_units")] internal Castle_Units Castle_Units;
        [JsonProperty("alliance_spells")] internal Castle_Units Castle_Spells;


        [JsonProperty("unit_upgrades")] internal Upgrades Unit_Upgrades;
        [JsonProperty("spell_upgrades")] internal Upgrades Spell_Upgrades;
        [JsonProperty("hero_upgrade")] internal Upgrades Heroes_Upgrades;

        [JsonProperty("hero_states")] internal Slots Heroes_States;
        [JsonProperty("hero_health")] internal Slots Heroes_Health;
        [JsonProperty("hero_modes")] internal Slots Heroes_Modes;

        [JsonProperty("resources")] internal Resources Resources;
        [JsonProperty("resources_cap")] internal Resources Resources_Cap;
        [JsonProperty("npcs")] internal Npcs Npcs;
        [JsonProperty("variable")] internal Variables Variables;
        [JsonProperty("debug_mode")] internal Modes Modes;

        [JsonProperty("login_count")] internal int Login_Count;

        [JsonProperty("play_time")]
        internal TimeSpan PlayTime => DateTime.UtcNow - Created;

        [JsonProperty("last_tick")] internal DateTime LastTick = DateTime.UtcNow;
        [JsonProperty("last_save")] internal DateTime LastSave = DateTime.UtcNow;
        [JsonProperty("creation_date")] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty("ban_date")] internal DateTime BanTime = DateTime.UtcNow;


        [JsonProperty("inbox")] internal Inbox Inbox;
        /*[JsonProperty("facebook")] internal Structure.API.Facebook Facebook;
        [JsonProperty("google")] internal Structure.API.Google Google;
        [JsonProperty("gamecenter")] internal Structure.API.Gamecenter Gamecenter;*/

        internal bool Banned => BanTime > DateTime.UtcNow;

        public Avatar()
        {
            Castle_Resources = new Resources();
            Resources = new Resources();
            Resources_Cap = new Resources();
            Npcs = new Npcs();
            Variables = new Variables();
            Modes = new Modes();

            Units = new Units();
            Units2 = new Units();
            Spells = new Units();
            Castle_Units = new Castle_Units();
            Castle_Spells = new Castle_Units();

            Unit_Upgrades = new Upgrades();
            Spell_Upgrades = new Upgrades();
            Heroes_Upgrades = new Upgrades();

            Heroes_Health = new Slots();
            Heroes_Modes = new Slots();
            Heroes_States = new Slots();

            Inbox = new Inbox(this);
        }

        public Avatar(long id)
        {
            UserId = id;

            Castle_Resources = new Resources(false);
            Resources = new Resources(true);
            Resources_Cap = new Resources(false);
            Npcs = new Npcs();
            Variables = new Variables(true);
            Modes = new Modes(true);

            Units = new Units();
            Units2 = new Units();
            Spells = new Units();
            Castle_Units = new Castle_Units();
            Castle_Spells = new Castle_Units();

            Unit_Upgrades = new Upgrades();
            Spell_Upgrades = new Upgrades();
            Heroes_Upgrades = new Upgrades();

            Heroes_Health = new Slots();
            Heroes_Modes = new Slots();
            Heroes_States = new Slots();
                
            Inbox = new Inbox(this);
        }

        internal byte[] ToBytes
        {
            get
            {
                //this.Refresh();

                var _Packet = new List<byte>();

                _Packet.AddLong(UserId);
                _Packet.AddLong(UserId);


                if (ClanId > 0)
                {
                    var clan = ObjectManager.GetAlliance(ClanId);

                    if (clan != null)
                    {
                        var member = clan.Members.Get(UserId);
                        _Packet.AddBool(member != null);
                        if (member != null)
                        {
                            _Packet.AddLong(ClanId);
                            _Packet.AddString(clan.Name);
                            _Packet.AddInt(clan.Badge); // Badge
                            _Packet.AddInt((int) member.Role); // Role
                            _Packet.AddInt(clan.Level); // Level

                            _Packet.AddBool(false); // Alliance War
                            {
                                // _Packet.AddLong(1); // War ID
                            }
                        }
                        else
                        {
                            ExceptionLogger.Log(new NullReferenceException(),
                                $"Member {UserId} is null for clan {ClanId}");
                            ClanId = 0;
                            Alliance_Role = -1;
                            Alliance_Level = -1;
                            Alliance_Name = string.Empty;
                            Badge_ID = -1;
                        }
                    }
                    else
                    {
                        _Packet.AddBool(false);
                        ExceptionLogger.Log(new NullReferenceException(), $"Clan {ClanId} is null for avatar {UserId}");
                        ClanId = 0;
                        Alliance_Role = -1;
                        Alliance_Level = -1;
                        Alliance_Name = string.Empty;
                        Badge_ID = -1;
                    }
                }
                else
                {
                    _Packet.AddBool(false);
                }

                _Packet.AddInt(Legendary_Trophies);
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

                _Packet.AddInt(League);
                _Packet.AddInt(Castle_Level);
                _Packet.AddInt(Castle_Total);
                _Packet.AddInt(Castle_Used);
                _Packet.AddInt(Castle_Total_SP);
                _Packet.AddInt(Castle_Used_SP);

                _Packet.AddInt(TownHall_Level);
                _Packet.AddInt(Builder_TownHall_Level);

#if DEBUG
                _Packet.AddString($"{Name} #{UserId}:{GameUtils.GetHashtag(UserId)}");
#else
                _Packet.AddString(this.Name);
#endif
                _Packet.AddString(null);
                // _Packet.AddString(!string.IsNullOrEmpty(this.Facebook.Identifier) ? this.Facebook.Identifier : null);

                _Packet.AddInt(Level);
                _Packet.AddInt(Experience);
                _Packet.AddInt(Resources.Gems);
                _Packet.AddInt(Resources.Gems);

                _Packet.AddInt(0); // 1200
                _Packet.AddInt(0); // 60

                _Packet.AddInt(Trophies);
                _Packet.AddInt(Builder_Trophies);

                _Packet.AddInt(Wons);
                _Packet.AddInt(Loses);
                _Packet.AddInt(0); // Def Wins
                _Packet.AddInt(0); // Def Loses

                _Packet.AddInt(Castle_Resources.Get(Resource.WarGold));
                _Packet.AddInt(Castle_Resources.Get(Resource.WarElixir));
                _Packet.AddInt(Castle_Resources.Get(Resource.WarDarkElixir));

                _Packet.AddInt(0);

                _Packet.AddBool(true);
                _Packet.AddInt(220);
                _Packet.AddInt(1828055880);

                _Packet.AddByte(NameState); //Name changed count

                _Packet.AddInt(NameState > 1 ? 1 : 0); //Name Changed

                _Packet.AddInt(6900); //6900
                _Packet.AddInt(0);
                _Packet.AddInt(WarState ? 1 : 0);

                _Packet.AddInt(0);
                _Packet.AddInt(0); // Total Attack with shield

                _Packet.AddBool(false); //0

                _Packet.AddDataSlots(Resources_Cap);

                _Packet.AddRange(Resources.ToBytes);

                _Packet.AddDataSlots(Units);
                _Packet.AddDataSlots(Spells);
                _Packet.AddDataSlots(Unit_Upgrades);
                _Packet.AddDataSlots(Spell_Upgrades);

                _Packet.AddDataSlots(Heroes_Upgrades);
                _Packet.AddDataSlots(Heroes_Health);
                _Packet.AddDataSlots(Heroes_States);

                _Packet.AddInt(Castle_Units.Count + Castle_Spells.Count);

                foreach (var _Unit in Castle_Units)
                {
                    _Packet.AddInt(_Unit.Data);
                    _Packet.AddInt(_Unit.Count);
                    _Packet.AddInt(_Unit.Level);
                }

                foreach (var _Spell in Castle_Spells)
                {
                    _Packet.AddInt(_Spell.Data);
                    _Packet.AddInt(_Spell.Count);
                    _Packet.AddInt(_Spell.Level);
                }


                _Packet.AddInt(Tutorials.Count);
                foreach (var Tutorial in Tutorials)
                    _Packet.AddInt(Tutorial);

                _Packet.AddInt(0); //Achievements
                _Packet.AddInt(0); //Achievements Progress

                _Packet.AddRange(Npcs.ToBytes);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddDataSlots(Variables);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddDataSlots(Units2); //Retrain

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                return _Packet.ToArray();
            }
        }


        internal static int GetDataIndex(List<Slot> dsl, Data d)
        {
            return dsl.FindIndex(ds => ds.Data == d.Id);
        }

        internal static int GetDataIndex(Units dsl, Data d)
        {
            return dsl.FindIndex(ds => ds.Data == d.Id);
        }

        internal int Get_Unit_Count_V2(Combat_Item cd)
        {
            var result = 0;
            var index = GetDataIndex(Units2, cd);
            if (index != -1)
                result = Units2[index].Count;
            return result;
        }

        internal void Set_Unit_Count_V2(Combat_Item cd, int count)
        {
            var index = GetDataIndex(Units2, cd);
            if (index != -1)
            {
                Units2[index].Count = count;
            }
            else
            {
                var ds = new Slot(cd.GetGlobalId(), count);
                Units2.Add(ds);
            }
        }

        internal void Add_Unit2(int Data, int Count)
        {
            var _Index = Units2.FindIndex(U => U.Data == Data);

            if (_Index > -1)
                Units2[_Index].Count += Count;
            else
                Units2.Add(new Slot(Data, Count));
        }

        internal void Add_Unit(int Data, int Count)
        {
            var _Index = Units.FindIndex(U => U.Data == Data);

            if (_Index > -1)
                Units[_Index].Count += Count;
            else
                Units.Add(new Slot(Data, Count));
        }

        public void Add_Spells(int Data, int Count)
        {
            var _Index = Spells.FindIndex(S => S.Data == Data);

            if (_Index > -1)
                Spells[_Index].Count += Count;
            else
                Spells.Add(new Slot(Data, Count));
        }

        public int GetUnitUpgradeLevel(Combat_Item cd)
        {
            var result = 0;
            switch (cd.GetCombatItemType())
            {
                case 2:
                {
                    var index = GetDataIndex(Heroes_Upgrades, cd);
                    if (index != -1)
                        result = Heroes_Upgrades[index].Count;
                    break;
                }
                case 1:
                {
                    var index = GetDataIndex(Spell_Upgrades, cd);
                    if (index != -1)
                        result = Spell_Upgrades[index].Count;
                    break;
                }

                default:
                {
                    var index = GetDataIndex(Unit_Upgrades, cd);
                    if (index != -1)
                        result = Unit_Upgrades[index].Count;
                    break;
                }
            }
            return result;
        }

        public void SetUnitUpgradeLevel(Combat_Item cd, int level)
        {
            switch (cd.GetCombatItemType())
            {
                case 2:
                {
                    var index = GetDataIndex(Heroes_Upgrades, cd);
                    if (index != -1)
                    {
                        Heroes_Upgrades[index].Count = level;
                    }
                    else
                    {
                        var ds = new Slot(cd.GetGlobalId(), level);
                        Heroes_Upgrades.Add(ds);
                    }
                    break;
                }
                case 1:
                {
                    var index = GetDataIndex(Spell_Upgrades, cd);
                    if (index != -1)
                    {
                        Spell_Upgrades[index].Count = level;
                    }
                    else
                    {
                        var ds = new Slot(cd.GetGlobalId(), level);
                        Spell_Upgrades.Add(ds);
                    }
                    break;
                }
                default:
                {
                    var index = GetDataIndex(Unit_Upgrades, cd);
                    if (index != -1)
                    {
                        Unit_Upgrades[index].Count = level;
                    }
                    else
                    {
                        var ds = new Slot(cd.GetGlobalId(), level);
                        Unit_Upgrades.Add(ds);
                    }
                    break;
                }
            }
        }

        public void SetHeroHealth(Heroes hd, int health)
        {
            var index = GetDataIndex(Heroes_Health, hd);
            if (index == -1)
            {
                var ds = new Slot(hd.GetGlobalId(), health);
                Heroes_Health.Add(ds);
            }
            else
            {
                Heroes_Health[index].Count = health;
            }
        }

        public void SetHeroState(Heroes hd, int state)
        {
            var index = GetDataIndex(Heroes_States, hd);
            if (index == -1)
            {
                var ds = new Slot(hd.GetGlobalId(), state);
                Heroes_States.Add(ds);
            }
            else
            {
                Heroes_States[index].Count = state;
            }
        }


        public bool HasEnoughResources(Resource resource, int buildCost)
        {
            return Resources.Get(resource) >= buildCost;
        }

        public bool HasEnoughResources(int globalId, int buildCost)
        {
            return Resources.Get(globalId) >= buildCost;
        }
        internal void ShowValues()
        {
            Console.WriteLine(Environment.NewLine);

            foreach (var Field in GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                if (Field != null)
                    Logger.SayInfo(Utils.Padding(GetType().Name) + " - " + Utils.Padding(Field.Name) + " : " +
                                   Utils.Padding(
                                       !string.IsNullOrEmpty(Field.Name)
                                           ? (Field.GetValue(this) != null ? Field.GetValue(this).ToString() : "(null)")
                                           : "(null)", 40));
        }
    }
}
