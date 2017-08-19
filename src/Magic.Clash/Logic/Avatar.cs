using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic
{
    internal class Avatar
    {
        // Ids

        [JsonIgnore] internal int ObstacleClearCount;
        [JsonIgnore]
        internal long UserId
        {
            get => ((long) UserHighId << 32) | (UserLowId & 0xFFFFFFFFL);
            set
            {
                UserHighId = Convert.ToInt32(value >> 32);
                UserLowId = (int) value;
            }
        }

        [JsonIgnore]
        internal long ClanId
        {
            get => ((long) ClanHighID << 32) | (ClanLowID & 0xFFFFFFFFL);
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

        /*[JsonProperty("facebook")] internal Structure.API.Facebook Facebook;
        [JsonProperty("google")] internal Structure.API.Google Google;
        [JsonProperty("gamecenter")] internal Structure.API.Gamecenter Gamecenter;
        [JsonProperty("inbox")] internal Inbox Inbox;*/

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
        }
        internal byte[] ToBytes
        {
            get
            {
                //this.Refresh();

                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(this.UserId);
                _Packet.AddLong(this.UserId);


                /*if (this.ClanId > 0)
                {
                    Clan clan = Core.Resources.Clans.Get(ClanId);

                    _Packet.AddBool(clan != null);
                    if (clan != null)
                    {
                        _Packet.AddLong(this.ClanId);
                        _Packet.AddString(clan.Name);
                        _Packet.AddInt(clan.Badge); // Badge
                        _Packet.AddInt((int)clan.Members[UserId].Role); // Role
                        _Packet.AddInt(clan.Level); // Level

                        _Packet.AddBool(false); // Alliance War
                        {
                            // _Packet.AddLong(1); // War ID
                        }

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Clan is null");
                        this.ClanId = 0;
                        this.Alliance_Role = -1;
                        this.Alliance_Level = -1;
                        this.Alliance_Name = string.Empty;
                        this.Badge_ID = -1;
                    }
                }
                else*/
                    _Packet.AddBool(false);

                _Packet.AddInt(this.Legendary_Trophies);
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
                _Packet.AddInt(this.Castle_Level);
                _Packet.AddInt(this.Castle_Total);
                _Packet.AddInt(this.Castle_Used);
                _Packet.AddInt(this.Castle_Total_SP);
                _Packet.AddInt(this.Castle_Used_SP);

                _Packet.AddInt(this.TownHall_Level);
                _Packet.AddInt(this.Builder_TownHall_Level);

#if DEBUG
                _Packet.AddString($"{this.Name} #{this.UserId}:{GameUtils.GetHashtag(this.UserId)}");
#else
                _Packet.AddString(this.Name);
#endif
                _Packet.AddString(null);
               // _Packet.AddString(!string.IsNullOrEmpty(this.Facebook.Identifier) ? this.Facebook.Identifier : null);

                _Packet.AddInt(this.Level);
                _Packet.AddInt(this.Experience);
                _Packet.AddInt(this.Resources.Gems);
                _Packet.AddInt(this.Resources.Gems);

                _Packet.AddInt(0); // 1200
                _Packet.AddInt(0); // 60

                _Packet.AddInt(this.Trophies);
                _Packet.AddInt(this.Builder_Trophies);

                _Packet.AddInt(this.Wons);
                _Packet.AddInt(this.Loses);
                _Packet.AddInt(0); // Def Wins
                _Packet.AddInt(0); // Def Loses

                _Packet.AddInt(this.Castle_Resources.Get(Resource.WarGold));
                _Packet.AddInt(this.Castle_Resources.Get(Resource.WarElixir));
                _Packet.AddInt(this.Castle_Resources.Get(Resource.WarDarkElixir));

                _Packet.AddInt(0);

                _Packet.AddBool(true);
                _Packet.AddInt(220);
                _Packet.AddInt(1828055880);

                _Packet.AddByte(this.NameState); //Name changed count

                _Packet.AddInt(this.NameState > 1 ? 1 : 0); //Name Changed

                _Packet.AddInt(6900); //6900
                _Packet.AddInt(0);
                _Packet.AddInt(this.WarState ? 1 : 0);

                _Packet.AddInt(0);
                _Packet.AddInt(0); // Total Attack with shield

                _Packet.AddBool(false); //0

                _Packet.AddDataSlots(this.Resources_Cap);

                _Packet.AddRange(this.Resources.ToBytes);

                _Packet.AddDataSlots(this.Units);
                _Packet.AddDataSlots(this.Spells);
                _Packet.AddDataSlots(this.Unit_Upgrades);
                _Packet.AddDataSlots(this.Spell_Upgrades);

                _Packet.AddDataSlots(this.Heroes_Upgrades);
                _Packet.AddDataSlots(this.Heroes_Health);
                _Packet.AddDataSlots(this.Heroes_States);

                _Packet.AddInt(this.Castle_Units.Count + this.Castle_Spells.Count);

                foreach (Alliance_Unit _Unit in this.Castle_Units)
                {
                    _Packet.AddInt(_Unit.Data);
                    _Packet.AddInt(_Unit.Count);
                    _Packet.AddInt(_Unit.Level);
                }

                foreach (Alliance_Unit _Spell in this.Castle_Spells)
                {
                    _Packet.AddInt(_Spell.Data);
                    _Packet.AddInt(_Spell.Count);
                    _Packet.AddInt(_Spell.Level);
                }


                _Packet.AddInt(this.Tutorials.Count);
                foreach (var Tutorial in this.Tutorials)
                {
                    _Packet.AddInt(Tutorial);
                }

                _Packet.AddInt(0); //Achievements
                _Packet.AddInt(0); //Achievements Progress

                _Packet.AddRange(this.Npcs.ToBytes);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddDataSlots(this.Variables);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddDataSlots(this.Units2);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                return _Packet.ToArray();
            }
        }

        internal void Add_Unit(int Data, int Count)
        {
            int _Index = this.Units.FindIndex(U => U.Data == Data);

            if (_Index > -1)
            {
                this.Units[_Index].Count += Count;
            }
            else
            {
                this.Units.Add(new Slot(Data, Count));
            }
        }

        public void Add_Spells(int Data, int Count)
        {
            int _Index = this.Spells.FindIndex(S => S.Data == Data);

            if (_Index > -1)
            {
                this.Spells[_Index].Count += Count;
            }
            else
            {
                this.Spells.Add(new Slot(Data, Count));
            }
        }

        public bool HasEnoughResources(Resource resource, int buildCost) => this.Resources.Get(resource) >= buildCost;

        public bool HasEnoughResources(int globalId, int buildCost) => this.Resources.Get(globalId) >= buildCost;
    }
}
