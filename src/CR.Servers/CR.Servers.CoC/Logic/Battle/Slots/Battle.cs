namespace CR.Servers.CoC.Logic.Battle
{
    using System;
    using CR.Servers.CoC.Extensions;
    using CR.Servers.CoC.Logic.Battle.Slots;
    using CR.Servers.CoC.Logic.Battle.Slots.Items;
    using CR.Servers.CoC.Packets;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Battle : IBattle
    {
        [JsonProperty] internal Player Attacker;
        internal double AttackTime = 180;

        [JsonProperty] internal long BattleId;

        [JsonProperty] internal Player Defender;
        internal double LastTick;


        [JsonProperty] internal JObject Level = new JObject();

        [JsonProperty] internal int PreparationSkip;

        internal double PreparationTime = 30;

        [JsonProperty] internal Replay_Info ReplayInfo = new Replay_Info();

        [JsonProperty] internal int TimeStamp = (int) TimeUtils.ToUnixTimestamp(DateTime.UtcNow);

        internal Battle(long battle, Level attacker, Level enemy) : this()
        {
            this.BattleId = battle;

            this.Attacker = attacker.Player.Copy();
            this.Defender = enemy.Player.Copy();
            this.Level = enemy.Battle();
        }

        internal Battle()
        {
            this.Commands = new Battle_Command();
        }

        [JsonIgnore]
        public double BattleTick
        {
            get => this.PreparationTime > 0 ? this.PreparationTime : this.AttackTime;
            set
            {
                if (this.PreparationTime >= 1 && this.Commands.Count < 1)
                {
                    this.PreparationTime -= (value - this.LastTick) / 63;
                    Console.WriteLine("Preparation Time : " + TimeSpan.FromSeconds(this.PreparationTime).TotalSeconds);
                }
                else
                {
                    this.AttackTime -= (value - this.LastTick) / 63;
                    Console.WriteLine("Attack Time      : " + TimeSpan.FromSeconds(this.AttackTime).TotalSeconds);
                }
                this.LastTick = value;
                this.EndTick = (int) value;
            }
        }

        [JsonProperty]
        public int EndTick { get; set; }

        [JsonProperty]
        public Battle_Command Commands { get; set; }

        public void Add(Command Command)
        {
            if (this.PreparationTime > 0)
            {
                this.PreparationSkip = (int) Math.Round(this.PreparationTime);
            }

            this.Commands.Add(Command);
        }

        public JObject Save()
        {
            JObject Global = new JObject
            {
                {"Global", new JObject {{"GiftPackExtension", ""}}},
                {"Village1", new JObject()},
                {
                    "Village2", new JObject
                    {
                        {"TownHallMaxLevel", 6},
                        {"ScoreChangeForLosing", new JArray()},
                        {"StrengthRangeForScore", new JArray()}
                    }
                },
                {"KillSwitches", new JObject()}
            };

            JObject Json = new JObject
            {
                {"level", this.Level},
                {"attacker", this.Attacker.Save()},
                {"defender", this.Defender.Save()},
                {"end_tick", this.EndTick},
                {"timestamp", (int) TimeUtils.ToUnixTimestamp(DateTime.UtcNow)},
                {"cmd", this.Commands.Save()},
                {"calendar", new JObject()},
                {"globals", Global},
                {"prep_skip", this.PreparationSkip},
                {"battle_id", this.BattleId}
            };
            return Json;
        }

        internal void Set_Replay_Info()
        {
            foreach (Item _Slot in this.Defender.Resources)
            {
                this.ReplayInfo.Loot.Add(new[] {_Slot.Data, _Slot.Count}); // For Debug
                this.ReplayInfo.AvailableLoot.Add(new[] {_Slot.Data, _Slot.Count});
            }

            this.ReplayInfo.Stats.HomeId[0] = this.Defender.HighID;
            this.ReplayInfo.Stats.HomeId[1] = this.Defender.LowID;
            this.ReplayInfo.Stats.OriginalAttackerScore = this.Attacker.Score;
            this.ReplayInfo.Stats.OriginalDefenderScore = this.Defender.Score;

            this.ReplayInfo.Stats.AllianceName = this.Attacker.Alliance != null ? this.Attacker.Alliance.Header.Name : string.Empty;

            this.ReplayInfo.Stats.AllianceBadge = this.Attacker.Alliance?.Header.Badge ?? 0;
            this.ReplayInfo.Stats.BattleTime = 180 - (int) this.AttackTime + 1;
        }
    }
}