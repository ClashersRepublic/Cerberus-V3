using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Magic.Royale.Extensions;
using Magic.Royale.Extensions.List;
using Magic.Royale.Files.CSV_Helpers;
using Magic.Royale.Files.CSV_Logic;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Logic.Structure.Components;
using Magic.Royale.Logic.Structure.Slots;
using Magic.Royale.Logic.Structure.Slots.Items;
using Newtonsoft.Json;
using Resource = Magic.Royale.Logic.Enums.Resource;

namespace Magic.Royale.Logic
{
    internal class Avatar
    {
        [JsonIgnore] internal float RareChance;
        [JsonIgnore] internal float EpicChance;
        [JsonIgnore] internal float LegendaryChance;

        [JsonIgnore] internal Device Device;
        [JsonIgnore] internal Component Component;

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
        [JsonProperty("active_deck")] internal int Active_Deck;

        [JsonProperty("wins")] internal int Wons;
        [JsonProperty("loses")] internal int Loses;
        [JsonProperty("games")] internal int Games;
        [JsonProperty("win_streak")] internal int Streak;
        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;

        [JsonProperty("shield")] internal int Shield;
        [JsonProperty("guard")] internal int Guard;
        [JsonProperty("score")] internal int Trophies;
        [JsonProperty("legend_troph")] internal int Legendary_Trophies;
        [JsonProperty("league_type")] internal int League;

        [JsonProperty("war_state")] internal bool WarState = true;
        [JsonProperty("name_state")] internal byte NameState;

        [JsonProperty("rank")] internal Rank Rank = Rank.PLAYER;

        [JsonProperty("bookmarks")] internal List<long> Bookmarks = new List<long>();
        [JsonProperty("tutorials")] internal List<int> Tutorials = new List<int>();
        [JsonProperty("last_search_enemy_id")] internal List<long> Last_Attack_Enemy_ID = new List<long>();
        [JsonProperty("account_locked")] internal bool Locked;

        [JsonProperty("badge_id")] internal int Badge_ID = -1;
        [JsonProperty("alliance_role")] internal int Alliance_Role = -1;
        [JsonProperty("alliance_level")] internal int Alliance_Level = -1;

        [JsonProperty("resources")] internal Resources Resources;
        [JsonProperty("decks")] internal Decks Decks;
        [JsonProperty("cards")] internal Cards Cards;
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

        [JsonConstructor]
        public Avatar()
        {
            Console.WriteLine("Yo");
            Cards = new Cards(this);
            Decks = new Decks(this);
            Component = new Component(this);
            Resources = new Resources();
            Modes = new Modes();
        }

        public Avatar(long id)
        {
            UserId = id;
            Component = new Component(this);
            Decks = new Decks(this);
            Cards = new Cards(this);
            Resources = new Resources(true);
            Modes = new Modes(true);
        }

        public bool HasEnoughResources(Resource resource, int buildCost)
        {
            return Resources.Get(resource) >= buildCost;
        }

        public bool HasEnoughResources(int globalId, int buildCost)
        {
            return Resources.Get(globalId) >= buildCost;
        }
    }
}
