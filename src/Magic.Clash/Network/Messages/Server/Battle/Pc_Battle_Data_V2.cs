using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Network.Messages.Server.Battle
{
    internal class Pc_Battle_Data_V2 : Message
    {
        internal JsonSerializerSettings Client_JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        internal Level Enemy;
        internal JObject EnemyObject;

        public Pc_Battle_Data_V2(Device Device, Level Enemy) : base(Device)
        {
            Identifier = 25023;
            this.Enemy = Enemy;
            EnemyObject = Enemy.GameObjectManager.Save();
        }

        public override void Encode()
        {
            Data.AddRange(Device.Player.Avatar.ToBytes);
            Data.AddLong(Enemy.Avatar.UserId); //Opponent id
            Data.AddInt(0);
            Data.AddInt(0);
            Data.AddInt(0);
            Data.AddCompressed(Json);
            Data.AddCompressed(Game_Events.Events_Json);
            Data.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
        }

        public override void Process()
        {
            Device.State = State.IN_1VS1_BATTLE;
        }

        internal string Json => JsonConvert.SerializeObject(new
        {
            exp_ver = 1,
            buildings = EnemyObject.SelectToken("buildings"),
            obstacles = EnemyObject.SelectToken("obstacles"),
            traps = EnemyObject.SelectToken("traps"),
            decos = EnemyObject.SelectToken("decos"),
            vobjs = EnemyObject.SelectToken("vobjs"),
            buildings2 = EnemyObject.SelectToken("buildings2"),
            obstacles2 = EnemyObject.SelectToken("obstacles2"),
            traps2 = EnemyObject.SelectToken("traps2"),
            decos2 = EnemyObject.SelectToken("decos2"),
            vobjs2 = EnemyObject.SelectToken("vobjs2"),
            avatar_id_high = Enemy.Avatar.UserHighId,
            avatar_id_low = Enemy.Avatar.UserLowId,
            name = Enemy.Avatar.Name,
            alliance_name = Enemy.Avatar.Alliance_Name,
            xp_level = Enemy.Avatar.Level,
            alliance_id_high = Enemy.Avatar.ClanHighID,
            alliance_id_low = Enemy.Avatar.ClanLowID,
            badge_id = Enemy.Avatar.Badge_ID,
            alliance_exp_level = Enemy.Avatar.Alliance_Level,
            alliance_unit_visit_capacity = 0,
            alliance_unit_spell_visit_capacity = 0,
            league_type = Enemy.Avatar.League,
            resources = Enemy.Avatar.Resources,
            alliance_units = Enemy.Avatar.Castle_Units,
            hero_states = Enemy.Avatar.Heroes_States,
            hero_health = Enemy.Avatar.Heroes_Health,
            hero_upgrade = Enemy.Avatar.Heroes_Upgrades,
            hero_modes = Enemy.Avatar.Heroes_Modes,
            variables = Enemy.Avatar.Variables,
            castle_lvl = Enemy.Avatar.Castle_Level,
            castle_total = Enemy.Avatar.Castle_Total,
            castle_used = Enemy.Avatar.Castle_Used,
            castle_total_sp = Enemy.Avatar.Castle_Total_SP,
            castle_used_sp = Enemy.Avatar.Castle_Used_SP,
            town_hall_lvl = Enemy.Avatar.TownHall_Level,
            th_v2_lvl = Enemy.Avatar.Builder_TownHall_Level,
            score = Enemy.Avatar.Trophies,
            duel_score = Enemy.Avatar.Builder_Trophies
        }, Client_JsonSettings);
    }
}